using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SFB;




// New pipeline:
// Gcode -> Parsed padded CSV -> mcode CSV -> Datapack

// Gcode:
//		;This is a gcode comment
//		G1 X5 Y5 Z0 E-0.2 F900; wipe and retract
//		G1 X5 Y200 E0.14 ; perimeter
//		G1 X5 Y200 Z5 E-0.14 ; perimeter

// Parsed padded CSV:  Xcord,Ycord,Zcord,ShouldExtrude,MoveSpeed
//		5,0,5,-0.2,900
//		5,0,200,0.14,900
//		5,5,200,-0.14,900

// mcode CSV:   Xcord,Ycord,Zcord,XMotion,YMotion,ZMotion,ShouldExtrude
//		0,0,0,0,0,0,0
//		5,0,5,0.7071068,0,0.7071068,0
//		5,0,200,0,0,1,1
//		5,5,200,0,1,0,0

	
/// <summary>
/// Manager responsible for creating all files that make up the Minecraft datapack
/// </summary>
public class DatapackManager
{
	private const int _numberOfIORetryAttempts = 5;

	// String Constants
	private const string c_StartFunctionSuffix = "_start";
	private const string c_StopFunctionSuffix = "_stop";
	private const string c_PauseFunctionSuffix = "_pause";
	private const string c_ScoreboardPrefix = "gp_";
	private const string c_Line = "line";
	private const string c_MainDatapackName = "GcodePrinter";
	private const string C_UpdateCodeLineName = "update_code_line";
	private const string C_ExecuteMcodeName = "execute_mcode";

	// Template file names
	private const string C_UpdateCodeLine = "update_code_line.mcfunction";
	private const string C_ExecuteMcode = "execute_mcode.mcfunction";

	private const string C_TemplateLineNoFill = "template_line_no_fill.mcfunction";
	private const string C_TemplateLineWithFill = "template_line_with_fill.mcfunction";
	private const string C_TemplateUpdateCode = "template_update_code.mcfunction";
	private const string C_TemplateExecuteLine = "template_execute_line.mcfunction";
	private const string C_TemplateFinishedLine = "template_finished_code.mcfunction";

	// Template constants
	private const string C_FillBlock = "FILLBLOCK";
	private const string C_LineNum_1 = "LINENUM-1";
	private const string C_LineNum = "LINENUM";
	private const string C_XNum = "XNUM";
	private const string C_YNum = "YNUM";
	private const string C_ZNum = "ZNUM";

	//Template Names
	private const string C_TemplateName = "TemplateDatapack";
	private const string C_TemplateNamespace = "mcode1";

	// Datapack hardcoded names
	private const string C_Data = "data";
	private const string c_Minecraft = "minecraft";
	private const string c_Functions = "functions";
	private const string c_McFunction = ".mcfunction";
	private const string c_Tags = "tags";
	private const string c_FakePlayerChar = "#";
	private const string C_Slash = "/";

	private string[] _excludeExtensions = new string[1] { ".meta" };
	private string _gcodeFilePath = "";     // Path of gcode file on disk
	private string _gcodeFileName = "";     // Main name of gcode file (without .gcode)
	private string _dateCreated = "";       // Date datapack was created (almost UUID)
	private string _datapackUUID = "";      // Simulates a UUID because the use case of this program is low
	private string _datapackName = "";      // Name of the datapack
	private string _shortUUID = "";         // Name used to make scoreboard values unique (10 chars or less)
	private string _fakePlayerName = "";    // The fake player name that datapack will use (39 chars or less)
	private string _outputRoot = "";        // The root folder where the datapack will be saved to

	// Datapack folder and file paths
	private string _datapackRootPath = "";  // Path where datapack will be saved on disk ----- data
	private string _dataFolderPath = "";    // Data folder within datapack ------------------- datapack/data
	private string _namespacePath = "";     // Namespace within datapack --------------------- datapack/data/namespace
	private string _namespaceFunctions = "";// Where all functions will generate ------------- datapack/data/namespace/functions
	private string _printFunctions = "";    // PrintFunction folder within datapack ---------- datapack/data/print/functions
	private string _datapackStart = "";     // File name of mcode printing start function ---- datapack/data/print/functions/start.mcfunction
	private string _datapackStop = "";      // File name of mcode printing stop function ----- datapack/data/print/functions/stop.mcfunction
	private string _datapackPause = "";     // File name of mcode printing poause function --- datapack/data/print/functions/pause.mcfunction
	private string _datapackMcFuncTags = "";// File path for --------------------------------- datapack/data/minecraft/tags/functions

	private Dictionary<string, string> _keyVars = new Dictionary<string, string>();

	public DatapackManager(in ParsedDataStats dataStats)
	{
		_gcodeFileName = SafeFileManagement.GetFileName(Path.GetFileName(dataStats.gcodePath)).ToLower();
		_dateCreated = SafeFileManagement.GetDateNow();
		_datapackUUID = _gcodeFileName + "_" + _dateCreated;
		_datapackName = c_MainDatapackName + "_" + _datapackUUID;
		_shortUUID = _datapackUUID.FirstLast5();
		_fakePlayerName = c_FakePlayerChar + _datapackUUID.Truncate(-30);

		_outputRoot = SafeFileManagement.FolderPath("Select where datapack will be saved");
		if (!string.IsNullOrWhiteSpace(_outputRoot))
		{
			CopyTemplateAndRename();
			RenameFiles();
			UpdateCopiedFiles();

			WriteMinecraftCodeFiles(dataStats.totalLines);

			Debug.Log("Finished!");
		}
	}

	private void WriteMinecraftCodeFiles(int totalLines)
	{
		// Get the template file contense
		string templateLineFill = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateLineWithFill));
		string templateLineNoFill = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateLineNoFill));
		string templateUpdateCode = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateUpdateCode));
		string templateExecuteLine = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateExecuteLine));
		string templateFinishedLine = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateFinishedLine));

		int factor = 1000;
		int factorPow2 = factor * factor;
		int facotrPow3 = factorPow2 * factor;
		int lineAmount = totalLines + 1;

		string lvl1Folder = "level1code0_" + (facotrPow3 - 1);
		string lvl1Root = Path.Combine(_namespaceFunctions, lvl1Folder);
		Directory.CreateDirectory(lvl1Root);

		string lvl1ExecuteCode = "";
		string lvl1UpdateCode = "";

		for (int lvl1 = 0; lvl1 < facotrPow3 && lvl1 < lineAmount; lvl1 += factorPow2)
		{
			string lvl2Folder = "level2code" + lvl1 + "_" + (lvl1 + factorPow2 - 1);
			string lvl2Root = Path.Combine(lvl1Root, lvl2Folder);
			Directory.CreateDirectory(lvl2Root);

			string lvl2ExecuteCode = "";
			string lvl2UpdateCode = "";

			string lvl1CurrentExecute = templateExecuteLine;
			lvl1CurrentExecute = lvl1CurrentExecute.Replace(C_LineNum_1, lvl1 + ".." + (lvl1 + factorPow2 - 1));
			lvl1CurrentExecute = lvl1CurrentExecute.Replace(C_LineNum, DatapackPath(lvl1Folder, lvl2Folder, C_ExecuteMcodeName));
			lvl1ExecuteCode += lvl1CurrentExecute;

			string lvl1CurrentUpdateCode = templateExecuteLine;
			lvl1CurrentUpdateCode = lvl1CurrentUpdateCode.Replace(C_LineNum_1, (lvl1) + ".." + (lvl1 + factorPow2));
			lvl1CurrentUpdateCode = lvl1CurrentUpdateCode.Replace(C_LineNum, DatapackPath(lvl1Folder, lvl2Folder, C_UpdateCodeLineName));
			lvl1UpdateCode += lvl1CurrentUpdateCode;

			for (int lvl2 = lvl1; lvl2 < lvl1 + 1 * factorPow2 && lvl2 < lineAmount; lvl2 += factor)
			{
				string lvl3ExecuteCode = "";
				string lvl3UpdateCode = "";

				string lvl3Folder = "level3code" + lvl2 + "_" + (lvl2 + factor - 1);
				string lvl3Root = Path.Combine(lvl2Root, lvl3Folder);
				string localFolderRoot = DatapackPath(lvl1Folder, lvl2Folder, lvl3Folder);

				Directory.CreateDirectory(lvl3Root);

				string lvl2CurrentExecute = templateExecuteLine;
				lvl2CurrentExecute = lvl2CurrentExecute.Replace(C_LineNum_1, lvl2 + ".." + (lvl2 + factor - 1));
				lvl2CurrentExecute = lvl2CurrentExecute.Replace(C_LineNum, DatapackPath(localFolderRoot, C_ExecuteMcodeName));
				lvl2ExecuteCode += lvl2CurrentExecute;

				string lvl2CurrentUpdateCode = templateExecuteLine;
				lvl2CurrentUpdateCode = lvl2CurrentUpdateCode.Replace(C_LineNum_1, (lvl2) + ".." + (lvl2 + factor));
				lvl2CurrentUpdateCode = lvl2CurrentUpdateCode.Replace(C_LineNum, DatapackPath(localFolderRoot, C_UpdateCodeLineName));
				lvl2UpdateCode += lvl2CurrentUpdateCode;

				for (int lvl3 = lvl2; lvl3 < lvl2 + 1 * factor && lvl3 < lineAmount; lvl3++)
				{
					string filePath = Path.Combine(lvl3Root, c_Line + lvl3 + c_McFunction);
					string currentLineCode;
					
					// Make sure to add special ending line when the last line is written
					if (lvl3 != lineAmount - 1)
					{
						currentLineCode = _mcodeData.data[lvl3].extrude ? templateLineFill : templateLineNoFill;
						Vector3 motionData = _mcodeData.data[lvl3].motion;
						currentLineCode = currentLineCode.Replace(C_XNum, motionData.x.ToString("F10"));
						currentLineCode = currentLineCode.Replace(C_YNum, motionData.y.ToString("F10"));
						currentLineCode = currentLineCode.Replace(C_ZNum, motionData.z.ToString("F10"));
						currentLineCode = currentLineCode.Replace(C_FillBlock, "stone");

						string currentUpdateCode = templateUpdateCode;
						currentUpdateCode = currentUpdateCode.Replace(C_LineNum, (lvl3).ToString());
						Vector3 posData = _mcodeData.data[lvl3].pos;
						currentUpdateCode = currentUpdateCode.Replace(C_XNum, posData.x.ToString("F10"));
						currentUpdateCode = currentUpdateCode.Replace(C_YNum, posData.y.ToString("F10"));
						currentUpdateCode = currentUpdateCode.Replace(C_ZNum, posData.z.ToString("F10"));
						lvl3UpdateCode += currentUpdateCode;
					}
					else
					{
						currentLineCode = templateFinishedLine;
					}

					SafeFileManagement.SetFileContents(filePath, currentLineCode);

					string currentExecute = templateExecuteLine;
					currentExecute = currentExecute.Replace(C_LineNum_1, (lvl3).ToString());
					currentExecute = currentExecute.Replace(C_LineNum, DatapackPath(localFolderRoot, c_Line + (lvl3)));
					lvl3ExecuteCode += currentExecute;
				}

				// Save strings to files
				SafeFileManagement.SetFileContents(Path.Combine(lvl3Root, C_ExecuteMcode), lvl3ExecuteCode);
				SafeFileManagement.SetFileContents(Path.Combine(lvl3Root, C_UpdateCodeLine), lvl3UpdateCode);
			}

			// Save strings to files
			SafeFileManagement.SetFileContents(Path.Combine(lvl2Root, C_ExecuteMcode), lvl2ExecuteCode);
			SafeFileManagement.SetFileContents(Path.Combine(lvl2Root, C_UpdateCodeLine), lvl2UpdateCode);

		}

		// Save strings to files
		SafeFileManagement.SetFileContents(Path.Combine(_namespaceFunctions, C_ExecuteMcode), lvl1ExecuteCode);
		SafeFileManagement.SetFileContents(Path.Combine(_namespaceFunctions, C_UpdateCodeLine), lvl1UpdateCode);

		// Clean up datapack folder templates
		SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateLineWithFill));
		SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateLineNoFill));
		SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateUpdateCode));
		SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateExecuteLine));
	}

	/// <summary>
	/// Populates the keyVar dictionary with all terms that should be replaced within the copied minecraft files
	/// </summary>
	private void InitulizeKeyVars()
	{
		string scoreboardVar = c_ScoreboardPrefix + _shortUUID;
		string tag = "Tag" + _datapackUUID;
		_keyVars[C_TemplateNamespace] = _datapackUUID;
		_keyVars["gp_ArgVar001"] = scoreboardVar + "001";
		_keyVars["gp_ArgVar002"] = scoreboardVar + "002";
		_keyVars["#fakePlayerVar"] = _fakePlayerName;
		_keyVars["TagPrintGroup"] = tag + "PrintGroup";
		_keyVars["TagCenterPoint"] = tag + "CenterPoint";
		_keyVars["TagHome"] = tag + "Home";
		_keyVars["TagPrintHead"] = tag + "PrintHead";
		_keyVars["TagNode"] = tag + "Node";
	}

	private void UpdateCopiedFiles()
	{
		InitulizeKeyVars();
		UpdateAllCopiedFiles(_datapackMcFuncTags);
		UpdateAllCopiedFiles(_printFunctions);
		UpdateAllCopiedFiles(_namespaceFunctions);
	}

	/// <summary>
	/// Update all files within a directory to correct varible names
	/// </summary>
	/// <param name="folderPath">In folder path</param>
	private void UpdateAllCopiedFiles(string folderPath)
	{
		if (Directory.Exists(folderPath))
		{
			string[] files = SafeFileManagement.GetFilesPaths(folderPath, _numberOfIORetryAttempts);
			foreach (string file in files)
			{
				//print("Updating: " + file);
				UpdateInFileVars(file);
			}
		}
	}

	/// <summary>
	/// Replaces all instaces of varibles within a given file
	/// </summary>
	/// <param name="filePath">In file path</param>
	private void UpdateInFileVars(string filePath)
	{
		string fileContents = SafeFileManagement.GetFileContents(filePath);
		fileContents = ReplaceStringVars(fileContents);
		SafeFileManagement.SetFileContents(filePath, fileContents);
	}

	/// <summary>
	/// Replace all instaces of keys within given string value
	/// </summary>
	/// <param name="fileContents">The contents of a file to be changed</param>
	/// <returns></returns>
	private string ReplaceStringVars(string fileContents)
	{
		foreach (string key in _keyVars.Keys)
		{
			fileContents = fileContents.Replace(key, _keyVars[key]);
		}
		return fileContents;
	}

	/// <summary>
	/// Rename the start, pause, and stop files
	/// </summary>
	private void RenameFiles()
	{
		_printFunctions = Path.Combine(_dataFolderPath, "print", "functions");
		string templateStart = Path.Combine(_printFunctions, C_TemplateNamespace + c_StartFunctionSuffix + c_McFunction);
		_datapackStart = Path.Combine(_printFunctions, _datapackUUID + c_StartFunctionSuffix + c_McFunction);
		if (SafeFileManagement.MoveFile(templateStart, _datapackStart, _numberOfIORetryAttempts))
		{
			string templateStop = Path.Combine(_printFunctions, C_TemplateNamespace + c_StopFunctionSuffix + c_McFunction);
			_datapackStop = Path.Combine(_printFunctions, _datapackUUID + c_StopFunctionSuffix + c_McFunction);
			if(SafeFileManagement.MoveFile(templateStop, _datapackStop, _numberOfIORetryAttempts))
			{
				string templatePause = Path.Combine(_printFunctions, C_TemplateNamespace + c_PauseFunctionSuffix + c_McFunction);
				_datapackPause = Path.Combine(_printFunctions, _datapackUUID + c_PauseFunctionSuffix + c_McFunction);
				SafeFileManagement.MoveFile(templatePause, _datapackPause, _numberOfIORetryAttempts);
			}
		}
	}

	/// <summary>
	/// Copy folders and files from datapack tempate then rename folders
	/// </summary>
	private void CopyTemplateAndRename()
	{
		string pathOfDatapackTemplate = Path.Combine(Application.dataPath, "StreamingAssets", "CopyTemplate");
		SafeFileManagement.DirectoryCopy(pathOfDatapackTemplate, _outputRoot, true, _excludeExtensions, _numberOfIORetryAttempts);

		// Rename main datapack folder
		string templateOutput = Path.Combine(_outputRoot, C_TemplateName);
		_datapackRootPath = Path.Combine(_outputRoot, _datapackName);
		if (SafeFileManagement.MoveDirectory(templateOutput, _datapackRootPath, _numberOfIORetryAttempts))
		{
			// Rename namespace folder
			_dataFolderPath = Path.Combine(_datapackRootPath, C_Data);
			string templateNamespace = Path.Combine(_dataFolderPath, C_TemplateNamespace);
			_namespacePath = Path.Combine(_dataFolderPath, _datapackUUID);
			SafeFileManagement.MoveDirectory(templateNamespace, _namespacePath, _numberOfIORetryAttempts);

			_namespaceFunctions = Path.Combine(_namespacePath, c_Functions);
			_datapackMcFuncTags = Path.Combine(_dataFolderPath, c_Minecraft, c_Tags, c_Functions);
		}
	}
	

	private string DatapackPath(params string[] values)
	{
		if (values.Length < 2)
			return values[0];

		string newString = "";
		foreach(string part in values)
		{
			newString += part + C_Slash;
		}

		return newString.TrimEnd(newString[newString.Length - 1]);

	}
	/// <summary>
	/// Prints all vars to unity console
	/// </summary>
	private void LogDynamicVars()
	{
		Debug.Log("_gcodeFilePath: " + _gcodeFilePath + " \n" +
			"_gcodeFileName: " + _gcodeFileName + "\n" +
			"_dateCreated: " + _dateCreated + "\n" +
			"_datapackUUID: " + _datapackUUID + "\n" +
			"_datapackName: " + _datapackName + "\n" +
			"_shortName: " + _shortUUID + "\n" +
			"_fakePlayerName: " + _fakePlayerName);
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SFB;

/// <summary>
/// Manager responsible for creating all files that make up the Minecraft datapack
/// </summary>
public class DatapackManager : MonoBehaviour
{
	// Mcode properties
	private float _magnitudeScalar = 0.001f;
	private float _maxMagnitude = 2f;

	private const int _numberOfIORetryAttempts = 5;
	
	// String Constants
	private const string c_StartFunctionSuffix = "_start";
	private const string c_StopFunctionSuffix = "_stop";
	private const string c_ScoreboardPrefix = "gp_";
	private const string c_Line = "line";
	private const string c_MainDatapackName = "GcodePrinter";

	//Template Names
	private const string c_TemplateName = "TemplateDatapack";
	private const string c_TemplateNamespace = "mcode1";

	// Datapack hardcoded names
	private const string c_Data = "data";
	private const string c_Minecraft = "minecraft";
	private const string c_Functions = "functions";
	private const string c_McFunction = ".mcfunction";
	private const string c_Tags = "tags";
	private const string c_FakePlayerChar = "#";

	[SerializeField] private FileManager _fileManager = null;
	
	private string _pathOfDatapackTemplate = "";	// Used to copy the common files needed for every mcode datapack

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
	private string _datapackRootPath = "";	// Path where datapack will be saved on disk ----- data
	private string _dataFolderPath = "";    // Data folder within datapack ------------------- datapack/data
	private string _namespacePath = "";     // Namespace within datapack --------------------- datapack/data/namespace
	private string _namespaceFunctions = "";// Where all functions will generate ------------- datapack/data/namespace/functions
	private string _printFunctions = "";    // PrintFunction folder within datapack ---------- datapack/data/print/functions
	private string _datapackStart = "";     // File name of mcode printing start function ---- datapack/data/print/functions/start.mcfunction
	private string _datapackStop = "";      // File name of mcode printing stop function ----- datapack/data/print/functions/stop.mcfunction
	private string _datapackMcFuncTags = "";// File path for --------------------------------- datapack/data/minecraft/tags/functions

	private Dictionary<string, string> _keyVars = new Dictionary<string, string>();
	private McodeData _mcodeData = new McodeData();

	private void Start()
	{
		_pathOfDatapackTemplate = Path.Combine(Application.dataPath, "StreamingAssets", "CopyTemplate");
	}
	
	/// <summary>
	/// Start the generation of a datapack (Will take some time?)
	/// </summary>
	public void GenerateDatapack()
	{
		SetUpVaribleNames();
		_outputRoot = SafeFileManagement.FolderPath("Select where datapack will be saved");
		if(!string.IsNullOrWhiteSpace(_outputRoot))
		{
			CopyTemplateAndRename();
			RenameFiles();
			UpdateCopiedFiles();
			_mcodeData = ConvertToMcodeData(_fileManager.GetParsedGcodeLines());
			_mcodeData.Log();
			WriteMinecraftCodeFiles();
			print("Finished!");
		}
	}

	private void WriteMinecraftCodeFiles()
	{

	}


	private McodeData ConvertToMcodeData(List<List<string>> parsedGcode)
	{
		McodeData newMcodeData = new McodeData();
		Vector3 pos = new Vector3();
		float f = 0;
		bool extrude = false;

		// Add starting value
		newMcodeData.data.Add(new McodeLine());

		foreach (List<string> gcodeLine in parsedGcode)
		{
			if(gcodeLine.Count > 1 && gcodeLine[0].ToUpper() == "G" && gcodeLine[1] == "1")
			{
				for(int i = 2; i < gcodeLine.Count; i++)
				{
					bool nextIsValid = i + 1 <= gcodeLine.Count - 1;
					string upperTerm = gcodeLine[i].ToUpper();

					if (nextIsValid)
					{
						if (upperTerm == "X")
						{
							try
							{
								pos.x = float.Parse(gcodeLine[i + 1]);
							}
							catch (Exception e) { LogFloatParseError(gcodeLine[i + 1], e.Message); }
						}
						else if (upperTerm == "Y")
						{
							try
							{
								pos.z = float.Parse(gcodeLine[i + 1]);
							}
							catch (Exception e) { LogFloatParseError(gcodeLine[i + 1], e.Message); }
						}
						else if (upperTerm == "Z")
						{
							try
							{
								pos.y = float.Parse(gcodeLine[i + 1]);
							}
							catch (Exception e) { LogFloatParseError(gcodeLine[i + 1], e.Message); }
						}
						else if (upperTerm == "F")
						{
							try
							{
								f = Mathf.Clamp(float.Parse(gcodeLine[i + 1]) * _magnitudeScalar, 0, _maxMagnitude);
							}
							catch (Exception e) { LogFloatParseError(gcodeLine[i + 1], e.Message); }
						}
						else if (upperTerm == "E")
						{
							try
							{
								float extrudeAmount = float.Parse(gcodeLine[i + 1]);
								extrude = extrudeAmount > 0;
							}
							catch (Exception e) { LogFloatParseError(gcodeLine[i + 1], e.Message); }
						}
					}
				}
				// If we made it here we know something should have changed so make it a new Mcode line
				newMcodeData.data.Add(new McodeLine(pos, f, extrude));
			}
		}

		newMcodeData.CalculateMotionVectors();
		// Calculate the motion vectors here


		return newMcodeData;
	}

	private void LogFloatParseError(string stringThatTriedToParse, string causedException)
	{
		Debug.Log("Tried to parse: " + stringThatTriedToParse + " which is not a valid float!\n" + causedException);
	}

	/// <summary>
	/// Populates the keyVar dictionary with all terms that should be replaced within the copied minecraft files
	/// </summary>
	public void InitulizeKeyVars()
	{
		string scoreboardVar = c_ScoreboardPrefix + _shortUUID;
		string tag = "Tag" + _datapackUUID;
		_keyVars[c_TemplateNamespace] = _datapackUUID;
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
		if(Directory.Exists(folderPath))
		{
			string[] files = SafeFileManagement.GetFilesPaths(folderPath, _numberOfIORetryAttempts);
			foreach (string file in files)
			{
				print("Updating: " + file);
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
		foreach(string key in _keyVars.Keys)
		{
			fileContents = fileContents.Replace(key, _keyVars[key]);
		}
		return fileContents;
	}
	
	/// <summary>
	/// Rename the start and stop files
	/// </summary>
	private void RenameFiles()
	{
		_printFunctions = Path.Combine(_dataFolderPath, "print", "functions");
		string templateStart = Path.Combine(_printFunctions, c_TemplateNamespace + c_StartFunctionSuffix + c_McFunction);
		_datapackStart = Path.Combine(_printFunctions, _datapackUUID + c_StartFunctionSuffix + c_McFunction);
		if(SafeFileManagement.MoveFile(templateStart, _datapackStart, _numberOfIORetryAttempts))
		{
			string templateStop = Path.Combine(_printFunctions, c_TemplateNamespace + c_StopFunctionSuffix + c_McFunction);
			_datapackStop = Path.Combine(_printFunctions, _datapackUUID + c_StopFunctionSuffix + c_McFunction);
			SafeFileManagement.MoveFile(templateStop, _datapackStop, _numberOfIORetryAttempts);
		}
	}

	/// <summary>
	/// Copy folders and files from datapack tempate then rename folders
	/// </summary>
	private void CopyTemplateAndRename()
	{
		SafeFileManagement.DirectoryCopy(_pathOfDatapackTemplate, _outputRoot, true, _excludeExtensions, _numberOfIORetryAttempts);

		// Rename main datapack folder
		string templateOutput = Path.Combine(_outputRoot, c_TemplateName);
		_datapackRootPath = Path.Combine(_outputRoot, _datapackName);
		if(SafeFileManagement.MoveDirectory(templateOutput, _datapackRootPath, _numberOfIORetryAttempts))
		{
			// Rename namespace folder
			_dataFolderPath = Path.Combine(_datapackRootPath, c_Data);
			string templateNamespace = Path.Combine(_dataFolderPath, c_TemplateNamespace);
			_namespacePath = Path.Combine(_dataFolderPath, _datapackUUID);
			SafeFileManagement.MoveDirectory(templateNamespace, _namespacePath, _numberOfIORetryAttempts);

			_namespaceFunctions = Path.Combine(_namespacePath, c_Functions);
			_datapackMcFuncTags = Path.Combine(_dataFolderPath, c_Minecraft, c_Tags, c_Functions);
		}
	}

	/// <summary>
	/// Initulize all varibles based on the gcode file name and date
	/// </summary>
	private void SetUpVaribleNames()
	{
		_gcodeFilePath = _fileManager.GetGcodeFilePath();
		_gcodeFileName = SafeFileManagement.GetFileName(Path.GetFileName(_gcodeFilePath));
		_dateCreated = SafeFileManagement.GetDateNow();
		_datapackUUID = _gcodeFileName + "_" + _dateCreated;
		_datapackName = c_MainDatapackName + "_" + _datapackUUID;
		_shortUUID = _datapackUUID.FirstLast5();
		_fakePlayerName = c_FakePlayerChar + _datapackUUID.Truncate(-30);
		LogDynamicVars();
	}

	/// <summary>
	/// Prints all vars to unity console
	/// </summary>
	private void LogDynamicVars()
	{
		print("_gcodeFilePath: " + _gcodeFilePath + " \n" +
			"_gcodeFileName: " + _gcodeFileName + "\n" +
			"_dateCreated: " + _dateCreated + "\n" +
			"_datapackUUID: " + _datapackUUID + "\n" +
			"_datapackName: " + _datapackName + "\n" +
			"_shortName: " + _shortUUID + "\n" +
			"_fakePlayerName: " + _fakePlayerName);
	}
}

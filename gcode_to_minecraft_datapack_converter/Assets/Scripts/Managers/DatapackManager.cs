using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Manager responsible for creating all files that make up the Minecraft datapack
/// </summary>
public class DatapackManager
{
	#region Constants
	private const int C_numberOfIORetryAttempts = 5;
	private const int C_LinesPerFunction = 10000; // Muse be lower then 65000ish (command limit within datapack functions)

	// Minecraft blocks
	private const string C_BlockAir = "air";
	private const string C_GuideLinesBlock = "white_concrete";

	// Prefix and Suffix
	private const string C_StartFunctionSuffix = "_start";
	private const string C_StopFunctionSuffix = "_stop";
	private const string C_PauseFunctionSuffix = "_pause";
	private const string C_OptionsFunctionSuffix = "_options";
	private const string C_ScoreboardPrefix = "gp_";


	private const string C_Line = "line";
	private const string C_MainDatapackName = "GcodePrinter";

	// Function names
	private const string C_UpdateCodeLineName = "update_code_line";
	private const string C_ClearPrintBedPrefix = "clear_print_bed";

	// Template file names
	private const string C_UpdateCodeLine = "update_code_line.mcfunction";
	private const string C_ExecuteMcode = "execute_mcode.mcfunction";
	private const string C_ProgressBar = "create_progress_bar.mcfunction";
	
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
	private const string C_PlaceBlock = "PLACEBLOCK";
	private const string C_MaxLineNumber = "MAXLINENUMBER";

	//Template Names
	private const string C_TemplateName = "TemplateDatapack";
	private const string C_TemplateNamespace = "mcode1";

	// Datapack hardcoded names
	private const string C_Data = "data";
	private const string C_Minecraft = "minecraft";
	private const string C_Functions = "functions";
	private const string C_McFunction = ".mcfunction";
	private const string C_Tags = "tags";
	private const string C_FakePlayerChar = "#";
	private const string C_Slash = "/";

	private readonly string[] _excludeExtensions = { ".meta" };
	#endregion Constants

	#region DynamicStrings
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
	#endregion DynamicStrings

	private Dictionary<string, string> _keyVars = new Dictionary<string, string>();

	/// <summary>
	/// An Async function that generates a minecraft datapack folder when given a parsed data type
	/// </summary>
	/// <param name="dataStats">The stats class used when parsing gcode files</param>
	/// <param name="progess">The ProgressAmount class that keeps track of this function's progress 0.0 -> 1.0</param>
	/// <param name="cancellationToken">Token that allows async function to be canceled</param>
	/// <returns>Modified ParsedDataStats type</returns>
	public Task<DataStats> GenerateAsync(DataStats dataStats, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			progess.ReportValue(0.0f, "Generating Datapack Files", "Creating file names");
			_gcodeFileName = MakeSafeString(SafeFileManagement.GetFileName(Path.GetFileName(dataStats.gcodePath)));
			_dateCreated = SafeFileManagement.GetDateNow();
			

			// Use default name if custom name is empty
			if(dataStats.datapackName.IsEmpty())
			{
				_datapackUUID = _gcodeFileName + "_" + _dateCreated;
				_datapackName = C_MainDatapackName + "_" + _datapackUUID;
				dataStats.datapackName = _datapackName;
			}
			else
			{
				_datapackUUID = dataStats.datapackName;
				_datapackName = dataStats.datapackName;
			}
			
			_shortUUID = _datapackUUID.FirstLast5();
			_fakePlayerName = C_FakePlayerChar + _datapackUUID.Truncate(-30);
			_outputRoot = dataStats.datapackPath;

			if (!_outputRoot.IsEmpty())
			{
				progess.ReportValue(0.05f, "Generating Datapack Files", "Copying Template");
				CopyTemplateAndRename(dataStats);
				dataStats.datapackPath = _datapackRootPath;

				progess.ReportValue(0.1f, "Generating Datapack Files", "Renaming Files");
				RenameFiles();

				progess.ReportValue(0.12f, "Generating Datapack Files", "Updating Files");
				UpdateCopiedFiles(dataStats);

				progess.ReportValue(0.15f, "Generating Datapack Files", "Writing files");
				WriteMinecraftCodeFilesAsync(dataStats.totalMcodeLines, dataStats.parsedGcodePath, progess, cancellationToken);

				GeneratePrintBed(dataStats);
			}
			progess.ReportValue(1.0f, "Generating Datapack Files");
			return dataStats;
		});
	}

	#region PrivateMembers
	/// <summary>
	/// Parse given string and return new string that is mcdatapack allowed
	/// </summary>
	/// <param name="name">String to be paresed</param>
	/// <returns></returns>
	private string MakeSafeString(string name)
	{
		name = name.ToLower();
		System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-z0-9_-]");
		return rgx.Replace(name, "");
	}

	/// <summary>
	/// Generate the minecraft code to make the printbed (will be dynamic in the future)
	/// </summary>
	/// <param name="dataStats">The stats class used when parsing gcode files</param>
	private void GeneratePrintBed(DataStats dataStats)
	{
		string commands = "";

		// Clear and place bed
		for (int x = -8; x <= 256; x += 8)
		{
			for (int z = -8; z <= 256; z += 16)
			{
				commands += "fill " + x.ToString() + " 0 " + z.ToString() + " " + (x + 7).ToString() + " 250 " + (z + 15).ToString() + " minecraft:" + C_BlockAir + " replace\n";
				commands += "fill " + x.ToString() + " 0 " + z.ToString() + " " + (x + 7).ToString() + " 0 " + (z + 15).ToString() + " minecraft:" + dataStats.printBedMaterial + " replace\n";
			}
		}
		
		commands += "fill 0 0 0 0 0 256 minecraft:" + C_GuideLinesBlock + " replace\n"; //Front
		commands += "fill 0 0 0 256 0 0 minecraft:" + C_GuideLinesBlock + " replace\n"; //Left
		commands += "fill 256 0 256 256 0 0 minecraft:" + C_GuideLinesBlock + " replace\n"; //Back
		commands += "fill 256 0 256 0 0 256 minecraft:" + C_GuideLinesBlock + " replace\n"; //Right

		for (int x = 0; x <= 256; x += 16)
		{
			for (int z = 0; z <= 256; z += 16)
			{
				commands += "setblock " + x.ToString() + " 0 " + z.ToString() + " minecraft:" + C_GuideLinesBlock + " replace\n";
			}
		}

		string filePath = Path.Combine(_namespaceFunctions, C_ClearPrintBedPrefix + C_McFunction);
		SafeFileManagement.SetFileContents(filePath, commands);
	}

	private void WriteMinecraftCodeFilesAsync(int totalLines, string mcodeCSVFilePath, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		if (File.Exists(mcodeCSVFilePath))
		{
			try
			{
				using (var mcodeCSVData = new StreamReader(mcodeCSVFilePath))
				{
					mcodeCSVData.ReadLine();   // Skip header
					GcodeStorage parsedData = null;

					// Get the template file contense
					string templateUpdateCode = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateUpdateCode));
					string templateExecuteLine = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateExecuteLine));
					string templateFinishedLine = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_TemplateFinishedLine));

					long factor = C_LinesPerFunction;
					long factorPow2 = factor * factor;
					long facotrPow3 = factorPow2 * factor;
					long lineAmount = totalLines;

					string lvl1Folder = "level1code0_" + (facotrPow3 - 1);
					string lvl1Root = Path.Combine(_namespaceFunctions, lvl1Folder);
					Directory.CreateDirectory(lvl1Root);

					string lvl1UpdateCode = "";

					for (long lvl1 = 0; lvl1 < facotrPow3 && lvl1 < lineAmount; lvl1 += factorPow2)
					{
						string lvl2Folder = "level2code" + lvl1 + "_" + (lvl1 + factorPow2 - 1);
						string lvl2Root = Path.Combine(lvl1Root, lvl2Folder);
						Directory.CreateDirectory(lvl2Root);

						string lvl2UpdateCode = "";

						string lvl1CurrentUpdateCode = templateExecuteLine;
						lvl1CurrentUpdateCode = lvl1CurrentUpdateCode.Replace(C_LineNum_1, (lvl1) + ".." + (lvl1 + factorPow2));
						lvl1CurrentUpdateCode = lvl1CurrentUpdateCode.Replace(C_LineNum, DatapackPath(lvl1Folder, lvl2Folder, C_UpdateCodeLineName));
						lvl1UpdateCode += lvl1CurrentUpdateCode;

						for (long lvl2 = lvl1; lvl2 < lvl1 + 1 * factorPow2 && lvl2 < lineAmount; lvl2 += factor)
						{
							string lvl3UpdateCode = "";

							string lvl3Folder = "level3code" + lvl2 + "_" + (lvl2 + factor - 1);
							string lvl3Root = Path.Combine(lvl2Root, lvl3Folder);
							string localFolderRoot = DatapackPath(lvl1Folder, lvl2Folder, lvl3Folder);

							Directory.CreateDirectory(lvl3Root);

							string lvl2CurrentUpdateCode = templateExecuteLine;
							lvl2CurrentUpdateCode = lvl2CurrentUpdateCode.Replace(C_LineNum_1, (lvl2) + ".." + (lvl2 + factor));
							lvl2CurrentUpdateCode = lvl2CurrentUpdateCode.Replace(C_LineNum, DatapackPath(localFolderRoot, C_UpdateCodeLineName));
							lvl2UpdateCode += lvl2CurrentUpdateCode;

							for (long lvl3 = lvl2; lvl3 < lvl2 + 1 * factor && lvl3 < lineAmount; lvl3++)
							{
								string filePath = Path.Combine(lvl3Root, C_Line + lvl3 + C_McFunction);
								string currentUpdateCode = "";
								string readData = "";

								// Send progress update every 250 lines processed
								if (lvl3 % 250 == 0)
								{
									progess.ReportValue(0.15f + ((float)lvl3 / lineAmount) * 0.85f, "Generating Datapack Files", "Writing file " + lvl3 + "/" + lineAmount);
									cancellationToken.ThrowIfCancellationRequested();
								}

								if (!mcodeCSVData.EndOfStream)
								{
									readData = mcodeCSVData.ReadLine();
									parsedData = new GcodeStorage(readData);

									// Make sure to add special ending line when the last line is written
									if (lvl3 != lineAmount - 1)
									{
										currentUpdateCode = templateUpdateCode;
										currentUpdateCode = currentUpdateCode.Replace(C_LineNum, (lvl3).ToString());
										currentUpdateCode = currentUpdateCode.Replace(C_XNum, parsedData.pos.x.ToString("F10"));
										currentUpdateCode = currentUpdateCode.Replace(C_YNum, parsedData.pos.y.ToString("F10"));
										currentUpdateCode = currentUpdateCode.Replace(C_ZNum, parsedData.pos.z.ToString("F10"));
										currentUpdateCode = currentUpdateCode.Replace(C_PlaceBlock, (parsedData.exturedAmount > 0 ? "1" : "0"));
										lvl3UpdateCode += currentUpdateCode;
									}
									else
									{
										currentUpdateCode = templateFinishedLine;
										currentUpdateCode = currentUpdateCode.Replace(C_LineNum, lvl3.ToString() + "..");
										lvl3UpdateCode += currentUpdateCode;
									}
								}

								string currentExecute = templateExecuteLine;
								currentExecute = currentExecute.Replace(C_LineNum_1, (lvl3).ToString());
								currentExecute = currentExecute.Replace(C_LineNum, DatapackPath(localFolderRoot, C_Line + (lvl3)));
							}

							// Save strings to files
							SafeFileManagement.SetFileContents(Path.Combine(lvl3Root, C_UpdateCodeLine), lvl3UpdateCode);
						}

						// Save strings to files
						SafeFileManagement.SetFileContents(Path.Combine(lvl2Root, C_UpdateCodeLine), lvl2UpdateCode);

					}

					progess.ReportValue(1, "Generating Datapack Files", "Cleaning up temp files");

					// Save strings to files
					SafeFileManagement.SetFileContents(Path.Combine(_namespaceFunctions, C_UpdateCodeLine), lvl1UpdateCode);

					// Update bossbar with total number of lines
					string bossBarContense = SafeFileManagement.GetFileContents(Path.Combine(_namespaceFunctions, C_ProgressBar));
					bossBarContense = bossBarContense.Replace(C_MaxLineNumber, (lineAmount + 1).ToString());
					SafeFileManagement.SetFileContents(Path.Combine(_namespaceFunctions, C_ProgressBar), bossBarContense);

					// Clean up datapack folder templates
					SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateUpdateCode));
					SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateExecuteLine));
					SafeFileManagement.DeleteFile(Path.Combine(_namespaceFunctions, C_TemplateFinishedLine));
				}
			}
			catch (OperationCanceledException wasCanceled)
			{
				throw wasCanceled;
			}
			catch (ObjectDisposedException wasAreadyCanceled)
			{
				throw wasAreadyCanceled;
			}
			catch (Exception e)
			{ LogError("The gcode file could not be written to", e); }
		}
	}

	/// <summary>
	/// Populates the keyVar dictionary with all terms that should be replaced within the copied minecraft files
	/// </summary>
	private void InitulizeKeyVars(DataStats dataStats)
	{
		string scoreboardVar = C_ScoreboardPrefix + _shortUUID;
		string tag = "Tag" + _datapackUUID;
		_keyVars[C_TemplateNamespace] = _datapackUUID;
		_keyVars["gp_ArgVar"] = scoreboardVar;
		_keyVars["#fakePlayerVar"] = _fakePlayerName;
		_keyVars["TAGG"] = tag;
		_keyVars["FILLBLOCK"] = dataStats.printMaterial;
		_keyVars["PRINTSPEED"] = dataStats.absoluteScalar.ToString();
		_keyVars["PRINTTOLERANCE"] = (dataStats.absoluteScalar + 0.1f).ToString();
	}

	private void UpdateCopiedFiles(DataStats dataStats)
	{
		InitulizeKeyVars(dataStats);
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
			string[] files = SafeFileManagement.GetFilesPaths(folderPath, C_numberOfIORetryAttempts);
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
	/// Rename the start, pause, stop, and options files
	/// </summary>
	private void RenameFiles()
	{
		_printFunctions = Path.Combine(_dataFolderPath, "print", "functions");
		string templateStart = Path.Combine(_printFunctions, C_TemplateNamespace + C_StartFunctionSuffix + C_McFunction);
		_datapackStart = Path.Combine(_printFunctions, _datapackUUID + C_StartFunctionSuffix + C_McFunction);
		if (SafeFileManagement.MoveFile(templateStart, _datapackStart, C_numberOfIORetryAttempts))
		{
			string templateStop = Path.Combine(_printFunctions, C_TemplateNamespace + C_StopFunctionSuffix + C_McFunction);
			_datapackStop = Path.Combine(_printFunctions, _datapackUUID + C_StopFunctionSuffix + C_McFunction);
			if (SafeFileManagement.MoveFile(templateStop, _datapackStop, C_numberOfIORetryAttempts))
			{
				string templatePause = Path.Combine(_printFunctions, C_TemplateNamespace + C_PauseFunctionSuffix + C_McFunction);
				_datapackPause = Path.Combine(_printFunctions, _datapackUUID + C_PauseFunctionSuffix + C_McFunction);
				if (SafeFileManagement.MoveFile(templatePause, _datapackPause, C_numberOfIORetryAttempts))
				{
					string templateOptions = Path.Combine(_printFunctions, C_TemplateNamespace + C_OptionsFunctionSuffix + C_McFunction);
					string _datapackOptions = Path.Combine(_printFunctions, _datapackUUID + C_OptionsFunctionSuffix + C_McFunction);
					SafeFileManagement.MoveFile(templateOptions, _datapackOptions, C_numberOfIORetryAttempts);
				}
			}
		}
	}

	/// <summary>
	/// Copy folders and files from datapack tempate then rename folders
	/// </summary>
	private void CopyTemplateAndRename(DataStats dataStats)
	{
		string pathOfDatapackTemplate = Path.Combine(dataStats.unityDataPath, "StreamingAssets", "CopyTemplate");
		SafeFileManagement.DirectoryCopy(pathOfDatapackTemplate, _outputRoot, true, _excludeExtensions, C_numberOfIORetryAttempts);

		// Rename main datapack folder
		string templateOutput = Path.Combine(_outputRoot, C_TemplateName);
		_datapackRootPath = Path.Combine(_outputRoot, _datapackName);
		if (SafeFileManagement.MoveDirectory(templateOutput, _datapackRootPath, C_numberOfIORetryAttempts))
		{
			// Rename namespace folder
			_dataFolderPath = Path.Combine(_datapackRootPath, C_Data);
			string templateNamespace = Path.Combine(_dataFolderPath, C_TemplateNamespace);
			_namespacePath = Path.Combine(_dataFolderPath, _datapackUUID);
			SafeFileManagement.MoveDirectory(templateNamespace, _namespacePath, C_numberOfIORetryAttempts);

			_namespaceFunctions = Path.Combine(_namespacePath, C_Functions);
			_datapackMcFuncTags = Path.Combine(_dataFolderPath, C_Minecraft, C_Tags, C_Functions);
		}
	}


	private string DatapackPath(params string[] values)
	{
		if (values.Length < 2)
			return values[0];

		string newString = "";
		foreach (string part in values)
		{
			newString += part + C_Slash;
		}

		return newString.TrimEnd(newString[newString.Length - 1]);

	}

	private void LogError(string text, Exception error)
	{
		Debug.LogError("Error\n" + text + "\n" + error.Message);
	}
	#endregion PrivateMembers
}

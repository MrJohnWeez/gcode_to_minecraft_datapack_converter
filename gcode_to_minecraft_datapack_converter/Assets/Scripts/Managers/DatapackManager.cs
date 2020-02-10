using System.Collections;
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
	// String Constants
	private const string c_StartFunctionSuffix = "_start";
	private const string c_StopFunctionSuffix = "_stop";
	private const string c_PrintPrefix = "print";
	private const string c_ScoreboardPrefix = "gp_";
	private const string c_Line = "line";
	private const string c_MainDatapackName = "GcodePrinter";

	private const string c_ExecuteLine = "execute_mcode_line";
	private const string c_NextLine = "next_mcode_line";
	private const string c_Reload = "reload";
	private const string c_Stop = "stop";

	private const string c_CenterpointTag = "centerpoint";
	private const string c_PrintheadTag = "printhead";
	private const string c_CurrentNodeTag = "currentnode";

	// Datapack hardcoded names
	private const string c_MetaFileName = "pack.mcmeta";
	private const string c_Data = "data";
	private const string c_Minecraft = "minecraft";
	private const string c_Functions = "functions";
	private const string c_McFunction = ".mcfunction";
	private const string c_Joson = ".json";
	private const string c_Init = "init";
	private const string c_Tick = "tick";
	private const string c_Tags = "tags";
	private const string c_Load = "load";
	private const string c_FakePlayerChar = "#";

	// Datapack format version
	private const int c_DatapackFormat = 5;


	[SerializeField] private FileManager _fileManager = null;


	private string _gcodeFilePath = "";     // Path of gcode file on disk
	private string _gcodeFileName = "";     // Main name of gcode file (without .gcode)

	private string _dateCreated = "";       // Date datapack was created (almost UUID)
	private string _datapackUUID = "";      // Simulates a UUID because the use case of this program is low
	private string _datapackName = "";      // Name of the datapack

	private string _shortName = "";         // Name used to make scoreboard values unique (10 chars or less)
	private string _fakePlayerName = "";    // The fake player name that datapack will use (39 chars or less)

	/// <summary>
	/// Start the generation of a datapack (Will take some time?)
	/// </summary>
	public void GenerateDatapack()
	{
		SetUpVaribleNames();
		//CreateDatapack(_fileManager.GetParsedGcodeLines());
	}

	/// <summary>
	/// Initulize all varibles based on the gcode file name and date
	/// </summary>
	private void SetUpVaribleNames()
	{
		_gcodeFilePath = _fileManager.GetGcodeFilePath();
		_gcodeFileName = FileManager.GetFileName(Path.GetFileName(_gcodeFilePath));
		_dateCreated = FileManager.GetDateNow();
		_datapackUUID = _gcodeFileName + "_" + _dateCreated;
		_datapackName = c_MainDatapackName + "_" + _datapackUUID;
		_shortName = _datapackUUID.FirstLast5();
		_fakePlayerName = c_FakePlayerChar + _datapackUUID.Truncate(-30);
		LogDynamicVars();
	}
	
	/// <summary>
	/// Prints all vars to unity concsel
	/// </summary>
	private void LogDynamicVars()
	{
		print("_gcodeFilePath: " + _gcodeFilePath + " \n" +
			"_gcodeFileName: " + _gcodeFileName + "\n" +
			"_dateCreated: " + _dateCreated + "\n" +
			"_datapackUUID: " + _datapackUUID + "\n" +
			"_datapackName: " + _datapackName + "\n" +
			"_shortName: " + _shortName + "\n" +
			"_fakePlayerName: " + _fakePlayerName);
	}

	/// <summary>
	/// Generates whole complete datapack where user selected
	/// </summary>
	private void CreateDatapack(List<List<string>> gcodeData)
	{
		string[] selectedRootFolder = StandaloneFileBrowser.OpenFolderPanel("Save Generated Datapack", "", false);

		if(selectedRootFolder.Length > 0)
		{
			CreateAllFolders(selectedRootFolder[0]);
		}
	}

	private void CreateAllFolders(string rootPath)
	{

		Directory.CreateDirectory(rootPath);
	}
}

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

	// Tags
	private const string c_CenterpointTag = "centerpoint";
	private const string c_PrintheadTag = "printhead";
	private const string c_CurrentNodeTag = "currentnode";
	private const string c_PrintGroup = "PrintGroup";

	//Template Names
	private const string c_TemplateName = "TemplateDatapack";
	private const string c_TemplateNamespace = "mcode1";

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

	private string slash = Path.DirectorySeparatorChar.ToString();
	private string _templateCopy = "";

	private string[] _excludeExtensions = new string[1] { ".meta" };
	private string _gcodeFilePath = "";     // Path of gcode file on disk
	private string _gcodeFileName = "";     // Main name of gcode file (without .gcode)
	private string _dateCreated = "";       // Date datapack was created (almost UUID)
	private string _datapackUUID = "";      // Simulates a UUID because the use case of this program is low
	private string _datapackName = "";      // Name of the datapack
	private string _shortName = "";         // Name used to make scoreboard values unique (10 chars or less)
	private string _fakePlayerName = "";    // The fake player name that datapack will use (39 chars or less)
	private string _outputRoot = "";        // The root folder where the datapack will be saved to
	private string _datapackRoot = "";		// The root of the datapack on drive
	private string _mcodeFunctions = "";    // The path where all main functions are stored

	private string _datapackRootPath = "";
	private string _dataFolderPath = "";
	private string _namespacePath = "";

	private void Start()
	{
		_templateCopy = Application.dataPath + "/StreamingAssets/CopyTemplate";
	}

	/// <summary>
	/// Start the generation of a datapack (Will take some time?)
	/// </summary>
	public void GenerateDatapack()
	{
		SetUpVaribleNames();
		_outputRoot = FileManager.FolderPath("Select where datapack will be saved");
		if(!string.IsNullOrWhiteSpace(_outputRoot))
		{
			CopyTemplateAndRename();
			RenameFiles();
			//GenerateMcFunctions(_fileManager.GetParsedGcodeLines());
		}
	}









	// Need to make a function that takes in a filepath and replaces all varibles with correct words











	private void RenameFiles()
	{
		string printFunctions = _dataFolderPath + slash + "print" + slash + "functions";
		string templateStart = printFunctions + slash + c_TemplateNamespace + c_StartFunctionSuffix + c_McFunction;
		string datapackStart = printFunctions + slash + _datapackUUID + c_StartFunctionSuffix + c_McFunction;
		File.Move(templateStart, datapackStart);

		string templateStop = printFunctions + slash + c_TemplateNamespace + c_StopFunctionSuffix + c_McFunction;
		string datapackStop = printFunctions + slash + _datapackUUID + c_StopFunctionSuffix + c_McFunction;
		File.Move(templateStop, datapackStop);
	}

	/// <summary>
	/// Copy folders and files from datapack tempate then rename folders
	/// </summary>
	private void CopyTemplateAndRename()
	{
		FileManager.DirectoryCopy(_templateCopy, _outputRoot, true, _excludeExtensions);

		// Rename main datapack folder
		string templateOutput = _outputRoot + slash + c_TemplateName;
		_datapackRootPath = _outputRoot + slash + _datapackName;
		Directory.Move(templateOutput, _datapackRootPath);

		// Rename namespace folder
		_dataFolderPath = _datapackRootPath + slash + c_Data;
		string templateNamespace = _dataFolderPath + slash + c_TemplateNamespace;
		_namespacePath = _dataFolderPath + slash + _datapackUUID;
		Directory.Move(templateNamespace, _namespacePath);
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
	/// Prints all vars to unity console
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
}

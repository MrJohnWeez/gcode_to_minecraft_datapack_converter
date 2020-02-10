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

	private string _gcodeFilePath = "";     // Path of gcode file on disk
	private string _gcodeFileName = "";     // Main name of gcode file (without .gcode)
	private string _dateCreated = "";       // Date datapack was created (almost UUID)
	private string _datapackUUID = "";      // Simulates a UUID because the use case of this program is low
	private string _datapackName = "";      // Name of the datapack
	private string _shortName = "";         // Name used to make scoreboard values unique (10 chars or less)
	private string _fakePlayerName = "";    // The fake player name that datapack will use (39 chars or less)
	private string _outputRoot = "";        // The root folder where the datapack will be saved to
	private string _datapackRoot = "";		// The root of the datapack on drive
	private string _mcodeFunctions = "";	// The path where all main functions are stored

	/// <summary>
	/// Start the generation of a datapack (Will take some time?)
	/// </summary>
	public void GenerateDatapack()
	{
		SetUpVaribleNames();
		_outputRoot = FileManager.FolderPath("Select where datapack will be saved");
		print(_outputRoot);
		if(!string.IsNullOrWhiteSpace(_outputRoot))
		{
			CreateDatapackFoldersAndBaseFiles();
			GenerateMcFunctions(_fileManager.GetParsedGcodeLines());
		}
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
	/// Create all folders and a few base files
	/// </summary>
	private void CreateDatapackFoldersAndBaseFiles()
	{
		_datapackRoot = _outputRoot + slash + _datapackName;
		Directory.CreateDirectory(_datapackRoot);

		string dataFolder = _datapackRoot + slash + c_Data;
		Directory.CreateDirectory(dataFolder);
		CreateMetaFile(_datapackRoot + slash + c_MetaFileName);

		string minecraftTagsFolder = dataFolder + slash + c_Minecraft + slash + c_Tags + slash + c_Functions;
		Directory.CreateDirectory(minecraftTagsFolder);
		CreateLoadTickTags(minecraftTagsFolder);

		string printFunctions = dataFolder + slash + c_PrintPrefix + slash + c_Functions;
		Directory.CreateDirectory(printFunctions);
		CreatePrintingStates(printFunctions);

		_mcodeFunctions = dataFolder + slash + _datapackUUID + slash + c_Functions;
		Directory.CreateDirectory(_mcodeFunctions);
	}

	/// <summary>
	/// Create mcmetafile
	/// </summary>
	/// <param name="filePath"></param>
	private void CreateMetaFile(string filePath)
	{
		using (StreamWriter sw = File.CreateText(filePath))
		{
			string jsonLine = "{\"pack\": {\"pack_format\": " + 
								c_DatapackFormat.ToString() +
								", \"description\": \"Print out a gcode file within minecraft!\"}}";
			sw.WriteLine(jsonLine);
		}
	}

	/// <summary>
	/// Create load and tick minecraft files
	/// </summary>
	/// <param name="filePath">Filepath to create these in</param>
	private void CreateLoadTickTags(string filePath)
	{
		string load = filePath + slash + c_Load + c_Joson;
		string tick = filePath + slash + c_Tick + c_Joson;

		using (StreamWriter sw = File.CreateText(load))
		{
			string jsonLine = "{ \"values\": [\"" +
								_datapackUUID +
								":reload\"]}";
			 sw.WriteLine(jsonLine);
		}

		using (StreamWriter sw = File.CreateText(tick))
		{
			string jsonLine = "{ \"values\": [\"" +
								_datapackUUID +
								":tick\"]}";
			sw.WriteLine(jsonLine);
		}
	}

	/// <summary>
	/// Create the printing state functions
	/// </summary>
	/// <param name="filePath">Filepath to create these in</param>
	private void CreatePrintingStates(string filePath)
	{
		string start = filePath + slash + _datapackUUID + c_StartFunctionSuffix + c_McFunction;
		string stop = filePath + slash + _datapackUUID + c_StopFunctionSuffix + c_McFunction;

		using (StreamWriter sw = File.CreateText(start))
		{
			string commands = "say printing " + _datapackUUID + "...\n" +
								"function " + _datapackUUID + ":" + c_Init;
			sw.WriteLine(commands);
		}

		using (StreamWriter sw = File.CreateText(stop))
		{
			string commands = "say printing " + _datapackUUID + "...\n" +
								"function " + _datapackUUID + ":" + c_Stop;
			sw.WriteLine(commands);
		}
	}

	private void GenerateMcFunctions(List<List<string>> data)
	{
		string gp_mcodeInited = c_ScoreboardPrefix + _datapackUUID + "Inited";
		string gp_mcodeLineNum = c_ScoreboardPrefix + _datapackUUID + "LineNumber";
		string printGroupTag = _datapackUUID + c_PrintGroup;
		string centerpointTag = _datapackUUID + c_CenterpointTag;

		using (StreamWriter sw = File.CreateText(_mcodeFunctions + slash + c_Init + c_McFunction))
		{
			string commands = CreateScoreboardValue(gp_mcodeInited, _fakePlayerName, 1) + "\n" +
								CreateScoreboardValue(gp_mcodeLineNum, _fakePlayerName, 1) + "\n" +
								"scoreboard objectives setdisplay sidebar " + gp_mcodeLineNum + "\n" +
								"kill @e[type=minecraft:armor_stand,tag=" + printGroupTag + "]\n" +
								"execute at @p run summon leash_knot ~ ~ ~ {Tags:[" + centerpointTag + "]}\n" +
								"execute at @e[type=leash_knot,limit=1,tag=CenterPoint] run summon armor_stand ~ ~0.5 ~ {DisabledSlots:2039583, Small:1b, Tags:[";
			sw.WriteLine(commands);
		}
	}

	/// <summary>
	/// Creates a scoreboard value and optionaly sets a player's score to it
	/// </summary>
	/// <param name="scoreName">The name of the scoreboard objective</param>
	/// <param name="playerName">The name of the player to set the score to</param>
	/// <param name="startValue">The start value of the score</param>
	/// <returns></returns>
	private string CreateScoreboardValue(string scoreName, string playerName = "", int startValue = 0)
	{
		string returnThis = "scoreboard objectives add " + scoreName + " dummy\n";
		if(!string.IsNullOrWhiteSpace(playerName))
		{
			returnThis += "\nscoreboard players set " + playerName + " " + scoreName + " " + startValue.ToString();
		}

		return returnThis;
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

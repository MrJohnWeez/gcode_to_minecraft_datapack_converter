using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;
using System.IO;

// Pipeline example:
//	Gcode -> Parsed padded CSV -> mcode CSV -> Datapack
//		 Gcode:
//				;This is a gcode comment
//				G1 X5 Y5 Z0 E-0.2 F900; wipe and retract
//				G1 X5 Y200 E0.14 ; perimeter
//				G1 X5 Y200 Z5 E-0.14 ; perimeter
//		 Parsed padded CSV:  Xcord,Ycord,Zcord,ShouldExtrude,MoveSpeed
//				5,0,5,-0.2,900
//				5,0,200,0.14,900
//				5,5,200,-0.14,900
//		 mcode CSV:   Xcord,Ycord,Zcord,XMotion,YMotion,ZMotion,ShouldExtrude
//				0,0,0,0,0,0,0
//				5,0,5,0.7071068,0,0.7071068,0
//				5,0,200,0,0,1,1
//				5,5,200,0,1,0,0


/// <summary>
/// Handles gcode -> mcode -> datapack pipeline
/// </summary>
public class FileManager : MonoBehaviour
{
	private const string C_Instructions = "Select a 3D printer Gcode file.";
	private readonly ExtensionFilter[] extensions = { new ExtensionFilter("RepRap toolchain Gcode File", "gcode") };

	[SerializeField] private TMP_Text _filePathDisplay = null;
	
	private ParsedDataStats _dataStats = new ParsedDataStats();
	private DatapackManager _datapackManager;

	/// <summary>
	/// Lets the user select a gcode file and parses the file into a mcode (minecraft code) csv file
	/// </summary>
	public void GcodeSelectAndParse()
	{
		string[] gCodePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
		_dataStats.gcodePath = gCodePaths.Length > 0 ? gCodePaths[0] : "";

		_filePathDisplay.text = _dataStats.gcodePath;

		if (!string.IsNullOrEmpty(_dataStats.gcodePath))
		{
			Debug.Log("Loading and converting gcode");
			if (GcodeManager.GcodeToParsedPaddedCSV(ref _dataStats))
			{
				if (GcodeManager.ParsedPaddedCSVToMcodeCSV(ref _dataStats))
				{
					Debug.Log("Finished");
				}
				File.Delete(_dataStats.parsedGcodePath);
			}
		}
	}

	/// <summary>
	/// Takes the parsed gcode csv file and outputs it
	/// </summary>
	public void CreateDatapack()
	{



		// Possibly use a enumerator 
		// Possibly use callbacks to determine what state the datapack generation is in










		Debug.Log("Creating Datapack");
		_datapackManager = new DatapackManager(ref _dataStats);
		File.Delete(_dataStats.mcodePath);

		Debug.Log("Calculating Stats");
		DatapackStats datapackStats = new DatapackStats(_dataStats.datapackPath);
		//Debug.Log("linesOfCode: " + datapackStats.linesOfCode);
		//Debug.Log("numOfFunctions: " + datapackStats.numOfFunctions);
		//Debug.Log("numOfFiles: " + datapackStats.numOfFiles);
		//Debug.Log("numOfDirectories: " + datapackStats.numOfDirectories);

		Debug.Log("Datapack generated!");
	}
}
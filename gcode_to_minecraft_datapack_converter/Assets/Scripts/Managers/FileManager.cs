using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;
using System.IO;


/// <summary>
/// Handles gcode -> mcode -> datapack pipeline
/// </summary>
public class FileManager : MonoBehaviour
{
	private const string C_Instructions = "Select a 3D printer Gcode file.";
	
	[SerializeField] private TMP_Text _filePathDisplay = null;
	
	private string _gcodePath = "";
	private ParsedDataStats _dataStats = new ParsedDataStats();
	private DatapackManager _datapackManager;

	/// <summary>
	/// Lets the user select a gcode file and parses the file into a mcode (minecraft code) csv file
	/// </summary>
	public void GetGcodePath()
    {
		var extensions = new[] { new ExtensionFilter("RepRap toolchain Gcode File", "gcode") };
		string[] gCodePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
		_gcodePath = gCodePaths.Length > 0 ? gCodePaths[0] : "";
		_filePathDisplay.text = _gcodePath;

		if(!string.IsNullOrEmpty(_gcodePath))
		{
			Debug.Log("Loading and converting gcode");
			_dataStats = new ParsedDataStats(_gcodePath);
			string outputPath = GcodeManager.GcodeToParsedPaddedCSV(ref _dataStats);
			_dataStats.mcodePath = GcodeManager.ParsedPaddedCSVToMcodeCSV(outputPath, _dataStats);
			File.Delete(outputPath);
			Debug.Log("Finished");
		}
	}

	/// <summary>
	/// Takes the parsed gcode csv file and outputs it
	/// </summary>
	public void CreateDatapack()
	{
		Debug.Log("Creating Datapack");
		_datapackManager = new DatapackManager(_dataStats);
		//File.Delete(_dataStats.mcodePath);
		Debug.Log("Datapack generated!");
	}
}



// Pipeline example:
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

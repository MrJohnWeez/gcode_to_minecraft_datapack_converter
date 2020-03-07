using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;

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
	
	private void Update()
	{
		//if(Time.frameCount % 10 == 0)
		Debug.Log("Frame");
	}

	/// <summary>
	/// Lets the user select a gcode file and parses the file into a mcode (minecraft code) csv file
	/// </summary>
	public async void GcodeSelectAndParseAsync()
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

		await CreateADatapack();
	}

	/// <summary>
	/// Takes the parsed gcode csv file and outputs it
	/// </summary>
	public void CreateDatapack()
	{


		Debug.Log("Creating Datapack");
		_datapackManager = new DatapackManager();
		_datapackManager.Generate(ref _dataStats);
		File.Delete(_dataStats.mcodePath);

		Debug.Log("Calculating Stats");
		DatapackStats datapackStats = new DatapackStats(_dataStats.datapackPath);
		Debug.Log("Datapack generated!");


		// Convert everything to async calls
		// https://stackoverflow.com/questions/36933869/how-to-make-a-custom-async-progress-method
		// Add a filesize constraint! 2,500kb or something
		// Add a option with a warning if the user wants to go over this
	}

	async Task CreateADatapack()
	{
		Debug.Log("Inside async task");
		long nthPrime = 0;
		nthPrime = await FindPrimeNumber(1000); //set higher value for more time
		Debug.Log("Finished async task");
	}

	public Task<long> FindPrimeNumber(int n)
	{
		return Task.Run(() =>
		{
			int count = 0;
			long a = 2;
			while (count < n)
			{
				long b = 2;
				int prime = 1;// to check if found a prime
				while (b * b <= a)
				{
					if (a % b == 0)
					{
						prime = 0;
						break;
					}
					b++;
				}
				if (prime > 0)
				{
					count++;
				}
				a++;
			}
			return (--a);
		});
	}
}
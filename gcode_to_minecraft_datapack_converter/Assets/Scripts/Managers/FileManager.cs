using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;


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
	private readonly ExtensionFilter[] extensions = { new ExtensionFilter("RepRap toolchain Gcode File", "gcode") };

	[SerializeField] private TMP_Text _gcodeFilePathText = null;
	[SerializeField] private TMP_Text _datapackOutputPathText = null;
	[SerializeField] private Slider _progressBar = null;

	private string _gcodeFilePath = "";
	private string _datapackOutputPath = "";
	public float total = 0;
	public float progressBarValue = 0;
	CancellationTokenSource sourceCancel;

	private void Update()
	{
		_progressBar.value = progressBarValue;

		if(Time.frameCount % 60 == 0)
			Debug.Log("Test: " + Time.frameCount);
	}

	// Add a filesize constraint! 2,500kb or something
	// Add a option with a warning if the user wants to go over this
	public void SelectGcodeFile()
	{
		string[] gCodePaths = StandaloneFileBrowser.OpenFilePanel("Select Gcode file", "", extensions, false);
		_gcodeFilePath = gCodePaths.Length > 0 ? gCodePaths[0] : "";
		_gcodeFilePathText.text = _gcodeFilePath;
	}

	public void SelectDatapackOutputPath()
	{
		_datapackOutputPath = SafeFileManagement.FolderPath("Select where datapack will be saved");
		_datapackOutputPathText.text = _datapackOutputPath;
	}

	// Make progress bar work with async calls
	// Split progress bar into 2 bars
	// make array[] of floats and use those all together for the progress bars
	public async void ConvertAndCreateDatapackAsync()
	{
		if (File.Exists(_gcodeFilePath) && Directory.Exists(_datapackOutputPath))
		{
			progressBarValue = 0;
			ParsedDataStats dataStats = new ParsedDataStats(_gcodeFilePath, _datapackOutputPath);

			sourceCancel = new CancellationTokenSource();
			ProgressAmount<float>[] progresses = new ProgressAmount<float>[4]
			{
				new ProgressAmount<float>(0),
				new ProgressAmount<float>(1),
				new ProgressAmount<float>(2),
				new ProgressAmount<float>(3)
			};

			foreach(ProgressAmount<float> pa in progresses)
				pa.ValueChangedEvent += UpdateProgressBarValue;

			print("Parsing Gcode");
			dataStats = await GcodeManager.GcodeToParsedPaddedCSV(dataStats, progresses[0], sourceCancel.Token);

			print("Converting to Mcode");
			dataStats = await GcodeManager.ParsedPaddedCSVToMcodeCSV(dataStats, progresses[1], sourceCancel.Token);
			
			print("Creating Datapack");
			DatapackManager datapackManager = new DatapackManager();
			dataStats = await datapackManager.Generate(dataStats, progresses[2], sourceCancel.Token);

			print("Calculating Stats");
			DatapackStats datapackStats = new DatapackStats();
			await datapackStats.Calculate(dataStats.datapackPath, progresses[3], sourceCancel.Token);

			File.Delete(dataStats.parsedGcodePath);
			File.Delete(dataStats.mcodePath);

			sourceCancel.Dispose();
			foreach (ProgressAmount<float> pa in progresses)
				pa.ValueChangedEvent -= UpdateProgressBarValue;
		}
		print("Done");
	}

	public void UpdateProgressBarValue(ProgressAmount<float> updateAmount)
	{
		// Need to make sure we update the correct data value
		progressBarValue = updateAmount.Data;
	}
}
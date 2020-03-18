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
//	Gcode -> Parsed padded CSV -> Datapack
//		 Gcode:
//				;This is a gcode comment
//				G1 X5 Y5 Z0 E-0.2 F900; wipe and retract
//				G1 X5 Y200 E0.14 ; perimeter
//				G1 X5 Y200 Z5 E-0.14 ; perimeter
//		 Parsed padded CSV:  Xcord,Ycord,Zcord,ShouldExtrude,MoveSpeed
//				5,0,5,-0.2,900
//				5,0,200,0.14,900
//				5,5,200,-0.14,900





// Upgrade Unity program
// -Migrate progress bars to new canvas
// -Estimated time remaining
// -Make folder a zip at the end
// -Min time to print using datapack
// -Smart speed, make 

// Upgrade Datapack
// -Make platform better
// -Animate print head?



/// <summary>
/// Handles gcode -> mcode -> datapack pipeline
/// </summary>
public class FileManager : MonoBehaviour
{
	private readonly ExtensionFilter[] extensions = { new ExtensionFilter("RepRap toolchain Gcode File", "gcode") };

	[SerializeField] private TMP_Text _gcodeFilePathText = null;
	[SerializeField] private TMP_Text _datapackOutputPathText = null;
	[SerializeField] private Slider _mainProgressBar = null;
	[SerializeField] private Slider _subProgressBar = null;
	[SerializeField] private Slider _absoluteScalarSlider = null;
	[SerializeField] private Toggle _computeDatapackStats = null;

	private string _gcodeFilePath = "";
	private string _datapackOutputPath = "";
	CancellationTokenSource sourceCancel;
	private int _currentProgress = 0;

	ProgressAmount<float>[] progresses = new ProgressAmount<float>[4]
	{
		new ProgressAmount<float>(0),
		new ProgressAmount<float>(1),
		new ProgressAmount<float>(2),
		new ProgressAmount<float>(3)
	};

	private void Update()
	{
		float total = 0;
		foreach (ProgressAmount<float> pa in progresses)
			total += pa.Data;

		_mainProgressBar.value = total;
		_subProgressBar.value = progresses[_currentProgress].Data;
	}
	
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

	public async void ConvertAndCreateDatapackAsync()
	{
		if (File.Exists(_gcodeFilePath) && Directory.Exists(_datapackOutputPath))
		{
			ParsedDataStats dataStats = new ParsedDataStats(_gcodeFilePath, _datapackOutputPath);

			dataStats.absoluteScalar = _absoluteScalarSlider.value;

			sourceCancel = new CancellationTokenSource();
			_currentProgress = 0;
			
			_mainProgressBar.maxValue = _computeDatapackStats.isOn ? progresses.Length : progresses.Length - 1;
			foreach (ProgressAmount<float> pa in progresses)
			{
				pa.ReportValue(0, "");	// Make sure value is reset
				pa.ValueChangedEvent += UpdateProgressBarValue;
			}

			try
			{
				dataStats = await GcodeManager.GcodeToParsedPaddedCSV(dataStats, progresses[0], sourceCancel.Token);
				
				DatapackManager datapackManager = new DatapackManager();
				dataStats = await datapackManager.Generate(dataStats, progresses[1], sourceCancel.Token);
				
				if(_computeDatapackStats.isOn)
				{
					DatapackStats datapackStats = new DatapackStats();
					await datapackStats.Calculate(dataStats.datapackPath, progresses[2], sourceCancel.Token);

					float estimatedPrintTime = await TimeEstimator.CalculateEstimatedTime(dataStats.parsedGcodePath, dataStats.absoluteScalar, progresses[3], sourceCancel.Token);
				}

				File.Delete(dataStats.parsedGcodePath);

				sourceCancel.Dispose();
			}
			catch(OperationCanceledException wasCanceled)
			{
				if(File.Exists(dataStats.parsedGcodePath))
					File.Delete(dataStats.parsedGcodePath);

				_mainProgressBar.value = 0;
				_subProgressBar.value = 0;

				if(sourceCancel != null)
					sourceCancel.Dispose();

				Debug.Log("Datapack generation was canceled!");
			}
			catch (ObjectDisposedException wasAreadyCanceled)
			{
				Debug.LogError("Objects was already disposed!\n" + wasAreadyCanceled);
			}
			
			foreach (ProgressAmount<float> pa in progresses)
				pa.ValueChangedEvent -= UpdateProgressBarValue;
		}
		print("Done");
	}

	public void UpdateProgressBarValue(ProgressAmount<float> updateAmount)
	{
		if(_currentProgress != updateAmount.Id)
		{
			_currentProgress = updateAmount.Id;
		}
	}

	public void CancelDatapackGeneration()
	{
		if(sourceCancel != null && !sourceCancel.IsCancellationRequested)
		{
			sourceCancel.Cancel();
			Debug.Log("CANCELED!!!!");
		}
	}
}
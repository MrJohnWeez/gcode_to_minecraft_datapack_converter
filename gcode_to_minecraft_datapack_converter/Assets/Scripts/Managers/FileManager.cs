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
// -Ablity to use custom datapack name
// -Ability to use custom blocks



/// <summary>
/// Handles gcode -> mcode -> datapack pipeline
/// </summary>
public class FileManager : MonoBehaviour
{
	private readonly ExtensionFilter[] extensions = { new ExtensionFilter("RepRap toolchain Gcode File", "gcode") };
	
	[Header("Texts")]
	[SerializeField] private TMP_Text _gcodeFilePathText = null;
	[SerializeField] private TMP_Text _datapackOutputPathText = null;

	[Header("Interactives")]
	[SerializeField] private Slider _absoluteScalarSlider = null;
	[SerializeField] private Toggle _computeDatapackStats = null;
	[SerializeField] private Button GenerateDatapackButton = null;

	[Header("Objects")]
	public ProgressTracker progressTrackerPrefab = null;
	[SerializeField] private StatsManager _statsManager = null;
	[SerializeField] private DropdownManager _dropDownManager = null;
	[SerializeField] private ValidateInput _validateInput = null;


	private string _gcodeFilePath = "";
	private string _datapackOutputPath = "";
	private CancellationTokenSource sourceCancel;
	private int _currentProgress = 0;
	private ProgressTracker _progressTracker = null;
	

	ProgressAmount<float>[] progresses = new ProgressAmount<float>[5]
	{
		new ProgressAmount<float>(0),
		new ProgressAmount<float>(1),
		new ProgressAmount<float>(2),
		new ProgressAmount<float>(3),
		new ProgressAmount<float>(4)
	};

	#region UnityCallbacks
	private void Start()
	{
		AreSelectedPathsValid();
	}

	private void Update()
	{
		float total = 0;
		foreach (ProgressAmount<float> pa in progresses)
			total += pa.Data;

		if(_progressTracker)
		{
			_progressTracker.SetMainValue(total, progresses[_currentProgress].Message);
			_progressTracker.SetSubValue(progresses[_currentProgress].Data, progresses[_currentProgress].SubMessage);
		}
	}
	#endregion UnityCallbacks

	/// <summary>
	/// Lets the user select a gcode file to be parsed and converted
	/// </summary>
	public void SelectGcodeFile()
	{
		string[] gCodePaths = StandaloneFileBrowser.OpenFilePanel("Select Gcode file", "", extensions, false);
		string newPath = gCodePaths.Length > 0 ? gCodePaths[0] : "";
		if(!newPath.IsEmpty())
		{
			_gcodeFilePath = newPath;
			_gcodeFilePathText.text = _gcodeFilePath;
			AreSelectedPathsValid();
		}
	}

	/// <summary>
	/// Lets the user select the datapack output folder
	/// </summary>
	public void SelectDatapackOutputPath()
	{
		string newPath = SafeFileManagement.FolderPath("Select where datapack will be saved");
		if(!newPath.IsEmpty())
		{
			_datapackOutputPath = newPath;
			_datapackOutputPathText.text = _datapackOutputPath;
			AreSelectedPathsValid();
		}
	}

	/// <summary>
	/// Convert and generate a datapack if the input and output paths are valid
	/// </summary>
	public async void ConvertAndCreateDatapackAsync()
	{
		if (AreSelectedPathsValid())
		{
			_progressTracker = Instantiate(progressTrackerPrefab);
			_progressTracker.name = "Datapack Generation Progress";
			_progressTracker.CanceledEvent += CancelDatapackGeneration;
			int mainMax = _computeDatapackStats.isOn ? progresses.Length : progresses.Length - 2;
			_progressTracker.Configure("Converting Gcode to Minecraft Datapack...", mainMax, 1);

			_statsManager.Clear();
			DataStats dataStats = new DataStats(_gcodeFilePath);
			dataStats.absoluteScalar = _absoluteScalarSlider.value;
			dataStats.printMaterial = _dropDownManager.GetPrintMaterial();
			dataStats.printBedMaterial = _dropDownManager.GetPrintBedMaterial();
			dataStats.datapackName = _validateInput.GetInput();

			sourceCancel = new CancellationTokenSource();
			_currentProgress = 0;
			
			foreach (ProgressAmount<float> pa in progresses)
			{
				pa.ReportValue(0, "");	// Make sure value is reset
				pa.ValueChangedEvent += UpdateProgressBarValue;
			}

			try
			{
				dataStats = await GcodeManager.GcodeToParsedPaddedCSVAsync(dataStats, progresses[0], sourceCancel.Token);
				
				DatapackManager datapackManager = new DatapackManager();
				dataStats = await datapackManager.GenerateAsync(dataStats, progresses[1], sourceCancel.Token);
				
				if (_computeDatapackStats.isOn)
				{
					DatapackStats datapackStats = new DatapackStats();
					await datapackStats.Calculate(dataStats.datapackPath, progresses[2], sourceCancel.Token);

					dataStats.estimatedPrintTime = await TimeEstimator.CalculateEstimatedTime(dataStats.parsedGcodePath, dataStats.absoluteScalar, progresses[3], sourceCancel.Token);

					_statsManager.DisplayStats("Stats:", dataStats, datapackStats);
				}

				File.Delete(dataStats.parsedGcodePath);

				await Archive.CompressAsync(dataStats.datapackPath, Path.Combine(_datapackOutputPath, dataStats.datapackName + ".zip"), progresses[4], sourceCancel.Token, true);


				sourceCancel.Dispose();
				Destroy(_progressTracker.gameObject);
			}
			catch(OperationCanceledException)
			{
				if(File.Exists(dataStats.parsedGcodePath))
					File.Delete(dataStats.parsedGcodePath);

				if(_progressTracker)
					Destroy(_progressTracker);

				if (sourceCancel != null)
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

	/// <summary>
	/// Update the current progress bar
	/// </summary>
	/// <param name="updateAmount">The new amount of the progress bar</param>
	public void UpdateProgressBarValue(ProgressAmount<float> updateAmount)
	{
		if(_currentProgress != updateAmount.Id)
		{
			_currentProgress = updateAmount.Id;
		}
	}

	/// <summary>
	/// Cancels the datapack async generation functions
	/// </summary>
	public void CancelDatapackGeneration()
	{
		if(_progressTracker)
		{
			_progressTracker.CanceledEvent -= CancelDatapackGeneration;
		}

		if(sourceCancel != null && !sourceCancel.IsCancellationRequested)
		{
			sourceCancel.Cancel();
			Debug.Log("CANCELED!!!!");
		}
	}

	/// <summary>
	/// Acts like a validation function for when the user is allowed to generate the datapack
	/// </summary>
	/// <returns>True if user can generate datapack</returns>
	private bool AreSelectedPathsValid()
	{
		bool areValid = File.Exists(_gcodeFilePath) && Directory.Exists(_datapackOutputPath);
		GenerateDatapackButton.interactable = areValid;

		return areValid;
	}
}
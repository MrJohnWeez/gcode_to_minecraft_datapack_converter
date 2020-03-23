// Created by MrJohnWeez
// March 2020
//
using UnityEngine;
using SFB;
using TMPro;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;

/// <summary>
/// Manages the highest level of logic within this application.
/// Create a pipeline of Gcode -> Parsed padded CSV -> Datapack
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
	private CancellationTokenSource asyncSourceCancel;
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

		if (_progressTracker)
		{
			_progressTracker.SetMainValue(total, progresses[_currentProgress].Message);
			_progressTracker.SetSubValue(progresses[_currentProgress].Data, progresses[_currentProgress].SubMessage);
		}
	}
	#endregion UnityCallbacks

	#region FileSelection
	/// <summary>
	/// Lets the user select a gcode file to be parsed and converted
	/// </summary>
	public void SelectGcodeFile()
	{
		string[] gCodePaths = StandaloneFileBrowser.OpenFilePanel("Select Gcode file", "", extensions, false);
		string newPath = gCodePaths.Length > 0 ? gCodePaths[0] : "";
		if (!newPath.IsEmpty())
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
		if (!newPath.IsEmpty())
		{
			_datapackOutputPath = newPath;
			_datapackOutputPathText.text = _datapackOutputPath;
			AreSelectedPathsValid();
		}
	}

	#endregion FileSelection

	/// <summary>
	/// Convert and generate a datapack if the input and output paths are valid
	/// </summary>
	public async void ConvertAndCreateDatapackAsync()
	{
		if (AreSelectedPathsValid())
		{
			// Create progress window
			_progressTracker = Instantiate(progressTrackerPrefab);
			_progressTracker.name = "Datapack Generation Progress";
			_progressTracker.CanceledEvent += CancelDatapackGeneration;
			int mainMax = _computeDatapackStats.isOn ? progresses.Length : progresses.Length - 2;
			_progressTracker.Configure("Converting Gcode to Minecraft Datapack...", mainMax, 1);

			// Clear and configure new stats
			_statsManager.Clear();
			DataStats dataStats = new DataStats(_gcodeFilePath);
			dataStats.absoluteScalar = _absoluteScalarSlider.value;
			dataStats.printMaterial = _dropDownManager.GetPrintMaterial();
			dataStats.printBedMaterial = _dropDownManager.GetPrintBedMaterial();
			dataStats.datapackName = _validateInput.GetInput();

			// Initulize async vars
			asyncSourceCancel = new CancellationTokenSource();
			_currentProgress = 0;
			foreach (ProgressAmount<float> pa in progresses)
			{
				pa.ReportValue(0, "");  // Make sure value is reset
				pa.ValueChangedEvent += UpdateProgressBarValue;
			}

			try
			{
				dataStats = await GcodeManager.GcodeToParsedPaddedCSVAsync(dataStats, progresses[0], asyncSourceCancel.Token);

				DatapackManager datapackManager = new DatapackManager();
				dataStats = await datapackManager.GenerateAsync(dataStats, progresses[1], asyncSourceCancel.Token);

				// Compute datapack stats and estimated print time if option allows
				if (_computeDatapackStats.isOn)
				{
					DatapackStats datapackStats = new DatapackStats();
					await datapackStats.Calculate(dataStats.datapackPath, progresses[2], asyncSourceCancel.Token);

					dataStats.estimatedPrintTime = await TimeEstimator.CalculateEstimatedTime(dataStats.parsedGcodePath, dataStats.absoluteScalar, progresses[3], asyncSourceCancel.Token);

					_statsManager.DisplayStats("Stats:", dataStats, datapackStats);
				}

				File.Delete(dataStats.parsedGcodePath); // Remove temp csv file

				// Convert datapack to zip file and place where user selected
				await Archive.CompressAsync(dataStats.datapackPath, Path.Combine(_datapackOutputPath, dataStats.datapackName + ".zip"), progresses[4], asyncSourceCancel.Token, true);

				asyncSourceCancel.Dispose();
				Destroy(_progressTracker.gameObject);   // Remove progress menu
			}
			catch (OperationCanceledException)
			{
				// User canceled datapack generation so clean up files
				if (File.Exists(dataStats.parsedGcodePath))
					File.Delete(dataStats.parsedGcodePath);

				// Remove any half way complete datapack files
				if (Directory.Exists(dataStats.datapackPath))
					Directory.Delete(dataStats.datapackPath, true);

				if (_progressTracker)
					Destroy(_progressTracker);

				if (asyncSourceCancel != null)
					asyncSourceCancel.Dispose();
			}
			catch (ObjectDisposedException wasAreadyCanceled)
			{
				Debug.LogError("Objects was already disposed!\n" + wasAreadyCanceled);
			}

			foreach (ProgressAmount<float> pa in progresses)
				pa.ValueChangedEvent -= UpdateProgressBarValue;
		}
	}

	/// <summary>
	/// Update the current progress bar from async calls
	/// </summary>
	/// <param name="updateAmount">The calling Progress Amount object</param>
	public void UpdateProgressBarValue(ProgressAmount<float> updateAmount)
	{
		if (_currentProgress != updateAmount.Id)
			_currentProgress = updateAmount.Id;
	}

	/// <summary>
	/// Cancels the datapack async generation functions
	/// </summary>
	public void CancelDatapackGeneration()
	{
		if (_progressTracker)
			_progressTracker.CanceledEvent -= CancelDatapackGeneration;

		if (asyncSourceCancel != null && !asyncSourceCancel.IsCancellationRequested)
			asyncSourceCancel.Cancel();
	}

	/// <summary>
	/// Acts like a validation function for when the user is allowed to generate the datapack
	/// </summary>
	/// <returns>True if user can generate datapack (paths are valid)</returns>
	private bool AreSelectedPathsValid()
	{
		bool areValid = File.Exists(_gcodeFilePath) && Directory.Exists(_datapackOutputPath);
		GenerateDatapackButton.interactable = areValid;
		return areValid;
	}
}
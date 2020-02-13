using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;

/// <summary>
/// Handles gcode management
/// </summary>
public class FileManager : MonoBehaviour
{
	private const string _instructions = "Select a 3D printer Gcode file.";
	
	[SerializeField] private TMP_Text _gcodeDisplay = null;
	[SerializeField] private TMP_Text _filePathDisplay = null;

	private GcodeManager _gocdeManager = new GcodeManager();
	private List<List<string>> _parsedGcode = new List<List<string>>();
	private static string[] _gcodeLines = null;
	private string _gcodePath = "";

	private void Start()
	{
		_gcodeDisplay.text = _instructions;
	}

	/// <summary>
	/// Lets the user select a gcode file.
	/// </summary>
	public void GetGcodePath()
    {
		var extensions = new[] { new ExtensionFilter("RepRap toolchain Gcode File", "gcode") };
		string[] gCodePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
		_gcodePath = gCodePaths.Length > 0 ? gCodePaths[0] : "";
		_filePathDisplay.text = _gcodePath;

		if(!string.IsNullOrEmpty(_gcodePath))
		{
			_gcodeLines = SafeFileManagement.DisplayFile(_gcodePath, _gcodeDisplay);
			_parsedGcode = _gocdeManager.ParseGcodeFile(_gcodeLines);
		}
		else
			_gcodeDisplay.text = _instructions;
	}

	public List<List<string>> GetParsedGcodeLines()
	{
		return _parsedGcode;
	}

	public string GetGcodeFilePath()
	{
		return _gcodePath;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;
using System.IO;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles all file selection, reading, writing
/// </summary>
public class FileManager : MonoBehaviour
{
	private const string _instructions = "Select a 3D printer Gcode file.";
	[NonSerialized] public string currentGcodePath = "";
	[SerializeField] private TMP_Text _gcodeDisplay = null;
	[SerializeField] private TMP_Text _filePathDisplay = null;
	private static string[] _fileLines = null;

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
		currentGcodePath = gCodePaths.Length > 0 ? gCodePaths[0] : "";
		_filePathDisplay.text = currentGcodePath;

		if(!string.IsNullOrEmpty(_filePathDisplay.text))
			_ = DisplayFileAsync(_filePathDisplay.text, _gcodeDisplay);
		else
			_gcodeDisplay.text = _instructions;
	}

	/// <summary>
	/// Returns the contense of the gcode file (if read in)
	/// </summary>
	/// <returns>fileLines</returns>
	public string[] GetFileArray()
	{
		return _fileLines;
	}

	/// <summary>
	/// Loads entire file into memory then displays it on the given tmp text object.
	/// </summary>
	/// <param name="path">The path of the file to read</param>
	/// <param name="textObject">Text mesh pro object used to display file contense</param>
	static async Task DisplayFileAsync(string path, TMP_Text textObject)
	{
		string fileAsString = "";
		textObject.text = "Loading file...";
		try
		{
			using (var sr = new StreamReader(path))
			{
				fileAsString = await sr.ReadToEndAsync();
			}
		}
		catch (Exception e)
		{
			// Let the user know what went wrong.
			textObject.text = "The file could not be read:\nError: ";
			textObject.text += e.Message.ToString();
		}
		finally
		{
			textObject.text = fileAsString;
			_fileLines = fileAsString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		}
	}
}

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
	public string currentGcodePath = "";
	[SerializeField] private TMP_Text _gcodeDisplay = null;
	[SerializeField] private TMP_Text _filePathDisplay = null;
	private Char[] _gcodeFileBuffer = null;

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
		{
			_ = DisplayFileAsync(_filePathDisplay.text, _gcodeFileBuffer, _gcodeDisplay);
		}
		else
		{
			_gcodeDisplay.text = _instructions;
		}
	}

	/// <summary>
	/// Returns the contense of the gcode file (if read in)
	/// </summary>
	/// <returns></returns>
	public Char[] GetGcodeFileBuffer()
	{
		return _gcodeFileBuffer;
	}

	/// <summary>
	/// Loads entire file into memory then displays it on the given tmp text object.
	/// </summary>
	/// <param name="path">The path of the file to read</param>
	/// <param name="buffer">Buffer object that holds entire read in file</param>
	/// <param name="textObject">Text mesh pro object used to display file contense</param>
	/// <returns></returns>
	static async Task DisplayFileAsync(string path, char[] buffer, TMP_Text textObject)
	{
		try
		{
			using (var sr = new StreamReader(path))
			{
				textObject.text = "Loading file...";
				buffer = new Char[(int)sr.BaseStream.Length];
				await sr.ReadAsync(buffer, 0, (int)sr.BaseStream.Length);
			}

			textObject.text = new String(buffer);
		}
		catch (Exception e)
		{
			// Let the user know what went wrong.
			textObject.text = "The file could not be read:\nError: ";
			textObject.text += e.Message.ToString();
		}
	}
}

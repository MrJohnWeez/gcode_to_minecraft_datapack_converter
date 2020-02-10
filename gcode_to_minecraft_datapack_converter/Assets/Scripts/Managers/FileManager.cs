﻿using System.Collections;
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
			DisplayFile(_gcodePath, _gcodeDisplay);
			_gocdeManager.ParseGcodeFile(_gcodeLines);
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

	/// <summary>
	/// Loads entire file into memory then displays it on the given tmp text object.
	/// </summary>
	/// <param name="path">The path of the file to read</param>
	/// <param name="textObject">Text mesh pro object used to display file contense</param>
	public static void DisplayFile(string path, TMP_Text textObject)
	{
		string fileAsString = "";
		textObject.text = "Loading file...";
		try
		{
			using (var sr = new StreamReader(path))
			{
				fileAsString = sr.ReadToEnd();
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
			_gcodeLines = fileAsString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		}
	}

	public static string GetFileName(string inFileName)
	{
		int index = inFileName.LastIndexOf(".");
		if (index > 0)
			return inFileName.Substring(0, index);

		return inFileName;
	}

	public static string GetDateNow()
	{
		return DateTime.Now.ToString("yyyyMMddHHmmss");
	}
}

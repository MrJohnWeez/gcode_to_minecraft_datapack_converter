using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using TMPro;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Threading;

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

	/// <summary>
	/// Get a file's entire contents
	/// </summary>
	/// <param name="filePath">Path of a file to get contents</param>
	/// <returns>Entire file as one string</returns>
	public static string GetFileContents(string filePath)
	{
		string returnThis = "";
		try
		{
			using (var sr = new StreamReader(filePath))
			{
				returnThis = sr.ReadToEnd();
			}
		}
		catch (Exception e)
		{
			Debug.Log("The file could not be read:\nError: \n" + e.Message.ToString());
		}
		return returnThis;
	}

	/// <summary>
	/// Sets a file's contents to a given string
	/// </summary>
	/// <param name="filePath">Path of a file to set</param>
	/// <param name="content">The new contents of an entire file</param>
	public static void SetFileContents(string filePath, string content)
	{
		try
		{
			using (var sw = new StreamWriter(filePath))
			{
				sw.WriteLine(content);
			}
		}
		catch (Exception e)
		{
			Debug.Log("The file could not be written to:\nError: \n" + e.Message.ToString());
		}
	}

	/// <summary>
	/// Get the name of a file before extension
	/// </summary>
	/// <param name="inFileName">The name of the file including extension</param>
	/// <returns>Name of a file without extension name</returns>
	public static string GetFileName(string inFileName)
	{
		int index = inFileName.LastIndexOf(".");
		if (index > 0)
			return inFileName.Substring(0, index);

		return inFileName;
	}

	/// <summary>
	/// Returns the given date
	/// </summary>
	/// <returns>Year as string</returns>
	public static string GetDateNow()
	{
		return DateTime.Now.ToString("yyyyMMddHHmmss");
	}

	/// <summary>
	/// Opens a native file browers and retunrs a path selcted
	/// </summary>
	/// <param name="dialogTitle">What to display on the window popup</param>
	/// <param name="startingFolder">The folder to start user in</param>
	/// <returns></returns>
	public static string FolderPath(string dialogTitle, string startingFolder = "")
	{
		string[] selectedRootFolder = StandaloneFileBrowser.OpenFolderPanel(dialogTitle, startingFolder, false);

		if (selectedRootFolder.Length > 0)
		{
			return selectedRootFolder[0];
		}

		return "";
	}

	/// <summary>
	/// Copy entire directory to a different location
	/// </summary>
	/// <param name="sourceDirName">The folder to copy files from</param>
	/// <param name="destDirName">The location to copy files to</param>
	/// <param name="copySubDirs">Should sub files and folders be copied</param>
	/// <param name="excludeExtensions">String array of any file extensitions to not copy</param>
	/// <param name="retryAttempts">Number of retried system will preform if errors are encountered</param>
	/// <returns></returns>
	public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, string[] excludeExtensions = null, int retryAttempts = 0)
	{
		bool didCopy = false;
		int failedTimes = 0;

		while (!didCopy && failedTimes <= retryAttempts)
		{
			if (failedTimes > 0)
			{
				print("Trying again! Sleeping...");
				Thread.Sleep(50);
			}
				
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs;
			try
			{
				dirs = dir.GetDirectories();
			}
			catch (UnauthorizedAccessException accessDenied)
			{
				Debug.Log(accessDenied);
				failedTimes++;
				continue;
			}

			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			FileInfo[] files;
			try
			{
				files = dir.GetFiles();
			}
			catch (UnauthorizedAccessException accessDenied)
			{
				Debug.Log(accessDenied);
				failedTimes++;
				continue;
			}

			// Get the files in the directory and copy them to the new location.
			if (files != null)
			{
				foreach (FileInfo file in files)
				{
					string temppath = Path.Combine(destDirName, file.Name);
					bool skipThisFile = false;
					if (excludeExtensions != null)
					{
						foreach (string ext in excludeExtensions)
						{
							skipThisFile = ext == Path.GetExtension(temppath);
						}
					}

					if (!skipThisFile)
					{
						try
						{
							file.CopyTo(temppath, true);
						}
						catch (UnauthorizedAccessException accessDenied)
						{
							Debug.Log(accessDenied);
							failedTimes++;
							continue;
						}
					}
				}
			}
			
			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs && dirs != null)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					didCopy = DirectoryCopy(subdir.FullName, temppath, copySubDirs, excludeExtensions, retryAttempts);
				}
			}
			didCopy = true;
		}
		return didCopy;
	}
}

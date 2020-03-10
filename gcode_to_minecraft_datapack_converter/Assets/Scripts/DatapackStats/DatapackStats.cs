﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Runs calculations on a datapack folder
/// </summary>
public class DatapackStats
{
	private const string C_Mcfunction = ".mcfunction";

	public int linesOfCode = 0;
	public int numOfFunctions = 0;
	public int numOfFiles = 0;
	public int numOfDirectories = 0;
	public string datapackPath = "";

	public Task Calculate(string datapackRootPath, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			progess.ReportValue(0.0f, "Calculating Stats");
			datapackPath = datapackRootPath;
			string[] filesAndDir = Directory.GetFileSystemEntries(datapackPath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < filesAndDir.Length; i++)
			{
				CalculateThis(filesAndDir[i]);
				progess.ReportValue((float)i / filesAndDir.Length, "Calculating Stats");
				cancellationToken.ThrowIfCancellationRequested();
			}
			progess.ReportValue(1.0f, "Calculating Stats");
		});
	}

	/// <summary>
	/// Calculate the stats on a given file path
	/// </summary>
	/// <param name="path">File path to check</param>
	private void CalculateThis(string path)
	{
		if (File.Exists(path))
		{
			numOfFiles++;
			if (path.Contains(C_Mcfunction))
			{
				numOfFunctions++;
				linesOfCode += CountLinesOfCode(path);
			}
		}
		else if (Directory.Exists(path))
		{
			numOfDirectories++;
		}
	}

	/// <summary>
	/// Counts the lines of non-commented minecraft function code within a file
	/// </summary>
	/// <param name="filePath">File to scan for code</param>
	/// <returns>Number of minecraft code lines within a file</returns>
	private int CountLinesOfCode(string filePath)
	{
		int lineCount = 0;
		try
		{
			using (var fileReader = new StreamReader(filePath))
			{
				string currentLine = "";
				while (!fileReader.EndOfStream)
				{
					currentLine = fileReader.ReadLine();
					int commentIndex = currentLine.Trim().IndexOf('#');
					if (commentIndex > 0 && !string.IsNullOrWhiteSpace(currentLine) && !string.IsNullOrEmpty(currentLine))
						lineCount++;

				}
			}
		}
		catch (Exception e)
		{
			LogError("The gcode file could not be written to", e);
		}

		return lineCount;
	}

	private void LogError(string text, Exception error)
	{
		Debug.LogError("Error\n" + text + "\n" + error.Message);
	}
}

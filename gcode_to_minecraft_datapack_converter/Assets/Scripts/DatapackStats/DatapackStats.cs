using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

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
			datapackPath = datapackRootPath;
			string[] filesAndDir = Directory.GetFileSystemEntries(datapackPath, "*", SearchOption.AllDirectories);
			foreach (string path in filesAndDir)
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
		});
	}

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

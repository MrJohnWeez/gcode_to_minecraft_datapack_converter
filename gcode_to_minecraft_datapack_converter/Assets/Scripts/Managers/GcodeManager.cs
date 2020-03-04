using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;






// Not sure what is happening but the 2nd csv file is not working








/// <summary>
/// Parse gcode using this static class
/// </summary>
public static class GcodeManager
{
	public static string ParsedPaddedCSVToMcodeCSV(string parsedPaddedCSVPath, in ParsedDataStats dataStats)
	{
		string mcodePath = "";
		string csvName = "ParsedPaddedCSVToMcodeCSV_" + SafeFileManagement.GetDateNow() + ".csv";
		mcodePath = Path.Combine(Application.temporaryCachePath, csvName);
		if (File.Exists(parsedPaddedCSVPath))
		{
			try
			{
				using (var mcodeFile = new StreamWriter(mcodePath))
				{
					mcodeFile.WriteLine("Xcord,Ycord,Zcord,XMotion,YMotion,ZMotion,ShouldExtrude");
					try
					{
						using (var paddedCSVFile = new StreamReader(parsedPaddedCSVPath))
						{
							string currentLine = "";
							paddedCSVFile.ReadLine();   // Skip header
							LastGcodeValues prevData = null;
							LastGcodeValues currData = null;
							while (!paddedCSVFile.EndOfStream)
							{
								currentLine = paddedCSVFile.ReadLine();
								currData = new LastGcodeValues(currentLine);

								McodeValues mcodeVals = new McodeValues(prevData, currData, dataStats.maxSpeed);
								prevData = currData;
								mcodeFile.WriteLine(mcodeVals.ToCSVString());
							}
						}
					}
					catch (Exception e)
					{ LogError("The gcode file could not be written to", e); }
				}
			}
			catch (Exception e)
			{ LogError("The csv file could not be written to", e); }
		}
		return mcodePath;
	}

	/// <summary>
	/// Converts a given gcode file path to a csv file containing padded x,y,z,extrude,movespeed
	/// </summary>
	/// <param name="gcodePath">path to the gcode file to parse</param>
	/// <returns>path to the created csv file</returns>
	public static string GcodeToParsedPaddedCSV(string gcodePath, ref ParsedDataStats dataStats)
	{
		string parsedPaddedCSVPath = "";
		string csvName = "GcodeToParsedPaddedCSV_" + SafeFileManagement.GetDateNow() + ".csv";
		parsedPaddedCSVPath = Path.Combine(Application.temporaryCachePath, csvName);
		if (File.Exists(gcodePath))
		{
			try
			{
				using (var csvFile = new StreamWriter(parsedPaddedCSVPath))
				{
					csvFile.WriteLine("Xcord,Ycord,Zcord,ShouldExtrude,MoveSpeed");
					try
					{
						using (var gcodeFile = new StreamReader(gcodePath))
						{
							LastGcodeValues lastValues = new LastGcodeValues();
							lastValues.pos = Vector3.zero;
							lastValues.exturedAmount = 0;
							lastValues.moveSpeed = 0;

							string currentLine = "";
							while (!gcodeFile.EndOfStream)
							{
								currentLine = gcodeFile.ReadLine();
								currentLine = RemoveNonGcode(currentLine);
								if (!string.IsNullOrWhiteSpace(currentLine))
								{
									csvFile.WriteLine(GcodeLineToCSVLine(currentLine, ref lastValues, ref dataStats));
								}
							}
						}
					}
					catch (Exception e)
					{ LogError("The gcode file could not be written to", e); }

				}
			}
			catch (Exception e)
			{ LogError("The csv file could not be written to", e); }
		}

		return parsedPaddedCSVPath;
	}

	/// <summary>
	/// Removes all comments, empty lines from a string
	/// </summary>
	/// <param name="inPhrase">phrase to parse</param>
	/// <returns>String containing only gcode commands</returns>
	private static string RemoveNonGcode(string inPhrase)
	{
		int index = inPhrase.IndexOf(";");
		if (index >= 0)
			inPhrase = inPhrase.Remove(index);

		return inPhrase;
	}

	/// <summary>
	/// Converts a gcode string into a csv string
	/// </summary>
	/// <param name="gcodeLine">gcode to convert</param>
	/// <param name="lastValues">last gcode values that were converted</param>
	/// <returns>gcode string as a csv string</returns>
	private static string GcodeLineToCSVLine(string gcodeLine, ref LastGcodeValues lastValues, ref ParsedDataStats stats)
	{
		string parsed = "";
		string[] sections = gcodeLine.Split(' ');

		if (sections.Length > 0)
		{
			string mainCodeTrimmed = sections[0].ToUpper().Trim();

			// Is gcode command a move command
			if (mainCodeTrimmed.Length == 2 && mainCodeTrimmed[0] == 'G' && mainCodeTrimmed[1] == '1')
			{
				for (int term = 1; term < sections.Length; term++)
				{
					string termTrimmed = sections[term].ToUpper().Trim();

					// Term is a minimum of 2 chars
					if (termTrimmed.Length > 1)
					{
						string stringValue = termTrimmed.Substring(1);
						switch (termTrimmed[0])
						{
							case 'X':
								lastValues.pos.x = float.Parse(stringValue);
								break;
							case 'Y':
								lastValues.pos.z = float.Parse(stringValue);
								break;
							case 'Z':
								lastValues.pos.y = float.Parse(stringValue);
								break;
							case 'E':
								lastValues.exturedAmount = float.Parse(stringValue);
								break;
							case 'F':
								lastValues.moveSpeed = float.Parse(stringValue);
								break;
						}
					}
				}
				stats.minExtrude = Mathf.Min(stats.minExtrude, lastValues.exturedAmount);
				stats.minSpeed = Mathf.Min(stats.minSpeed, lastValues.moveSpeed);
				stats.minPos.x = Mathf.Min(stats.minPos.x, lastValues.pos.x);
				stats.minPos.y = Mathf.Min(stats.minPos.y, lastValues.pos.y);
				stats.minPos.z = Mathf.Min(stats.minPos.z, lastValues.pos.z);

				stats.maxExtrude = Mathf.Min(stats.maxExtrude, lastValues.exturedAmount);
				stats.maxSpeed = Mathf.Min(stats.maxSpeed, lastValues.moveSpeed);
				stats.maxPos.x = Mathf.Min(stats.maxPos.x, lastValues.pos.x);
				stats.maxPos.y = Mathf.Min(stats.maxPos.y, lastValues.pos.y);
				stats.maxPos.z = Mathf.Min(stats.maxPos.z, lastValues.pos.z);

				parsed = lastValues.ToCSVString();
			}
		}

		return parsed;
	}


	private static void LogError(string text, Exception error)
	{
		Debug.Log("Error\n" + text + "\n" + error.Message);
	}
}

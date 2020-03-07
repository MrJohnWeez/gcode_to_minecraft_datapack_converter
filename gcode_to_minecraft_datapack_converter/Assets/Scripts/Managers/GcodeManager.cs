using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Parse gcode using this static class
/// </summary>
public static class GcodeManager
{
	#region PublicMembers
	public static bool ParsedPaddedCSVToMcodeCSV(ref ParsedDataStats dataStats)
	{
		string csvName = "ParsedPaddedCSVToMcodeCSV_" + SafeFileManagement.GetDateNow() + ".csv";
		dataStats.mcodePath = Path.Combine(Application.temporaryCachePath, csvName);
		if (File.Exists(dataStats.parsedGcodePath))
		{
			try
			{
				using (var mcodeFile = new StreamWriter(dataStats.mcodePath))
				{
					mcodeFile.WriteLine("Xcord,Ycord,Zcord,XMotion,YMotion,ZMotion,ShouldExtrude");
					try
					{
						using (var paddedCSVFile = new StreamReader(dataStats.parsedGcodePath))
						{
							string currentLine = "";
							paddedCSVFile.ReadLine();   // Skip header

							LastGcodeValues prevData = new LastGcodeValues();
							LastGcodeValues currData = new LastGcodeValues();

							// Add orgin as first value
							McodeValues mcodeVals = new McodeValues();
							mcodeFile.WriteLine(mcodeVals.ToCSVString());

							// Continue to convert and calculate until last value from file is read
							while (!paddedCSVFile.EndOfStream)
							{
								currentLine = paddedCSVFile.ReadLine();
								currData = new LastGcodeValues(currentLine);

								mcodeVals = new McodeValues(prevData, currData, dataStats.maxSpeed);
								prevData = currData;
								mcodeFile.WriteLine(mcodeVals.ToCSVString());
							}
							return true;
						}
					}
					catch (Exception e)
					{ LogError("The gcode file could not be written to", e); }
				}
			}
			catch (Exception e)
			{ LogError("The csv file could not be written to", e); }
		}
		return false;
	}

	/// <summary>
	/// Converts a given gcode file path to a csv file containing padded x,y,z,extrude,movespeed
	/// </summary>
	/// <param name="gcodePath">path to the gcode file to parse</param>
	/// <returns>path to the created csv file</returns>
	public static bool GcodeToParsedPaddedCSV(ref ParsedDataStats dataStats)
	{
		string csvName = "GcodeToParsedPaddedCSV_" + SafeFileManagement.GetDateNow() + ".csv";
		dataStats.parsedGcodePath = Path.Combine(Application.temporaryCachePath, csvName);
		if (File.Exists(dataStats.gcodePath))
		{
			try
			{
				using (var csvFile = new StreamWriter(dataStats.parsedGcodePath))
				{
					csvFile.WriteLine("Xcord,Ycord,Zcord,ShouldExtrude,MoveSpeed");
					try
					{
						using (var gcodeFile = new StreamReader(dataStats.gcodePath))
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
							return true;
						}
					}
					catch (Exception e)
					{ LogError("The gcode file could not be written to", e); }
				}
			}
			catch (Exception e)
			{ LogError("The csv file could not be written to", e); }
		}
		return false;
	}
	#endregion PublicMembers

	#region PrivateMembers
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
	private static string GcodeLineToCSVLine(string gcodeLine, ref LastGcodeValues lastValues, ref ParsedDataStats dataStats)
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
				dataStats.minExtrude = Mathf.Min(dataStats.minExtrude, lastValues.exturedAmount);
				dataStats.minSpeed = Mathf.Min(dataStats.minSpeed, lastValues.moveSpeed);
				dataStats.minPos.x = Mathf.Min(dataStats.minPos.x, lastValues.pos.x);
				dataStats.minPos.y = Mathf.Min(dataStats.minPos.y, lastValues.pos.y);
				dataStats.minPos.z = Mathf.Min(dataStats.minPos.z, lastValues.pos.z);

				dataStats.maxExtrude = Mathf.Max(dataStats.maxExtrude, lastValues.exturedAmount);
				dataStats.maxSpeed = Mathf.Max(dataStats.maxSpeed, lastValues.moveSpeed);
				dataStats.maxPos.x = Mathf.Max(dataStats.maxPos.x, lastValues.pos.x);
				dataStats.maxPos.y = Mathf.Max(dataStats.maxPos.y, lastValues.pos.y);
				dataStats.maxPos.z = Mathf.Max(dataStats.maxPos.z, lastValues.pos.z);

				parsed = lastValues.ToCSVString();
				dataStats.totalMcodeLines++;
			}
		}

		return parsed;
	}


	private static void LogError(string text, Exception error)
	{
		Debug.LogError("Error\n" + text + "\n" + error.Message);
	}

	#endregion PrivateMembers
}

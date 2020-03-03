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
	/// <summary>
	/// Datatype used to track the previous updated gcode term
	/// </summary>
	public struct LastGcodeValues
	{
		public string x;
		public string y;
		public string z;
		public string shouldExtrude;
		public string moveSpeed;

		/// <summary>
		/// Convert this struct to a string as a csv
		/// TODO: Use stringbuilder if this struct gets bigger
		/// </summary>
		/// <returns>string as a csv</returns>
		public string ToCSVString()
		{
			return x + "," + y + "," + z + "," + shouldExtrude + "," + moveSpeed + ",";
		}
	}

	/// <summary>
	/// Converts a given gcode file path to a csv file containing padded x,y,z,extrude,movespeed
	/// </summary>
	/// <param name="gcodePath">path to the gcode file to parse</param>
	/// <returns>path to the created csv file</returns>
	public static string GcodeToParsedPaddedCSV(string gcodePath)
	{
		string tempPath = "";
		string csvName = "GcodeToParsedPaddedCSV_" + SafeFileManagement.GetDateNow() + ".csv";
		tempPath = Path.Combine(Application.temporaryCachePath, csvName);
		if (File.Exists(gcodePath))
		{
			try
			{
				using (var csvFile = new StreamWriter(tempPath))
				{
					csvFile.WriteLine("x,y,z,extrude,movespeed");
					try
					{
						using (var gcodeFile = new StreamReader(gcodePath))
						{
							LastGcodeValues lastValues = new LastGcodeValues();
							lastValues.x = "0";
							lastValues.y = "0";
							lastValues.z = "0";
							lastValues.shouldExtrude = "0";
							lastValues.moveSpeed = "0";

							string currentLine = "";
							while (!gcodeFile.EndOfStream)
							{
								currentLine = gcodeFile.ReadLine();
								currentLine = RemoveNonGcode(currentLine);
								if (!string.IsNullOrWhiteSpace(currentLine))
								{
									csvFile.WriteLine(GcodeLineToCSVLine(currentLine, ref lastValues));
								}
							}
						}
					}
					catch (Exception e)
					{
						Debug.Log("The gcode file could not be written to:\nError: \n" + e.Message.ToString());
					}

				}
			}
			catch (Exception e)
			{
				Debug.Log("The csv file could not be written to:\nError: \n" + e.Message.ToString());
			}
		}

		return tempPath;
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
	private static string GcodeLineToCSVLine(string gcodeLine, ref LastGcodeValues lastValues)
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
								lastValues.x = stringValue;
								break;
							case 'Y':
								lastValues.z = stringValue;
								break;
							case 'Z':
								lastValues.y = stringValue;
								break;
							case 'E':
								lastValues.shouldExtrude = stringValue;
								break;
							case 'F':
								lastValues.moveSpeed = stringValue;
								break;

						}
					}
				}
				parsed = lastValues.ToCSVString();
			}
		}

		return parsed;
	}
}

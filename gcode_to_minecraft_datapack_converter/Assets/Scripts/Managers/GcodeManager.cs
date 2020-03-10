using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Parse gcode using this static class
/// </summary>
public static class GcodeManager
{
	#region PublicMembers

	/// <summary>
	/// Converts csv file of x,y,z,extrude,movespeed to xpos,ypos,zpos,xmov,ymov,zmov,shouldExtrude
	/// </summary>
	/// <param name="dataStats">The stats class used when parsing gcode files</param>
	/// <param name="progess">The ProgressAmount class that keeps track of this function's progress 0.0 -> 1.0</param>
	/// <param name="cancellationToken">Token that allows async function to be canceled</param>
	/// <returns>Modified ParsedDataStats type</returns>
	public static Task<ParsedDataStats> ParsedPaddedCSVToMcodeCSV(ParsedDataStats dataStats, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			progess.ReportValue(0.0f, "Converting Gcode");
			string csvName = "ParsedPaddedCSVToMcodeCSV_" + SafeFileManagement.GetDateNow() + ".csv";
			dataStats.mcodePath = Path.Combine(dataStats.tempFilePath, csvName);
			if (File.Exists(dataStats.parsedGcodePath))
			{
				try
				{
					using (var mcodeFile = new StreamWriter(dataStats.mcodePath))
					{
						mcodeFile.WriteLine("Xcord,Ycord,Zcord,XMotion,YMotion,ZMotion,ShouldExtrude");
						try
						{
							FileInfo parsedGcodeFileInfo = new FileInfo(dataStats.parsedGcodePath);
							long parsedGcodeFileLength = parsedGcodeFileInfo.Length;
							long byteCount = 0;

							string currentLine = "";
							LastGcodeValues prevData = new LastGcodeValues();
							LastGcodeValues currData = new LastGcodeValues();

							// Add orgin as first value
							McodeValues mcodeVals = new McodeValues();
							mcodeFile.WriteLine(mcodeVals.ToCSVString());

							using (var paddedCSVFile = new StreamReader(dataStats.parsedGcodePath))
							{
								
								paddedCSVFile.ReadLine();   // Skip header
								
								// Continue to convert and calculate until last value from file is read
								while (!paddedCSVFile.EndOfStream)
								{
									currentLine = paddedCSVFile.ReadLine();

									byteCount += System.Text.Encoding.Unicode.GetByteCount(currentLine);
									progess.ReportValue(byteCount / parsedGcodeFileLength, "Converting Gcode");
									cancellationToken.ThrowIfCancellationRequested();

									currData = new LastGcodeValues(currentLine);

									mcodeVals = new McodeValues(prevData, currData, dataStats.maxSpeed);
									prevData = currData;
									mcodeFile.WriteLine(mcodeVals.ToCSVString());
								}
							}
						}
						catch (OperationCanceledException wasCanceled)
						{
							throw wasCanceled;
						}
						catch (ObjectDisposedException wasAreadyCanceled)
						{
							throw wasAreadyCanceled;
						}
						catch (Exception e)
						{ LogError("The gcode file could not be written to", e); }
					}
				}
				catch (Exception e)
				{ LogError("The csv file could not be written to", e); }
			}
			progess.ReportValue(1.0f, "Converting Gcode");
			return dataStats;
		});
	}
	
	/// <summary>
	/// Converts a given gcode file path to a csv file containing padded x,y,z,extrude,movespeed
	/// </summary>
	/// <param name="dataStats">The stats class used when parsing gcode files</param>
	/// <param name="progess">The ProgressAmount class that keeps track of this function's progress 0.0 -> 1.0</param>
	/// <param name="cancellationToken">Token that allows async function to be canceled</param>
	/// <returns>Modified ParsedDataStats type</returns>
	public static Task<ParsedDataStats> GcodeToParsedPaddedCSV(ParsedDataStats dataStats, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			progess.ReportValue(0.0f, "Parsing Gcode");
			string csvName = "GcodeToParsedPaddedCSV_" + SafeFileManagement.GetDateNow() + ".csv";
			dataStats.parsedGcodePath = Path.Combine(dataStats.tempFilePath, csvName);
			
			if (File.Exists(dataStats.gcodePath))
			{
				try
				{
					using (var csvFile = new StreamWriter(dataStats.parsedGcodePath))
					{
						csvFile.WriteLine("Xcord,Ycord,Zcord,ShouldExtrude,MoveSpeed");
						try
						{
							FileInfo gcodeFileInfo = new FileInfo(dataStats.gcodePath);
							long gcodeFileLength = gcodeFileInfo.Length;
							long byteCount = 0;

							LastGcodeValues lastValues = new LastGcodeValues();
							lastValues.pos = Vector3.zero;
							lastValues.exturedAmount = 0;
							lastValues.moveSpeed = 0;
							string currentLine = "";

							using (var gcodeFile = new StreamReader(dataStats.gcodePath))
							{
								while (!gcodeFile.EndOfStream)
								{
									currentLine = gcodeFile.ReadLine();

									byteCount += System.Text.Encoding.Unicode.GetByteCount(currentLine);
									progess.ReportValue(byteCount / gcodeFileLength, "Parsing Gcode");
									cancellationToken.ThrowIfCancellationRequested();

									currentLine = RemoveNonGcode(currentLine);
									if (!currentLine.IsEmpty())
									{
										string g1Code = GcodeLineToCSVLine(currentLine, ref lastValues, ref dataStats);
										if(g1Code.Length >= 10)
										{
											csvFile.WriteLine(g1Code);
										}
									}
								}
							}
						}
						catch (OperationCanceledException wasCanceled)
						{
							throw wasCanceled;
						}
						catch (ObjectDisposedException wasAreadyCanceled)
						{
							throw wasAreadyCanceled;
						}
						catch (Exception e)
						{ LogError("The gcode file could not be written to", e); }
					}
				}
				catch (Exception e)
				{ LogError("The csv file could not be written to", e); }
			}
			progess.ReportValue(1.0f, "Parsing Gcode");
			return dataStats;
		});
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

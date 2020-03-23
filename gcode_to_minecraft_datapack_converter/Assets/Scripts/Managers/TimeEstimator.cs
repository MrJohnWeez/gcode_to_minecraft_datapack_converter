// Created by MrJohnWeez
// March 2020
//
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Calculates the estimated time that datapack would take to print out an object
/// if minecraft runs at 20 tps
/// </summary>
public static class TimeEstimator
{
	public const float tickTime = 1.0f / 20.0f;		// Minecraft's ticks per second (tps) when not lagging

	/// <summary>
	/// Calculate the estimated time that a datapack would take to print out a object
	/// </summary>
	/// <param name="parsedCSVFilePath">The path of the parsed gcode csv file</param>
	/// <param name="moveSpeed">The max move speed of the tip of the machine</param>
	/// <param name="progess">The ProgressAmount class that keeps track of this function's progress 0.0 -> 1.0</param>
	/// <param name="cancellationToken">Token that allows async function to be canceled</param>
	/// <returns>Estimated total time in seconds</returns>
	public static Task<float> CalculateEstimatedTime(string parsedCSVFilePath, float moveSpeed, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			progess.ReportValue(0.0f, "Calculating Estimated Print Time", "Reading folder");
			string currentLine = "";
			long byteCount = 0;
			float totalEstimatedTime = 0;
			
			try
			{
				// Get the length of the file in bytes
				FileInfo csvFileInfo = new FileInfo(parsedCSVFilePath);
				long csvFileLength = csvFileInfo.Length;

				GcodeStorage currentValues = new GcodeStorage();
				GcodeStorage prevValues = null;

				using (var csvFile = new StreamReader(parsedCSVFilePath))
				{
					csvFile.ReadLine();
					byteCount += System.Text.Encoding.Unicode.GetByteCount(currentLine);

					while (!csvFile.EndOfStream)
					{
						currentLine = csvFile.ReadLine();

						// Send progress update
						byteCount += System.Text.Encoding.Unicode.GetByteCount(currentLine);
						progess.ReportValue(byteCount / csvFileLength, "Calculating Estimated Print Time", "Reading temp CSV file");
						cancellationToken.ThrowIfCancellationRequested();
						
						if (!currentLine.IsEmpty())
						{
							currentValues = new GcodeStorage(currentLine);
							float timeAmount = tickTime;

							if (prevValues != null)
							{
								float distance = Vector3.Distance(currentValues.pos, prevValues.pos);
								if(distance > 0)
									timeAmount += Mathf.Floor(distance / moveSpeed) * tickTime;
							}
							totalEstimatedTime += timeAmount;

							prevValues = currentValues;
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


			progess.ReportValue(1.0f, "Calculating Estimated Print Time" , "Finished");
			return totalEstimatedTime;
		});
	}

	private static void LogError(string text, Exception error)
	{
		Debug.LogError("Error\n" + text + "\n" + error.Message);
	}
}

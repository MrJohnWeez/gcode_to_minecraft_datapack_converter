using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

public static class TimeEstimator
{
	public const float tickTime = 1.0f / 20.0f;
	public static Task<float> CalculateEstimatedTime(string parsedCSVFilePath, float moveSpeed, ProgressAmount<float> progess, CancellationToken cancellationToken)
	{
		return Task.Run(() =>
		{
			progess.ReportValue(0.0f, "Calculating Estimated Print Time");
			string currentLine = "";
			long byteCount = 0;
			float totalEstimatedTime = 0;
			
			try
			{
				FileInfo gcodeFileInfo = new FileInfo(parsedCSVFilePath);
				long gcodeFileLength = gcodeFileInfo.Length;
				LastGcodeValues currentValues = new LastGcodeValues();
				LastGcodeValues prevValues = null;

				using (var csvFile = new StreamReader(parsedCSVFilePath))
				{
					csvFile.ReadLine();
					byteCount += System.Text.Encoding.Unicode.GetByteCount(currentLine);

					while (!csvFile.EndOfStream)
					{
						currentLine = csvFile.ReadLine();

						byteCount += System.Text.Encoding.Unicode.GetByteCount(currentLine);
						progess.ReportValue(byteCount / gcodeFileLength, "Parsing Gcode");
						cancellationToken.ThrowIfCancellationRequested();
						
						if (!currentLine.IsEmpty())
						{
							currentValues = new LastGcodeValues(currentLine);
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


			progess.ReportValue(1.0f, "Calculating Estimated Print Time");
			return totalEstimatedTime;
		});
	}

	private static void LogError(string text, Exception error)
	{
		Debug.LogError("Error\n" + text + "\n" + error.Message);
	}
}

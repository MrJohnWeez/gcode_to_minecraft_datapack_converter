using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows calculations on McodeData type
/// </summary>
public static class McodeDataExtensions
{
	

	


	/// <summary>
	/// Logs the entire mcodeData varible to console
	/// </summary>
	/// <param name="value">McodeData self</param>
	public static void Log(this McodeData value)
	{
		string logMcodeData = "";
		foreach (McodeLine code in value.data)
		{
			logMcodeData += code.AsString() + "\n";
		}
		Debug.Log(logMcodeData);
	}
}

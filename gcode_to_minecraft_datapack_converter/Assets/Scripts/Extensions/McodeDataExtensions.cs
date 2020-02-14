using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows calculations on McodeData type
/// </summary>
public static class McodeDataExtensions
{
	/// <summary>
	/// Scales up the entire mcode dataset
	/// </summary>
	/// <param name="value">McodeData self</param>
	/// <param name="scaleFactor">The value to scale by</param>
	/// <returns></returns>
	public static McodeData Scale(this McodeData value, float scaleFactor)
	{
		return value;
	}

	/// <summary>
	/// Gets the minimum cordinate space used for print
	/// </summary>
	/// <param name="value">McodeData self</param>
	/// <returns>xyz vector of min space used</returns>
	public static Vector3 MinCord(this McodeData value)
	{
		float minX = 0;
		float minY = 0;
		float minZ = 0;

		if (value.data.Count > 0)
		{
			minX = value.data[0].pos.x;
			minY = value.data[0].pos.y;
			minZ = value.data[0].pos.z;

			foreach (McodeLine mcl in value.data)
			{
				minX = Mathf.Min(minX, mcl.pos.x);
				minY = Mathf.Min(minX, mcl.pos.y);
				minZ = Mathf.Min(minX, mcl.pos.z);
			}
		}

		return new Vector3(minX, minY, minZ);
	}

	/// <summary>
	/// Gets the maximun cordinate space used for print
	/// </summary>
	/// <param name="value">McodeData self</param>
	/// <returns>xyz vector of max space used</returns>
	public static Vector3 MaxCord(this McodeData value)
	{
		float maxX = 0;
		float maxY = 0;
		float maxZ = 0;

		if (value.data.Count > 0)
		{
			maxX = value.data[0].pos.x;
			maxY = value.data[0].pos.y;
			maxZ = value.data[0].pos.z;

			foreach(McodeLine mcl in value.data)
			{
				maxX = Mathf.Max(maxX, mcl.pos.x);
				maxY = Mathf.Max(maxX, mcl.pos.y);
				maxZ = Mathf.Max(maxX, mcl.pos.z);
			}
		}
		
		return new Vector3(maxX, maxY, maxZ);
	}


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to store gcode commands into datatypes
/// </summary>
public class GcodeStorage
{
	public Vector3 pos = Vector3.zero;
	public float exturedAmount = 0;
	public float moveSpeed = 0;

	public GcodeStorage()
	{

	}

	/// <summary>
	/// Converts a csv string to this data type struct
	/// </summary>
	/// <param name="lastGcodeValuesAsCSV">The string of csv terms</param>
	public GcodeStorage(string lastGcodeValuesAsCSV)
	{
		string[] sections = lastGcodeValuesAsCSV.Split(',');
		if (sections.Length >= 5)
		{
			pos.x = float.Parse(sections[0]);
			pos.y = float.Parse(sections[1]);
			pos.z = float.Parse(sections[2]);
			exturedAmount = float.Parse(sections[3]);
			moveSpeed = float.Parse(sections[4]);
		}
	}

	/// <summary>
	/// Convert this struct to a string as a csv
	/// TODO: Use stringbuilder if this struct gets bigger
	/// </summary>
	/// <returns>string as a csv</returns>
	public string ToCSVString()
	{
		return pos.x + "," + pos.y + "," + pos.z + "," + exturedAmount + "," + moveSpeed + ",";
	}
}

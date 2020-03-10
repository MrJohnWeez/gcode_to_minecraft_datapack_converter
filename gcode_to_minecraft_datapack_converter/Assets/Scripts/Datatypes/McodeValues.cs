using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to store data that will be directly used in the datapack creation of minecraft code (mcode)
/// </summary>
public class McodeValues
{
	private static readonly float MinRealisticSpeed = 0.1f;
	public Vector3 pos = new Vector3();
	public Vector3 motion = new Vector3();
	public bool shouldExtrude = false;

	public McodeValues()
	{

	}
	
	/// <summary>
	/// Creates a McodeValue that calculates motion data automatically
	/// </summary>
	/// <param name="prevGcodeValues">prv Gcode value struct</param>
	/// <param name="lastGcodeValues">current Gcode value struct</param>
	/// <param name="maxMoveSpeed">Limit of the max move speed</param>
	/// <param name="minMoveSpeed">Limit of the min move speed</param>
	public McodeValues(LastGcodeValues prevGcodeValues, LastGcodeValues lastGcodeValues, float maxMoveSpeed, float minMoveSpeed)
	{
		if(prevGcodeValues != null && lastGcodeValues != null)
		{
			pos = lastGcodeValues.pos;
			shouldExtrude = lastGcodeValues.exturedAmount > 0;
			motion = lastGcodeValues.pos - prevGcodeValues.pos;
			float newSpeed = Mathf.Clamp(lastGcodeValues.moveSpeed, minMoveSpeed, maxMoveSpeed);
			motion = motion.normalized * ConvertRange(minMoveSpeed, maxMoveSpeed, MinRealisticSpeed, 1, newSpeed);
		}
		else
		{
			if (lastGcodeValues != null)
			{
				pos = lastGcodeValues.pos;
				shouldExtrude = lastGcodeValues.exturedAmount > 0;
				motion = Vector3.zero;
			}
		}
	}

	/// <summary>
	/// Creates a McodeValue that calculates motion data automatically and controls all motion speed by scalar
	/// </summary>
	/// <param name="prevGcodeValues">prv Gcode value struct</param>
	/// <param name="lastGcodeValues">current Gcode value struct</param>
	/// <param name="absoluteScalar">The percentage of speed the print will run at in minecraft. 1 = max 20tps</param>
	public McodeValues(LastGcodeValues prevGcodeValues, LastGcodeValues lastGcodeValues, float absoluteScalar)
	{
		if (prevGcodeValues != null && lastGcodeValues != null)
		{
			pos = lastGcodeValues.pos;
			shouldExtrude = lastGcodeValues.exturedAmount > 0;
			motion = lastGcodeValues.pos - prevGcodeValues.pos;
			motion = motion.normalized * absoluteScalar;
		}
		else
		{
			if (lastGcodeValues != null)
			{
				pos = lastGcodeValues.pos;
				shouldExtrude = lastGcodeValues.exturedAmount > 0;
				motion = Vector3.zero;
			}
		}
	}

	public McodeValues(string mcodeAsCSVstring)
	{
		string[] sections = mcodeAsCSVstring.Split(',');
		if (sections.Length >= 7)
		{
			pos.x = float.Parse(sections[0]);
			pos.y = float.Parse(sections[1]);
			pos.z = float.Parse(sections[2]);
			motion.x = float.Parse(sections[3]);
			motion.y = float.Parse(sections[4]);
			motion.z = float.Parse(sections[5]);
			shouldExtrude = sections[6] == "1";
		}
	}

	/// <summary>
	/// Converts this struct into a csv string
	/// </summary>
	/// <returns>csv string</returns>
	public string ToCSVString()
	{
		return pos.x + "," + pos.y + "," + pos.z + "," + motion.x + "," + motion.y + "," + motion.z + "," + (shouldExtrude ? 1 : 0);
	}

	/// <summary>
	/// Converts a value from a range to a new range
	/// </summary>
	/// <param name="originalStart">Old start range value</param>
	/// <param name="originalEnd">Old end range value</param>
	/// <param name="newStart">New start range value</param>
	/// <param name="newEnd">New end range value</param>
	/// <param name="value">Value to scale</param>
	/// <returns>Scaled value within new range</returns>
	public static float ConvertRange(float originalStart, 
										float originalEnd,
										float newStart,
										float newEnd,
										float value)
	{
		double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
		return (float)(newStart + ((value - originalStart) * scale));
	}
}

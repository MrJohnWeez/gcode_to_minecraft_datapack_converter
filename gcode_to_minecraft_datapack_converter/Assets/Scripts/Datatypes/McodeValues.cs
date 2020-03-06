using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to store data that will be directly used in the datapack creation of minecraft code (mcode)
/// </summary>
public class McodeValues
{
	Vector3 pos = new Vector3();
	Vector3 motion = new Vector3();
	bool shouldExtrude = false;

	public McodeValues()
	{

	}
	
	/// <summary>
	/// Creates a McodeValue that calculates motion data automatically
	/// </summary>
	/// <param name="prevGcodeValues">prv Gcode value struct</param>
	/// <param name="lastGcodeValues">current Gcode value struct</param>
	/// <param name="maxMoveSpeed">Limit of the move speed</param>
	public McodeValues(LastGcodeValues prevGcodeValues, LastGcodeValues lastGcodeValues, in float maxMoveSpeed)
	{
		if(prevGcodeValues != null && lastGcodeValues != null)
		{
			pos = lastGcodeValues.pos;
			shouldExtrude = lastGcodeValues.exturedAmount > 0;
			motion = lastGcodeValues.pos - prevGcodeValues.pos;
			float newSpeed = Mathf.Clamp(lastGcodeValues.moveSpeed, 0, maxMoveSpeed);
			motion = motion.normalized * ConvertRange(0, maxMoveSpeed, 0, 1, newSpeed);
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



	// This is where I left off



	public McodeValues(string mcodeAsCSVstring)
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

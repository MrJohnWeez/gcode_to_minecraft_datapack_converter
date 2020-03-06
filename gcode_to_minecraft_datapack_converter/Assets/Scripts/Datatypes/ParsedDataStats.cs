using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data type that holds stats about parsed gcode data
/// </summary>
public class ParsedDataStats
{
	public Vector3 minPos = new Vector3();
	public Vector3 maxPos = new Vector3();
	public float minExtrude = 0;
	public float maxExtrude = 0;
	public float minSpeed = 0;
	public float maxSpeed = 0;
	public string gcodePath = "";
	public string mcodePath = "";
	public int totalLines = 0;

	public ParsedDataStats()
	{

	}

	public ParsedDataStats(string currentGcodePath)
	{
		gcodePath = currentGcodePath;
	}

	public string AsString()
	{
		return minPos.x + "," + minPos.y + "," + minPos.z + "," + maxPos.x + "," + maxPos.y + "," + maxPos.z + "," + minExtrude + "," + maxExtrude + "," + minSpeed + "," + maxSpeed;
	}
}

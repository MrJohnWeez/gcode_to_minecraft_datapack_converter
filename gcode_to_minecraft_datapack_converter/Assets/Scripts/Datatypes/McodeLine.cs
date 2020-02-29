using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that stores converted gcode data that will be used within minecraft command generation
/// </summary>
public class McodeLine
{
	public Vector3 pos = new Vector3();
	public float magnitude = 1; // Feedrate and acceleration control (magnitude that motion vector should have
	public bool extrude = false;
	public float errorRadius = 0;   // Minecraft lag detection/prevention/catching
	public Vector3 motion = new Vector3();     // Motion of arm moving to x,y,z cord

	public McodeLine()
	{

	}

	public McodeLine(Vector3 inPos, float inMagnitude, bool inExtrude)
	{
		pos = inPos;
		magnitude = inMagnitude;
		errorRadius = magnitude;
		extrude = inExtrude;
	}

	/// <summary>
	/// Converts this class type into one string. Can be useful for displaying
	/// </summary>
	/// <returns>class varibles as a concatinated string</returns>
	public string AsString()
	{
		return "pos: " + pos.ToString("F3") +
				" motion: " + motion.ToString("F3") +
				" mag: " + magnitude.ToString() +
				" extrude: " + extrude;
	}

}

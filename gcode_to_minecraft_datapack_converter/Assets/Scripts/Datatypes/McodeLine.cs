using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that stores converted gcode data that will be used within minecraft command generation
/// </summary>
public class McodeLine
{
	public float x = 0;	// X position
	public float y = 0;	// Y position
	public float z = 0;	// Z position
	public float magnitude = 0;	// Feedrate and acceleration control (magnitude that motion vector should have
	public float errorRadius = 0;   // Minecraft lag detection/prevention/catching
	public Vector3 motion = new Vector3();     // Motion of arm moving to x,y,z cord

	public McodeLine()
	{

	}

	public McodeLine(float inX, float inY, float inZ, float inMagnitude)
	{
		x = inX;
		y = inY;
		z = inZ;
		magnitude = inMagnitude;
		errorRadius = magnitude;
	}

	/// <summary>
	/// Converts this class type into one string. Can be useful for displaying
	/// </summary>
	/// <returns>class varibles as a concatinated string</returns>
	public string AsString()
	{
		return "X: " + x.ToString() + " Y: " + y.ToString() + " Z: " + z.ToString() + " mag: " + magnitude.ToString() + " motion: " + motion.ToString();
	}

}

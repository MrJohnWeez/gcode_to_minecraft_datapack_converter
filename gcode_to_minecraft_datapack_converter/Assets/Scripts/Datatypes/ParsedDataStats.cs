using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data type that holds stats about parsed gcode data
/// </summary>
public struct ParsedDataStats
{
	public Vector3 minPos;
	public Vector3 maxPos;
	public float minExtrude;
	public float maxExtrude;
	public float minSpeed;
	public float maxSpeed;
}

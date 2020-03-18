using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data type that holds stats about parsed gcode data
/// </summary>
public class ParsedDataStats
{
	// Gcode
	public int totalGcodeLines = 0;
	public int totalGcodeMoveLines = 0;

	// Mcode
	public Vector3 minPos = new Vector3();
	public Vector3 maxPos = new Vector3();
	public float minExtrude = 0;
	public float maxExtrude = 0;
	public float minSpeed = 0;
	public float maxSpeed = 0;
	public int totalMcodeLines = 0;

	// User Settings
	public float absoluteScalar = 1;

	// File Gen Paths
	public string gcodePath = "";
	public string parsedGcodePath = "";
	public string datapackPath = "";

	// Application Paths
	public string tempFilePath = "";
	public string unityDataPath = "";

	public ParsedDataStats(string inGcodePath, string inDatapackOutputPath)
	{
		gcodePath = inGcodePath;
		datapackPath = inDatapackOutputPath;
		tempFilePath = Application.temporaryCachePath;
		unityDataPath = Application.dataPath;
	}

	public ParsedDataStats(string inGcodePath, string inDatapackOutputPath, string inTempFilePath, string indataPath)
	{
		gcodePath = inGcodePath;
		datapackPath = inDatapackOutputPath;
		tempFilePath = inTempFilePath;
		unityDataPath = indataPath;
	}
}
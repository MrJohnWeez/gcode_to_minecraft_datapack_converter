// Created by MrJohnWeez
// March 2020
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data type that holds stats when generating a gcode -> datapack folder
/// </summary>
public class DataStats
{
	// Calculations
	public Vector3 minPos = new Vector3();
	public Vector3 maxPos = new Vector3();
	public float minExtrude = 0;
	public float maxExtrude = 0;
	public float minSpeed = 0;
	public float maxSpeed = 0;
	public int totalMcodeLines = 0;
	public float estimatedPrintTime = 0;
	public int totalGcodeLines = 0;
	public int totalGcodeMoveLines = 0;

	// User Settings
	public float absoluteScalar = 1;

	// Names
	public string datapackName = "";
	public string printMaterial = "";
	public string printBedMaterial = "";

	// File Gen Paths
	public string gcodePath = "";
	public string parsedGcodePath = "";
	public string datapackPath = "";

	// Application Paths
	public string tempFilePath = "";
	public string unityDataPath = "";

	public DataStats(string inGcodePath)
	{
		gcodePath = inGcodePath;
		datapackPath = Application.temporaryCachePath;
		tempFilePath = Application.temporaryCachePath;
		unityDataPath = Application.dataPath;
	}

	public DataStats(string inGcodePath, string inDatapackOutputPath, string inTempFilePath, string indataPath)
	{
		gcodePath = inGcodePath;
		datapackPath = inDatapackOutputPath;
		tempFilePath = inTempFilePath;
		unityDataPath = indataPath;
	}
}
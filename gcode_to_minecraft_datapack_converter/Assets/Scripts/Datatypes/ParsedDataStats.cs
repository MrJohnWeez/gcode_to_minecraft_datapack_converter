﻿using System.Collections;
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

	// Paths
	public string gcodePath = "";
	public string parsedGcodePath = "";
	public string mcodePath = "";

	// Datapack
	public int linesOfCode = 0;
	public int numberOfFunctions = 0;





	
	// Need to test datapack
	// Need to gather stats about datapack
	//		- Lines of code
	//		- Functions







}
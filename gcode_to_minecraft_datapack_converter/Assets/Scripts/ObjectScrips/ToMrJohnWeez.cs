// Created by MrJohnWeez
// March 2020
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that re-directs user to Websites
/// </summary>
public class WebManager : MonoBehaviour
{
	/// <summary>
	/// Open MrJohnWeez Website
	/// </summary>
    public void ToMrJohnWeezSite()
	{
		Application.OpenURL("https://www.mrjohnweez.com/");
	}

	/// <summary>
	/// Open MrJohnWeez Website
	/// </summary>
	public void DownloadResourcePack()
	{
		Application.OpenURL("https://github.com/MrJohnWeez/gcode_to_minecraft_datapack_converter/releases");
	}
}

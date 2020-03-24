// Created by MrJohnWeez
// March 2020
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that re-directs user to Websites
/// </summary>
public class WebManager: MonoBehaviour
{
	/// <summary>
	/// Open MrJohnWeez Website
	/// </summary>
    public void ToMrJohnWeezSite()
	{
		Application.OpenURL("https://www.mrjohnweez.com/");
	}

	/// <summary>
	/// Open DownloadResourcePack Website
	/// </summary>
	public void ToResourcePackSite()
	{
		Application.OpenURL("https://github.com/MrJohnWeez/3D_Printer_Emulator_In_Minecraft/releases");
	}
}

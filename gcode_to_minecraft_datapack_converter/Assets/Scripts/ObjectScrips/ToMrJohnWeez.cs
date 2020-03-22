// Created by MrJohnWeez
// March 2020
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that re-directs user to MrJohnWeez Website
/// </summary>
public class ToMrJohnWeez : MonoBehaviour
{
	/// <summary>
	/// Open MrJohnWeez Website
	/// </summary>
    public void ToMrJohnWeezSite()
	{
		Application.OpenURL("https://www.mrjohnweez.com/");
	}
}

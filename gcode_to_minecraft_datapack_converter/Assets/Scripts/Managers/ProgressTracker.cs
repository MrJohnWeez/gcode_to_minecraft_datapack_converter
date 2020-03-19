using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Manages a progress window for long tasks
/// </summary>
public class ProgressTracker : MonoBehaviour
{
	public delegate void ProgressTrackerCallback();
	public event ProgressTrackerCallback CanceledEvent;

	[SerializeField] private TMP_Text _panelTitle = null;
	[SerializeField] private CustomProgressBar _mainProgressBar = null;
	[SerializeField] private CustomProgressBar _subProgressBar = null;


	public void Configure(string panelTitleName, float mainMaxValue, float subMaxValue)
	{
		_panelTitle.text = panelTitleName;
		_mainProgressBar.Configure("", 0, mainMaxValue);
		_subProgressBar.Configure("", 0, subMaxValue);
	}

	#region Set Functions
	public void SetMainValue(float newValue, string newTaskName = "")
	{
		_mainProgressBar.SetBarValue(newValue, newTaskName);
	}

	public void SetSubValue(float newValue, string newTaskName = "")
	{
		_subProgressBar.SetBarValue(newValue, newTaskName);
	}

	#endregion Set Functions



	/// <summary>
	/// Invokes any subscribed callbacks and then destroys self
	/// </summary>
	public void CloseWindow()
	{
		CanceledEvent?.Invoke();
		Destroy(gameObject);
	}
}

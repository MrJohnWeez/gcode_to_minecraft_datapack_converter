// Created by MrJohnWeez
// March 2020
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Creates a more feature rich progress bar with a percentage and title
/// </summary>
public class CustomProgressBar : MonoBehaviour
{
	[SerializeField] private TMP_Text _taskName = null;
	[SerializeField] private Slider _progressBar = null;
	[SerializeField] private TMP_Text _percentage = null;

	/// <summary>
	/// Callback for when the progress bar changed values
	/// </summary>
	public void ProgressBarWasChanged()
	{
		_percentage.text = ((_progressBar.value / _progressBar.maxValue * 100).ToString("F1")) + "%";
	}

	/// <summary>
	/// Configure the progress bar
	/// </summary>
	/// <param name="taskName">Middle title name of the progress bar</param>
	/// <param name="startingValue">The stating value of the progress bar</param>
	/// <param name="maxValue">The max value of the progress bar</param>
	public void Configure(string taskName, float startingValue, float maxValue)
	{
		_progressBar.maxValue = maxValue;
		SetBarValue(startingValue, taskName);
	}

	/// <summary>
	/// Set the value of the progress bar and its title
	/// </summary>
	/// <param name="newValue">Value to make the progress bar</param>
	/// <param name="title">Optional title for the progress bar</param>
	public void SetBarValue(float newValue, string title = "")
	{
		newValue = Mathf.Clamp(newValue, _progressBar.minValue, _progressBar.maxValue);
		_progressBar.value = newValue;
		if (!title.IsEmpty())
			_taskName.text = title;
	}
}

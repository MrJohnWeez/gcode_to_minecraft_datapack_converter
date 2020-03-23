// Created by MrJohnWeez
// March 2020
//
using UnityEngine;
using TMPro;

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
	
	/// <summary>
	/// Set up the progress bars and panel
	/// </summary>
	/// <param name="panelTitleName">Progress window name</param>
	/// <param name="mainMaxValue">Max main progress bar value</param>
	/// <param name="subMaxValue">Max sub progress bar value</param>
	public void Configure(string panelTitleName, float mainMaxValue, float subMaxValue)
	{
		_panelTitle.text = panelTitleName;
		_mainProgressBar.Configure("", 0, mainMaxValue);
		_subProgressBar.Configure("", 0, subMaxValue);
	}

	#region Set Functions
	/// <summary>
	/// Set the value of the main progress bar
	/// </summary>
	/// <param name="newValue">Value of progress bar</param>
	/// <param name="newTaskName">Task name of progress bar</param>
	public void SetMainValue(float newValue, string newTaskName = "")
	{
		_mainProgressBar.SetBarValue(newValue, newTaskName);
	}

	/// <summary>
	/// Set the value of the sub progress bar
	/// </summary>
	/// <param name="newValue">Value of progress bar</param>
	/// <param name="newTaskName">Task name of progress bar</param>
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

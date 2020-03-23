// Created by MrJohnWeez
// March 2020
//
using UnityEngine;
using TMPro;

/// <summary>
/// This manager will display parsed data stats to the user
/// </summary>
public class StatsManager : MonoBehaviour
{
	[SerializeField] private GameObject _textPrefab = null;
	[SerializeField] private GameObject _gridParent = null;
	[SerializeField] private TMP_Text _title = null;

	private void Start()
	{
		Clear();
	}

	/// <summary>
	/// Add a stat to the cell display
	/// </summary>
	/// <param name="phraseToDisplay"></param>
	private void AddStat(string phraseToDisplay)
	{
		GameObject textObject = Instantiate(_textPrefab, _gridParent.transform);
		TMP_Text tmpObject = textObject.GetComponentInChildren<TMP_Text>();
		tmpObject.text = phraseToDisplay;
	}

	/// <summary>
	/// Display all stats from the gcode and datapack generation
	/// </summary>
	/// <param name="title">Title of the stats box</param>
	/// <param name="dataStats">ParsedDataStats object</param>
	/// <param name="datapackStats">DatapackStats object</param>
	public void DisplayStats(string title, DataStats dataStats, DatapackStats datapackStats)
	{
		_title.text = title;

		// Order of added stats matter
		AddStat("Estimated print time: " + dataStats.estimatedPrintTime.ToString("F1") + " seconds");
		AddStat("Datapack command count: " + datapackStats.linesOfCode);
		AddStat("Datapack functions count: " + datapackStats.numOfFunctions);
		AddStat("Number of parsed Gcode lines: " + dataStats.totalGcodeLines);
		AddStat("Number of G1 codes: " + dataStats.totalGcodeMoveLines);
		AddStat("Datapack files count: " + datapackStats.numOfFiles);
		AddStat("Datapack directories count: " + datapackStats.numOfDirectories);
	}
	
	/// <summary>
	/// Removes any stats and clears the title of the stat window
	/// </summary>
	public void Clear()
	{
		_title.text = "";

		foreach(Transform child in _gridParent.GetComponentInChildren<Transform>())
		{
			if(child != _gridParent.transform)
				Destroy(child.gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parses gCode files and converts all commands into a list of string lists
/// </summary>
public class GcodeManager
{
	/// <summary>
	/// Seperate each gcode line into a nested string list
	/// </summary>
	public List<List<string>> ParseGcodeFile(string[] unparsedGcode)
	{
		List<List<string>> parsedGcode = new List<List<string>>();
		if (unparsedGcode != null)
		{
			for (int i = 0; i < unparsedGcode.Length; i++)
			{
				List<string> gcodeLineList = ParsePhrase(unparsedGcode[i]);
				if(gcodeLineList != null && gcodeLineList.Count >= 2)
					parsedGcode.Add(gcodeLineList);
			}
		}
		return parsedGcode;
	}

	/// <summary>
	/// Parse the given string. Strips everything but code and returns list of arguments
	/// </summary>
	/// <param name="inPhrase">gcode phrase to parse</param>
	/// <returns>Null if phrase was non gcode, list if it contatins gcode</returns>
	private List<string> ParsePhrase(string inPhrase)
	{
		inPhrase = RemoveNonGcode(inPhrase);
		if(!string.IsNullOrWhiteSpace(inPhrase))
		{
			return ConvertToGcodeList(inPhrase);
		}

		return null;
	}

	/// <summary>
	/// Removes all comments, empty lines from a string
	/// </summary>
	/// <param name="inPhrase">phrase to parse</param>
	/// <returns>String containing only gcode commands</returns>
	private string RemoveNonGcode(string inPhrase)
	{
		int index = inPhrase.IndexOf(";");
		if (index >= 0)
			inPhrase = inPhrase.Remove(index);

		return inPhrase;
	}

	/// <summary>
	/// Convert string to a list of gcode arguments
	/// </summary>
	/// <param name="inPhrase">phrase to parse</param>
	/// <returns>List of seperated gcode command arguments</returns>
	private List<string> ConvertToGcodeList(string inPhrase)
	{
		List<string> parsedLine = new List<string>();
		string[] sections = inPhrase.Split(' ');
		foreach(string arg in sections)
		{
			if(arg.Length >= 2)
			{
				parsedLine.Add(arg[0].ToString());
				parsedLine.Add(arg.Substring(1));
			}
		}

		return parsedLine;
	}
}

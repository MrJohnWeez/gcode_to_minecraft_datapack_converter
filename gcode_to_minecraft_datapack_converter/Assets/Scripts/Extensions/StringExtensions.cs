// Created by MrJohnWeez
// March 2020
//
using UnityEngine;

/// <summary>
/// Some functions to exetend the string class
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Used to trim a string from the front or the back
	/// </summary>
	/// <param name="value">The string to use</param>
	/// <param name="newLength">The new length of the string. Negative value truncates starting at the end</param>
	/// <returns></returns>
	public static string Truncate(this string value, int newLength)
	{
		int positiveLength = Mathf.Abs(newLength);
		if (!string.IsNullOrEmpty(value) && value.Length > positiveLength && newLength != 0)
		{
			if (newLength > 0)
				return value.Substring(0, newLength);
			else
				return value.Substring(value.Length - positiveLength, positiveLength);
		}

		return value;
	}

	/// <summary>
	/// Returns the first 5 and last 5 characters a a string
	/// </summary>
	/// <param name="value">The string to use</param>
	/// <returns></returns>
	public static string FirstLast5(this string value)
	{
		if (value.Length > 10)
		{
			string first = value.Substring(0, 5);
			string last = value.Substring(value.Length - 5, 5);
			return first + last;
		}

		return value;
	}

	/// <summary>
	/// Determines if a string is null, empty, or spaces
	/// </summary>
	/// <param name="value"></param>
	/// <returns>True if string is null, empty, or spaces</returns>
	public static bool IsEmpty(this string value)
	{
		return (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
	}
}

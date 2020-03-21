using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ValidateInput : MonoBehaviour
{
	[SerializeField] private TMP_InputField inputField = null;

    public void ValidateInputField()
	{
		inputField.text = MakeSafeString(inputField.text);
	}

	/// <summary>
	/// Parse given string and return new string that is mcdatapack allowed
	/// </summary>
	/// <param name="name">String to be paresed</param>
	/// <returns></returns>
	private string MakeSafeString(string name)
	{
		name = name.ToLower();
		System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-z0-9_-]");
		return rgx.Replace(name, "");
	}

	public string GetInput()
	{
		return inputField.text;
	}
}

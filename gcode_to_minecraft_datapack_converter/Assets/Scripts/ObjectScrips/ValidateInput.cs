// Created by MrJohnWeez
// March 2020
//
using UnityEngine;
using TMPro;

/// <summary>
/// Validate the user input of an input field
/// </summary>

[RequireComponent(typeof(TMP_InputField))]
public class ValidateInput : MonoBehaviour
{
	TMP_InputField.OnValidateInput ValueChangedEvent;
	private TMP_InputField _inputField = null;

	private void Start()
	{
		_inputField = GetComponent<TMP_InputField>();
		_inputField.onValueChanged.AddListener(ValidateInputField);
	}

	private void OnDestroy()
	{
		_inputField.onValueChanged.RemoveListener(ValidateInputField);
	}

	public void ValidateInputField(string text)
	{
		_inputField.text = MakeSafeString(_inputField.text);
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

	/// <summary>
	/// Gets the current string value of the inputfield
	/// </summary>
	/// <returns></returns>
	public string GetInput()
	{
		return _inputField.text;
	}
}

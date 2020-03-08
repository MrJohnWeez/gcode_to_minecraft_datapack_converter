using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class ProgressAmount<T>
{
	public delegate void ProgressUpdate(T data, string msg);
	public ProgressUpdate ValueChangedEvent;
	private T data;
	private string message = "";

	public void ReportValue(T newValue, string newMessage)
	{
		data = newValue;
		message = newMessage;

		ValueChangedEvent?.Invoke(data, message);
	}
}

// Created by MrJohnWeez
// March 2020
//

/// <summary>
/// Class that lets values be passed from async functions to main unity thread
/// </summary>
/// <typeparam name="T">Type that can be passed to change event</typeparam>
public class ProgressAmount<T>
{
	public delegate void ProgressUpdate(ProgressAmount<T> self);
	public ProgressUpdate ValueChangedEvent;

	public T Data { get; private set; }
	public string Message { get; private set; } = "";
	public string SubMessage { get; private set; } = "";
	public int Id { get; private set; } = 0;

	public ProgressAmount(int idValue)
	{
		Id = idValue;
	}

	public void ReportValue(T newValue, string newMessage, string newSubMessage = "")
	{
		Data = newValue;
		Message = newMessage;
		SubMessage = newSubMessage;
		ValueChangedEvent?.Invoke(this);
	}
}

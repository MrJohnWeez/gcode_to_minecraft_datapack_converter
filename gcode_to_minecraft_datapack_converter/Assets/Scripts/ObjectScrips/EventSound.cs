using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Simple class that allows buttons to have custom sounds
/// </summary>
public class EventSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	[SerializeField] private AudioClip onPointerEnterSound = null;
	[SerializeField] private AudioClip onPointerDownSound = null;
	[SerializeField] private AudioClip notInteractableSound = null;

	private AudioSource _audioSource = null;

	void Start()
	{
		_audioSource = GameObject.FindGameObjectWithTag("UIAudioSource")?.GetComponent<AudioSource>();
		if (!_audioSource)
			Debug.LogError("AudioManager was unable to find an Audio Source within the scene");
	}
	
	public void OnPointerEnter(PointerEventData ped)
	{
		if (onPointerEnterSound && _audioSource)
		{
			_audioSource.PlayOneShot(onPointerEnterSound);
		}
	}

	public void OnPointerDown(PointerEventData ped)
	{
		if (_audioSource)
		{
			Button button = GetComponent<Button>();
			if(button)
			{
				if(button.interactable)
				{
					if(onPointerDownSound)
						_audioSource.PlayOneShot(onPointerDownSound);
				}
				else
				{
					if(notInteractableSound)
						_audioSource.PlayOneShot(notInteractableSound);
				}
			}
			else
			{
				if (onPointerDownSound)
					_audioSource.PlayOneShot(onPointerDownSound);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioSource _uiAudio = null;
	[SerializeField] private AudioSource _musicAudio = null;

    public void ToggleAudioSounds()
	{
		_uiAudio.mute = !_uiAudio.mute;
		_musicAudio.mute = !_musicAudio.mute;
	}
}

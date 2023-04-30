using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
	public AudioSource source;

	public void PlayButtonClickSound()
	{
		source.volume = 0.4f;
		source.Play();
	}
}

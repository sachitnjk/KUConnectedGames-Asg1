using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	AudioSource source;
	[SerializeField] AudioClip ImpulseClip;
	[SerializeField] AudioClip DashClip;
	[SerializeField] AudioClip singleShotClip;
	[SerializeField] AudioClip burstFireClip;
	[SerializeField] AudioClip autoFireClip;

	public enum AudioClipEnum
	{
		Impulse,
		Dash,
		SingleShot
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}

		source = GetComponent<AudioSource>();
	}

	public void PlayAudio(AudioClipEnum clipEnum)
	{
		if(source.isPlaying)
		{
			source.Stop();
		}

		source.clip = GetAudioClip(clipEnum);
		source.Play();
	}

	public void PlayOneShotAudio(AudioClipEnum clipEnum)
	{
		source.PlayOneShot(GetAudioClip(clipEnum));
	}

	public void StopAudio() 
	{
		source.Stop();
	}


	public AudioClip GetAudioClip(AudioClipEnum clipEnum)
	{
		switch (clipEnum) 
		{
			case AudioClipEnum.Impulse:
				source.volume = 0.6f;
				return ImpulseClip;
			case AudioClipEnum.Dash:
				source.volume = 0.6f;
				return DashClip;
			case AudioClipEnum.SingleShot:
				source.volume = 0.2f;
				return singleShotClip;
		}
		return null;
	}
}

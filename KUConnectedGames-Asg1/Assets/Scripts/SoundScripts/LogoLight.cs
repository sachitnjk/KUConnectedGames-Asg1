using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoLight : MonoBehaviour
{
	[SerializeField] Light logoSpotLight;

	[Header("Audio references")]
	[SerializeField] AudioSource source;
	[SerializeField] AudioClip logoLightClip;

	private bool isPlaying = false;

	private void Start()
	{
		logoSpotLight.enabled = true;

		source.clip = logoLightClip;
		source.volume = 0.5f;
	}

	private void Update()
	{
		if (source.isPlaying && !isPlaying)
		{
			isPlaying = true;
			StartCoroutine(FlickerOnAndOffTwice());
		}
	}

	private IEnumerator FlickerOnAndOffTwice()
	{
		yield return new WaitForSeconds(0.2f);
		logoSpotLight.enabled = false;
		yield return new WaitForSeconds(0.1f);
		logoSpotLight.enabled = true;
		yield return new WaitForSeconds(0.3f);
		logoSpotLight.enabled = false;
		yield return new WaitForSeconds(0.2f);
		logoSpotLight.enabled = true;
		yield return new WaitForSeconds(0.1f);
		logoSpotLight.enabled = false;
		yield return new WaitForSeconds(0.2f);
		logoSpotLight.enabled = true;
	}
}

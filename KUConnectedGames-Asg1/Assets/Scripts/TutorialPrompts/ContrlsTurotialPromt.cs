using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContrlsTurotialPromt : MonoBehaviour
{
	public TextMeshProUGUI controlsText;

	private bool hasShownControls = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && !hasShownControls)
		{
			controlsText.gameObject.SetActive(true);
			hasShownControls = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			controlsText.gameObject.SetActive(false);
		}
	}

}

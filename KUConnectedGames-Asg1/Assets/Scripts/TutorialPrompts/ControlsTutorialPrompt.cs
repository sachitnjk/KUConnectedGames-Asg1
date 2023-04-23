using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsTutorialPrompt : MonoBehaviour
{
	public GameObject tutorialPanel;

	[Header("Tutorial box inputs")]
	public float tutorialDisplayTime;
	public float tutorialInitializeTime;

	private void Start()
	{
		Invoke("ActivateTutorialPanel", tutorialInitializeTime);
		Invoke("DeactivateTutorialPanel", tutorialDisplayTime);
	}

	void ActivateTutorialPanel()
	{
		tutorialPanel.SetActive(true);
	}
	void DeactivateTutorialPanel()
	{
		tutorialPanel.SetActive(false);
	}
}

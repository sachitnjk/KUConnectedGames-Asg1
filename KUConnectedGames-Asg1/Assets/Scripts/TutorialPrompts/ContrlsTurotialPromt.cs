using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContrlsTurotialPromt : MonoBehaviour
{
	public GameObject tutorialPanel;
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

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviourPunCallbacks
{
	[SerializeField] private float interactionDistance;
	[SerializeField] private LayerMask interactableLayer;

	private TextMeshProUGUI interactableText;
	private PlayerKeyManager keyManagerScript;

	private void Start()
	{
		interactableText = ReferenceManager.instance.interactionText;
		keyManagerScript = GetComponent<PlayerKeyManager>();

		interactableText.gameObject.SetActive(false);
	}

	private void Update()
	{
		if(keyManagerScript.playerHasKey)
		{
			InteractionAppear();
		}
		else
		{
			interactableText.gameObject.SetActive(false);
		}
	}

	private void InteractionAppear()
	{
		if(photonView.IsMine)
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hitInfo;

			if(Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer)) 
			{
				interactableText.gameObject.SetActive(true);
			}
			else
			{
				interactableText.gameObject.SetActive(false);
			}
		}
	}

}

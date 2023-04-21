using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerInteraction : MonoBehaviourPunCallbacks
{
	[SerializeField] private float interactionDistance;
	[SerializeField] private LayerMask interactableLayer;

	private TextMeshProUGUI interactableText;

	private void Start()
	{
		interactableText = ReferenceManager.instance.interactionText;
	}

	private void Update()
	{
		if(photonView.IsMine)
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hitInfo;

			if(Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer)) 
			{
				if(hitInfo.collider.CompareTag("Interactable"))
				{
					Debug.Log("interactable available");
					interactableText.text = "Press E to Interact";
					interactableText.gameObject.SetActive(true);
				}
			}
			else
			{
				interactableText.gameObject.SetActive(false);
			}
		}
	}

}

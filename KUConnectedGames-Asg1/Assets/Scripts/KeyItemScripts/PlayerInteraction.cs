using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviourPunCallbacks
{
	[SerializeField] private float interactionDistance;
	[SerializeField] private LayerMask interactableLayer;
	
	[SerializeField] private StarterAssetsInputs _input;

	private TextMeshProUGUI interactableText;
	private TextMeshProUGUI gateOpeningText;
	private PlayerKeyManager keyManagerScript;

	private bool canInteract;
	private int gateOpeningCountdown = 10;

	private void Start()
	{
		interactableText = ReferenceManager.instance.interactionText;
		gateOpeningText = ReferenceManager.instance.gateOpeningText;

		keyManagerScript = GetComponent<PlayerKeyManager>();

		interactableText.gameObject.SetActive(false);

		canInteract = false;
	}

	private void Update()
	{
		if (keyManagerScript.playerHasKey)
		{
			canInteract = true;
			InteractionAppear();
		}
		else
		{
			interactableText.gameObject.SetActive(false);
		}

		InteractionAction();
	}

	private void InteractionAppear()
	{
		if (photonView.IsMine)
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer))
			{
				interactableText.gameObject.SetActive(true);
			}
			else
			{
				interactableText.gameObject.SetActive(false);
			}
		}
	}

	private void InteractionAction()
	{
		if (canInteract)
		{
			if (_input.Interaction.WasPerformedThisFrame())
			{
				RaycastHit hitInfo;
				Ray ray = new Ray(transform.position, transform.forward);

				if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer))
				{
					gateOpeningText.gameObject.SetActive(true);
					//StartCoroutine(GateOpeningTextDisable());
					photonView.RPC("GateOpeningTextDisableRPC", RpcTarget.AllBuffered);

					keyManagerScript.playerHasKey = false;
				}
			}
		}
	}

	[PunRPC]
	private void GateOpeningTextDisableRPC()
	{
		gateOpeningText.gameObject.SetActive(true);
		StartCoroutine(GateOpeningCountdown());
	}

	private IEnumerator GateOpeningCountdown()
	{
		for (int i = gateOpeningCountdown; i > 0; i--)
		{
			gateOpeningText.text = "Gate will open in " + i.ToString() + " second(s)";
			yield return new WaitForSeconds(1f);
		}
		gateOpeningText.gameObject.SetActive(false);
		photonView.RPC("LoadNextLevel", RpcTarget.AllBuffered);
	}

	[PunRPC]
	private void LoadNextLevel()
	{
		SceneManager.LoadScene(4);
	}

}

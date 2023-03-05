using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using StarterAssets;

public class MultiplayerSetup : MonoBehaviourPunCallbacks
{

	[SerializeField] List<GameObject> itemsToDelete;

	PhotonView phView;

	private void Awake()
	{
		phView = GetComponent<PhotonView>();

		if (!phView.IsMine)
		{
			gameObject.tag = "Untagged";
		}

	}

	private void Start()
	{
		if(!phView.IsMine)
		{
			PlayerInput playerInput = GetComponent<PlayerInput>();
			ThirdPersonController thirdPersonController = GetComponent<ThirdPersonController>();

			if (thirdPersonController != null)
				DestroyImmediate(thirdPersonController);
			if (playerInput != null)
				Destroy(playerInput);
			for(int i = 0; i < itemsToDelete.Count; i++)
			{
				Destroy(itemsToDelete[i]);
			}
		}
	}
}

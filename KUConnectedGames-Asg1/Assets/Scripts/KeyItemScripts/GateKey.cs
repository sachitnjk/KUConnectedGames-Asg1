using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GateKey : MonoBehaviourPunCallbacks, IPunObservable
{
	private bool keyIsPicked = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerKeyManager playerKeyManager = other.GetComponent<PlayerKeyManager>();

			if(playerKeyManager != null && !keyIsPicked)
			{
				PickUp(playerKeyManager);
			}
		}
	}

	public void PickUp(PlayerKeyManager player)
	{
		keyIsPicked = true;

		photonView.RPC("UpdateKeyState", RpcTarget.All, keyIsPicked);

		player.playerHasKey = true;

		Hashtable props = new Hashtable();
		props.Add("playerHasKey", player.playerHasKey);
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}

	[PunRPC]
	private void UpdateKeyState(bool keyState)
	{
		keyIsPicked = keyState;
		gameObject.SetActive(!keyIsPicked);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting)
		{
			stream.SendNext(keyIsPicked);
		}
		else
		{
			keyIsPicked = (bool)stream.ReceiveNext();

			gameObject.SetActive(!keyIsPicked);
		}
	}
}

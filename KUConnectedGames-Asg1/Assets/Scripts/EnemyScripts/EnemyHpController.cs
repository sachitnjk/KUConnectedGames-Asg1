using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class EnemyHpController : MonoBehaviourPunCallbacks
{
	public int e_MaxHealth = 100;
	public int e_CurrentHealth;

	private int enemy_ID;

	private void Start()
	{
		e_CurrentHealth = e_MaxHealth;
		e_CurrentHealth = (int)Mathf.Clamp(e_CurrentHealth, 0f, e_MaxHealth);
	}

	[PunRPC]
	public void EnemyDamageTake(int damage)
	{
		int ownerID = GetEnemyOwnerID();

		if(!photonView.IsMine && ownerID != PhotonNetwork.LocalPlayer.ActorNumber)
		{
			//Transferring ownership to current player
			photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
		}

		if (!photonView.IsMine)
		{
			// Transfer ownership to current player
			photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
		}
		e_CurrentHealth -= damage;

		if (e_CurrentHealth <= 0)
		{
			if(photonView.IsMine)
			{
				PlayerKillCounter.Instance.KillCounterIncrease();
				//PhotonNetwork.Destroy(this.gameObject);  //Destroy enemy for all over the network
			}
		}
	}

	private void DestroyEnemyObject()
	{
		if(photonView.IsMine)
		{
			Debug.Log("Enemy is dying");
			PhotonNetwork.Destroy(this.gameObject);
		}
	}

	[PunRPC]
	void UpdateEnemyHealth(int newHealth)
	{
		e_CurrentHealth = newHealth;
	}
	public void SetEnemyID(int id)
	{
		enemy_ID = id;											//Setting an owner ID so that who ever kills enemy gets ownership
	}
	public int GetEnemyOwnerID()
	{
		return enemy_ID % 4;
	}
}

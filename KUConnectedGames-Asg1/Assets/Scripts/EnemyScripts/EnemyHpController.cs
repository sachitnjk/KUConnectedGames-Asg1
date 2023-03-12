using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHpController : MonoBehaviourPunCallbacks
{
	public int e_MaxHealth = 100;
	public int e_CurrentHealth;

	private int enemy_ID;

	public EnemyHp enemyHp;
	private void Start()
	{
		e_CurrentHealth = e_MaxHealth;
		enemyHp?.SetMaxHealth(e_MaxHealth);

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
		enemyHp?.SetHealth(e_CurrentHealth);

		if (e_CurrentHealth <= 0)
		{
			PlayerKillCounter.Instance.KillCounterIncrease();

			if(photonView.IsMine)
			{
				PhotonNetwork.Destroy(this.gameObject);  //Destroy enemy for all over the network
			}
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

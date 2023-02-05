using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			PlayerDamageRecieve(20);
			Debug.Log(GameManager.gameManager.playerUnitHealth.Health);
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			PlayerHealRecieve(20);
			Debug.Log(GameManager.gameManager.playerUnitHealth.Health);
		}
	}

	private void PlayerDamageRecieve(int damageTake)
	{
		GameManager.gameManager.playerUnitHealth.DamageUnitHealth(damageTake);
	}
	private void PlayerHealRecieve(int healTake)
	{
		GameManager.gameManager.playerUnitHealth.HealUnitHealth(healTake);
	}

}

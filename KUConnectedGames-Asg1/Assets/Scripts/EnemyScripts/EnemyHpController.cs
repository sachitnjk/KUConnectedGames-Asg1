using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpController : MonoBehaviour
{
	public int e_MaxHealth = 100;
	public int e_CurrentHealth;

	PlayerKillCounter player_KillCounterScript;
	public EnemyHp enemyHp;
	private void Start()
	{
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		player_KillCounterScript = playerGameObject.GetComponent<PlayerKillCounter>();
		player_KillCounterScript.KillCounterReset();

		e_CurrentHealth = e_MaxHealth;
		enemyHp?.SetMaxHealth(e_MaxHealth);

		e_CurrentHealth = (int)Mathf.Clamp(e_CurrentHealth, 0f, e_MaxHealth);
	}

	public void EnemyDamageTake(int damage)
	{
		e_CurrentHealth -= damage;
		enemyHp?.SetHealth(e_CurrentHealth);

		if (e_CurrentHealth <= 0)
		{
			Destroy(this.gameObject);

			player_KillCounterScript.KillCounterIncrease();
		}
	}

}

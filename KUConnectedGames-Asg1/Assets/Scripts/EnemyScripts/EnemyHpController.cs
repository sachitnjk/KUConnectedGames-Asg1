using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpController : MonoBehaviour
{
	public int e_MaxHealth = 100;
	public int e_CurrentHealth;

	public EnemyHp enemyHp;
	private void Start()
	{
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
			PlayerKillCounter.Instance.KillCounterIncrease();

			Destroy(this.gameObject);
		}
	}

}

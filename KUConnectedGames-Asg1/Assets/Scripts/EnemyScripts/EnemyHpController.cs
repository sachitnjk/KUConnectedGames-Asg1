using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpController : MonoBehaviour
{
	//[SerializeField] private GameObject enemyGameObject;

	public int e_MaxHealth = 100;
	public int e_CurrentHealth;

	public EnemyHp enemyHp;

	private void Start()
	{
		e_CurrentHealth = e_MaxHealth;
		enemyHp?.SetMaxHealth(e_MaxHealth);
	}

	public void Update()
	{
		E_Die();
	}

	public void EnemyDamageTake(int damage)
	{
		e_CurrentHealth -= damage;
		enemyHp?.SetHealth(e_CurrentHealth);

		e_CurrentHealth = (int)Mathf.Clamp(e_CurrentHealth, 0f, e_MaxHealth);
	}

	public void E_Die()
	{
		if(e_CurrentHealth <= 0)
		{
			Destroy(this.gameObject);
		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpController : MonoBehaviour
{
	[SerializeField] private GameObject enemyGameObject;

	public int e_MaxHealth = 100;
	public int e_CurrentHealth;

	public EnemyHp enemyHp;

	private void Start()
	{
		e_CurrentHealth = e_MaxHealth;
		enemyHp?.SetMaxHealth(e_MaxHealth);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Bullet")
		{
			EnemyDamageTake(collision.gameObject.GetComponent<BulletController>().damage);

			Destroy(collision.gameObject);

			if (e_CurrentHealth < 0)
			{
				E_Die();
			}
		}
	}

	public void EnemyDamageTake(int damage)
	{
		e_CurrentHealth -= damage;
		enemyHp?.SetHealth(e_CurrentHealth);
	}

	public void E_Die()
	{
		Destroy(enemyGameObject);
	}

}

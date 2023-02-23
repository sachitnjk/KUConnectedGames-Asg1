using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	public float speed = 20f;
	public float lifeTime = 2f;
	public int damage = 5;

	private float timer;

	void Update()
	{
		transform.position += transform.forward * speed * Time.deltaTime;

		// Destroy the bullet after a certain amount of time
		timer += Time.deltaTime;
		if (timer >= lifeTime)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// Check if the bullet hit an enemy
		EnemyHpController e_HealthPoints = other.GetComponent<EnemyHpController>();
		if (e_HealthPoints != null)
		{
			e_HealthPoints.EnemyDamageTake(damage);
			Destroy(gameObject);
		}
	}
}

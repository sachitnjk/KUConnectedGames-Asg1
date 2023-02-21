using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehaviour : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float speed = 3.0f;
	[SerializeField] private float detectionRange = 10.0f;
	[SerializeField] private float attackRange = 2.0f;

	[SerializeField] private int g_EnemyDamage;

	private enum State
	{
		Idle,
		Patrol,
		Chase,
		Attack
	}
	private State currentState;
	private int currentWaypoint;

	PlayerHealthBar healthBar;

	public Transform[] waypoints;

	void Start()
	{
		currentState = State.Idle;
		currentWaypoint = 0;
	}

	void Update()
	{
		float distanceToTarget = Vector3.Distance(transform.position, target.position);

		switch (currentState)
		{
			case State.Idle:
				if (distanceToTarget < detectionRange)
				{
					currentState = State.Patrol;
				}
				break;

			case State.Patrol:
				transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

				if (transform.position == waypoints[currentWaypoint].position)
				{
					currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
				}

				if (distanceToTarget < detectionRange)
				{
					currentState = State.Chase;
				}
				break;

			case State.Chase:
				transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

				if (distanceToTarget < attackRange)
				{
					currentState = State.Attack;
				}
				break;

			case State.Attack:
				healthBar.TakeDamage(g_EnemyDamage);
				currentState = State.Chase;
				break;
		}
	}
}

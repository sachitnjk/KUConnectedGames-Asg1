using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AiBehaviour : MonoBehaviour
{
	[SerializeField] Transform enemy_Target;
	[SerializeField] LayerMask playerMask;
	[SerializeField] LayerMask obstacleMask;

	[SerializeField] private float enemy_SpeedWalk;
	[SerializeField] private float enemy_SpeedRun;

	[SerializeField] private float enemy_DetactionRange;
	[SerializeField] private float enemy_StartWaitTime;
	[SerializeField] private float enemy_TimeToRotate;

	float waitTime;
	float timeToRotate;

	[SerializeField] private float enemy_AttackRange;
	[SerializeField] private int enemy_Damage;
	[SerializeField] private float enemy_ViewRadius = 5f;
	[SerializeField] private float enemy_GapBetweenDamage;

	[SerializeField] private Animator _animator;

	public NavMeshAgent navMeshAgent;

	private State enemy_CurrentState;

	private GameObject player;
	private EnemyHpController enemyHpController;

	private Transform[] waypoints;
	private	int enemy_CurrentWaypointIndex;

	private Vector3 player_LastKnownPos;

	private bool enemy_CanDamage;

	private enum State
	{
		Patrol,
		Chase,
		Searching,
		Attack,
		Dead
	}

	private void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if(players.Length > 0)
		{
			player = players[0];
		}

		enemy_CanDamage = true;

		waypoints = ReferenceManager.instance.enemyWaypoints.waypoints;

		enemy_CurrentWaypointIndex = 0;
		navMeshAgent = GetComponent<NavMeshAgent>();

		navMeshAgent.isStopped = false;
		navMeshAgent.speed = enemy_SpeedWalk;
		navMeshAgent.SetDestination(waypoints[enemy_CurrentWaypointIndex].position);


		enemy_CurrentState = State.Patrol;
	}

	private void Update()
	{
		switch (enemy_CurrentState)
			{
			case State.Patrol:
				Patrol();
				break;
			case State.Chase:
				Chasing();
				break;
			case State.Searching:
				Searching();
				break;
			case State.Attack:
				EnemyAttack(enemy_Damage);
				break;
			case State.Dead:
				EnemyDead();
				break;
		}

	}

	public void NextPoint()
	{
		enemy_CurrentWaypointIndex = (enemy_CurrentWaypointIndex + 1) % waypoints.Length;
		navMeshAgent.SetDestination(waypoints[enemy_CurrentWaypointIndex].position);
	}

	void Move(float speed)
	{
		navMeshAgent.isStopped = false;
		navMeshAgent.speed = speed;
	}

	void Stop()
	{
		navMeshAgent.isStopped = true;
		navMeshAgent.speed = 0;
	}

	private void Patrol()
	{
		_animator.SetBool("isAttacking", false);
		_animator.SetBool("isRunning", false);
		_animator.SetBool("isWalking", true);
		navMeshAgent.SetDestination(waypoints[enemy_CurrentWaypointIndex].position);

		//AvoidEnemy();

		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			if (waitTime <= 0)
			{
				NextPoint();
				Move(enemy_SpeedWalk);
				waitTime = enemy_StartWaitTime;
			}
			else
			{
				Stop();
				waitTime -= Time.deltaTime;
			}
		}

		if(DetectEntity())
		{
			Debug.Log("changinmg to chase from patrol");

			enemy_CurrentState = State.Chase;
		}
	}

	private bool DetectEntity()
	{
		Collider[] entityInRange = Physics.OverlapSphere(transform.position, enemy_ViewRadius, playerMask);

		if (entityInRange != null && entityInRange.Length > 0)
		{
			foreach (Collider entity in entityInRange)
			{
				Vector3 targetPoint = entity.transform.position;
				targetPoint.y += 1;
				Vector3 direction = targetPoint - transform.position;
				float distance = direction.magnitude;
				direction.Normalize();

				RaycastHit hitInfo;

				if (!Physics.Raycast(transform.position, direction, out hitInfo, distance, obstacleMask))
				{
					Debug.Log("Detecting player-raycast");

					enemy_Target = entity.transform;
					return true;
				}
				else
				{
					Debug.Log(hitInfo.collider.gameObject.name);
				}
			}
		}
			return false;
	}

	//private void AvoidEnemy()
	//{
	//	RaycastHit hit;
	//	if(Physics.Raycast(transform.position, transform.forward, out hit, 4))
	//	{
	//		if(hit.collider.CompareTag("Enemy"))
	//		{
	//			transform.Rotate(0, 180, 0);
	//			Vector3 avoidPos = transform.position + (transform.forward * 2);
	//			transform.position = Vector3.Lerp(transform.position, avoidPos, Time.deltaTime * 2);
	//		}
	//	}
	//}

	private void Chasing()
	{
		_animator.SetBool("isWalking", false);
		_animator.SetBool("isAttacking", false);
		_animator.SetBool("isRunning", true);

		Vector3 targetPosition = enemy_Target.position;
		var towardsPlayer = enemy_Target.position - transform.position;

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(towardsPlayer), Time.deltaTime * enemy_TimeToRotate);

		Move(enemy_SpeedRun);
		navMeshAgent.SetDestination(targetPosition);
		if (Vector3.Distance(transform.position, targetPosition) <= enemy_AttackRange)
		{
			//enemy_CanDamage = true;
			enemy_CurrentState = State.Attack;
		}
		else if (Vector3.Distance(transform.position, targetPosition) > enemy_DetactionRange)
		{
			player_LastKnownPos = enemy_Target.position;
			player_LastKnownPos.y = transform.position.y;  // adjusting target position so that it is close to enemy pos when enmey on it

			enemy_CurrentState = State.Searching;
		}
	}

	private void Searching()
	{
		_animator.SetBool("isRunning", false);
		_animator.SetBool("isWalking", true);

		Debug.Log("going to player last pos");
		navMeshAgent.SetDestination(player_LastKnownPos);

		float distanceToLastKnownPos = Vector3.Distance(transform.position, player_LastKnownPos);

		if (distanceToLastKnownPos <= navMeshAgent.stoppingDistance)
		{
			if (DetectEntity())
			{
				Debug.Log("Searching -> chase");
				enemy_CurrentState = State.Chase;
			}
			else
			{
				Debug.Log("Searching -> patrol");
				enemy_CurrentState = State.Patrol;
			}
		}
	}

	void EnemyAttack(int damage)
	{
		_animator.SetBool("isWalking", false);
		_animator.SetBool("isRunning", false);
		_animator.SetBool("isAttacking", true);

		if (enemy_CanDamage)
		{
			Debug.Log("I am attacking");
			player.GetComponent<PlayerHealthBar>().TakeDamage(damage);
			Stop();
			var towardsPlayer = enemy_Target.position - transform.position;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(towardsPlayer), Time.deltaTime * enemy_TimeToRotate);
			StartCoroutine(AiAttacked());
		}
	}

	private IEnumerator AiAttacked()
	{
		enemy_CanDamage = false;
		yield return new WaitForSeconds(enemy_GapBetweenDamage);
		enemy_CanDamage = true;

		if (Vector3.Distance(transform.position, enemy_Target.position) > enemy_AttackRange)
		{
			Debug.Log("attack -> chase");
			enemy_CurrentState = State.Chase;
		}
	}

	private void EnemyDead()
	{
		//trigger dead animation here
		if(enemyHpController.e_CurrentHealth <= 0)
		{
			enemy_CurrentState = State.Dead;
			Debug.Log("EnemyDead");
		}
	}

}

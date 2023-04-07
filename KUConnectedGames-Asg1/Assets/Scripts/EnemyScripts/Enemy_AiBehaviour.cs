using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy_AiBehaviour : MonoBehaviourPunCallbacks
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
	[SerializeField] private float enemy_ViewRadius;
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
		IsHit,
		Attack,
		Dead
	}

	private State previousState;

	private void Start()
	{
		enemyHpController = GetComponent<EnemyHpController>();

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

		previousState = enemy_CurrentState;
	}

	private void Update()
	{
		if(enemy_CurrentState != State.Dead && enemyHpController.e_CurrentHealth == 0)
		{
			EnemyDead();
			enemy_CurrentState = State.Dead;	
		}

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
			case State.IsHit:
				IsHit();
				break;
			case State.Attack:
				EnemyAttack(enemy_Damage);
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

	public void Stop()
	{
		navMeshAgent.isStopped = true;
		navMeshAgent.speed = 0;
	}

	[PunRPC]
	public void TriggerHitAnimation()
	{
		if(enemyHpController.e_CurrentHealth > 0)
		{
			previousState = enemy_CurrentState;
			enemy_CurrentState = State.IsHit;
			_animator.SetTrigger("isHit");
			navMeshAgent.isStopped = true;
		}
	}

	[PunRPC]
	public void EndHitAnimation()
	{
		navMeshAgent.isStopped=false;
		photonView.RPC("SetPreviousState", RpcTarget.All);
	}

	[PunRPC]
	public void SetPreviousState()
	{
		enemy_CurrentState = previousState;
	}
	private void IsHit()
	{
		navMeshAgent.isStopped = true;
		enemy_CurrentState = previousState;

	}

	private void Patrol()
	{
		_animator.SetBool("isAttacking", false);
		_animator.SetBool("isRunning", false);
		_animator.SetBool("isWalking", true);
		navMeshAgent.SetDestination(waypoints[enemy_CurrentWaypointIndex].position);


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

	private void Chasing()
	{
		_animator.SetBool("isWalking", false);
		_animator.SetBool("isAttacking", false);
		_animator.SetBool("isRunning", true);

		Vector3 targetPosition = enemy_Target.position;
		var towardsPlayer = targetPosition - transform.position;

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(towardsPlayer), Time.deltaTime * enemy_TimeToRotate);

		Move(enemy_SpeedRun);
		navMeshAgent.SetDestination(targetPosition);
		if (Vector3.Distance(transform.position, targetPosition) <= enemy_AttackRange)
		{
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
				enemy_CurrentState = State.Chase;
			}
			else
			{
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
		Stop();
		_animator.SetBool("isDead", true);
		_animator.SetTrigger("isDeadTrigger");
		Collider enemy_Collider = GetComponent<Collider>();
		enemy_Collider.enabled = false;
	}

}

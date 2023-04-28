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

	[SerializeField] private float enemy_AttackRange;
	[SerializeField] private int enemy_Damage;
	[SerializeField] private float enemy_ViewRadius;
	[SerializeField] private float enemy_GapBetweenDamage;

	[Header("Enemy Audio")]
	[SerializeField] private AudioSource enemy_PatrolSound;
	[SerializeField] private AudioSource enemy_ChaseSound;
	[SerializeField] private AudioSource enemy_AttackSound;

	[SerializeField] private Animator _animator;

	public NavMeshAgent navMeshAgent;

	private State enemy_CurrentState;
	private State previousState;

	private GameObject player;
	private EnemyHpController enemyHpController;
	private Rigidbody enemyRigidBody;

	private Transform[] waypoints;
	private	int enemy_CurrentWaypointIndex;

	private Vector3 player_LastKnownPos;
	private Vector3 previousDestination;

	private bool enemy_CanDamage;

	public enum State
	{
		Patrol,
		Chase,
		Searching,
		IsHit,
		Attack,
		Dead
	}


	private void Start()
	{
		enemyHpController = GetComponent<EnemyHpController>();
		enemyRigidBody = GetComponent<Rigidbody>();

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
				break;
			case State.Attack:
				EnemyAttack(enemy_Damage);
				break;
		}
	}

	public State GetCurrentState()
	{
		return enemy_CurrentState;
	}

	public Vector3 TargetPosition()
	{
		return enemy_Target.position;
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
			previousDestination = navMeshAgent.destination;
			navMeshAgent.SetDestination(transform.position);
			enemy_CurrentState = State.IsHit;
			_animator.SetTrigger("isHit");
		}
	}

	//Calling this as animation event
	private void ReturnFromHit()
	{
		enemy_CurrentState = previousState;
		navMeshAgent.SetDestination(previousDestination);
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
					enemy_Target = entity.transform;
					return true;
				}
			}
		}
			return false;
	}

	private void Chasing()
	{
		navMeshAgent.isStopped = false;
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
			player_LastKnownPos.y = transform.position.y;

			enemy_CurrentState = State.Searching;
		}
	}

	private void Searching()
	{
		_animator.SetBool("isRunning", false);
		_animator.SetBool("isWalking", true);

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

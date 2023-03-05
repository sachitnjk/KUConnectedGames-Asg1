using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviourPunCallbacks
{
	public NavMeshAgent navMeshAgent;
	public float startWaitTime = 4;
	public float timeToRotate = 2;
	public float speedWalk = 6;
	public float speedRun = 9;

	public float viewRadius = 15;
	public float viewAngle = 90;
	public LayerMask playerMask;
	public LayerMask obstacleMask;
	public float meshResolution = 1.0f;
	public int edgeIterations = 4;
	public float edgeDistance = 0.5f;

	public EnemyWaypointsScript waypointsScript;
	private Transform[] waypoints;
	int m_CurrentWaypointIndex;

	Vector3 playerLastPosition = Vector3.zero;
	Vector3 m_PlayerPosition;

	float m_WaitTime;
	float m_TimeToRotate;
	bool m_playerInRange;
	bool m_PlayerNear;
	bool m_IsPatrol;
	bool m_CaughtPlayer;
	bool ai_CanDamage;

	private PlayerHealthBar playerHealthBar;
	[SerializeField] private int damageDealt;
	[SerializeField] float gapBetweenDamage = 1f;

	private Transform playerTransform;

	[SerializeField] PhotonView phView;

	public void Start()
	{
		//PhotonView phView = GetComponent<PhotonView>();

		ai_CanDamage = true;

		m_PlayerPosition = Vector3.zero;
		m_IsPatrol = true;
		m_CaughtPlayer = false;
		m_playerInRange = false;
		m_PlayerNear = false;
		m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
		m_TimeToRotate = timeToRotate;

		waypoints = waypointsScript.waypoints;

		m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
		navMeshAgent = GetComponent<NavMeshAgent>();

		navMeshAgent.isStopped = false;
		navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
		navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the destination to the first waypoint

	}

	private void Update()
	{

		if (phView.IsMine)
		{
		//	Vector3 newPosition = CalculateNewPosition();
		//	SetPosition(newPosition);

			EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

			if (!m_IsPatrol)
			{
				Chasing();
			}
			else
			{
				Patroling();
			}
		}


		//EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

		//if (!m_IsPatrol)
		//{
		//	Chasing();
		//}
		//else
		//{
		//	Patroling();
		//}
	}

	//[Photon.Pun.PunRPC]
	//void SetPosition(Vector3 newPosition)
	//{
	//	transform.position = newPosition;
	//}

	private Vector3 CalculateNewPosition()
	{
		if (m_IsPatrol)
		{
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				NextPoint();
			}
			return waypoints[m_CurrentWaypointIndex].position;
		}
		else
		{
			return m_PlayerPosition;
		}
	}

	public void EnemyAttack(int damage)
	{
		if (ai_CanDamage)
		{
			playerHealthBar.TakeDamage(damage);
			StartCoroutine(AiAttacked());
		}

	}

	private IEnumerator AiAttacked()
	{
		ai_CanDamage = false;
		yield return new WaitForSeconds(gapBetweenDamage);
		ai_CanDamage = true;
	}

	private void Chasing()
	{
		//  The enemy is chasing the player
		m_PlayerNear = false;
		playerLastPosition = Vector3.zero;

		if (!m_CaughtPlayer)
		{
			Move(speedRun);
			navMeshAgent.SetDestination(m_PlayerPosition);
		}
		if (Vector3.Distance(transform.position, playerTransform.position) <= 2f)
		{
			EnemyAttack(damageDealt);
		}
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, playerTransform.position) >= 6f)
			{
				m_IsPatrol = true;
				m_PlayerNear = false;
				Move(speedWalk);
				m_TimeToRotate = timeToRotate;
				m_WaitTime = startWaitTime;
				navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
			}
			else
			{
				m_WaitTime -= Time.deltaTime;
			}
		}
	}

	private void Patroling()
	{
		if (m_PlayerNear)
		{
			if (m_TimeToRotate <= 0)
			{
				Move(speedWalk);
				LookingPlayer(playerLastPosition);
			}
			else
			{
				Stop();
				m_TimeToRotate -= Time.deltaTime;
			}
		}
		else
		{
			m_PlayerNear = false;
			playerLastPosition = Vector3.zero;
			navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				if (m_WaitTime <= 0)
				{
					NextPoint();
					Move(speedWalk);
					m_WaitTime = startWaitTime;
				}
				else
				{
					Stop();
					m_WaitTime -= Time.deltaTime;
				}
			}
		}
	}

	//private void OnAnimatorMove()
	//{

	//}

	public void NextPoint()
	{
		m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
		navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
	}

	void Stop()
	{
		navMeshAgent.isStopped = true;
		navMeshAgent.speed = 0;
	}

	void Move(float speed)
	{
		//Debug.Log("Move being called");
		navMeshAgent.isStopped = false;
		navMeshAgent.speed = speed;
	}
	void CaughtPlayer()
	{
		m_CaughtPlayer = true;
	}

	void LookingPlayer(Vector3 player)
	{
		navMeshAgent.SetDestination(player);
		if (Vector3.Distance(transform.position, player) <= 0.3)
		{
			if (m_WaitTime <= 0)
			{
				m_PlayerNear = false;
				Move(speedWalk);
				navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
				m_WaitTime = startWaitTime;
				m_TimeToRotate = timeToRotate;
			}
			else
			{
				Stop();
				m_WaitTime -= Time.deltaTime;
			}
		}
	}

	void EnviromentView()
	{
		Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

		for (int i = 0; i < playerInRange.Length; i++)
		{
			Transform player = playerInRange[i].transform;
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
			{
				float dstToPlayer = Vector3.Distance(transform.position, player.position);
				if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
				{
					m_playerInRange = true;
					m_IsPatrol = false;
				}
				else
				{
					m_playerInRange = false;
				}
			}
			if (Vector3.Distance(transform.position, player.position) > viewRadius)
			{
				m_playerInRange = false;
			}
			if (m_playerInRange)
			{
				playerTransform = player.transform;

				m_PlayerPosition = player.transform.position;
				playerHealthBar = player.GetComponent<PlayerHealthBar>();
			}
		}
	}
}

using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

	public NavMeshAgent navMeshAgent;
	public float startWaitTime = 4f;
	public float timeToRotate = 2f;
	public float speedWalk = 6f;
	public float speedRun = 9f;

	public float viewRadius = 15;
	public float viewAngle = 90;
	public LayerMask playerMask;
	public LayerMask obstacleMask;
	public float meshResolution = 1f;
	public int edgeIterations = 4;
	public float edgeDistance = 0.5f;

	public Transform[] waypoints;
	int m_CurrentWaypointIndex;

	Vector3 playerLastPosition = Vector3.zero;
	Vector3 m_PlayerPosition;

	float m_WaitTime;
	float m_TimeToRotate;
	bool m_PlayerInRange;
	bool m_PlayerNear;
	bool m_IsPatrol;
	bool m_CaughtPlayer;

	void Start()
	{

		m_PlayerPosition = Vector3.zero;
		m_IsPatrol = true;
		m_CaughtPlayer = false;
		m_PlayerInRange = false;
		m_WaitTime = startWaitTime;
		m_TimeToRotate = timeToRotate;

		m_CurrentWaypointIndex = 0;
		navMeshAgent = GetComponent<NavMeshAgent>();

		navMeshAgent.isStopped = false;
		navMeshAgent.speed = speedWalk;
		navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

	}

	void Update()
	{

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

	void NextPoint()
	{
		m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
		navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
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
				float distToPlayer = Vector3.Distance(transform.position, player.position);
				if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
				{
					m_PlayerInRange = true;
					m_IsPatrol = false;
				}
				else
				{
					m_PlayerInRange = false;
				}
			}
			if (Vector3.Distance(transform.forward, player.position) > viewRadius)
			{
				m_PlayerInRange = false;
			}
			if (m_PlayerInRange)
			{
				m_PlayerPosition = player.transform.position;
			}
		}

	}
}


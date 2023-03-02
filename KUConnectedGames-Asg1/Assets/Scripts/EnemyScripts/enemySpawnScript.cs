using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class enemySpawnScript : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private Transform[] enemySpawnPoints;
	public EnemyWaypointsScript waypointsScript;
	[SerializeField] PhotonView phView;

	private void Start()
	{
		if(phView.IsMine)
		{
			int randomNumber = Random.Range(0, enemySpawnPoints.Length);
			Transform enemySpawnPoint = enemySpawnPoints[randomNumber];
			GameObject e_Instance = PhotonNetwork.Instantiate(enemyPrefab.name, enemySpawnPoint.position, Quaternion.identity);
			NavMeshAgent e_NavMeshAgent = e_Instance.GetComponent<NavMeshAgent>();
			e_NavMeshAgent.SetDestination(waypointsScript.waypoints[0].position);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class enemySpawnScript : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private Transform[] enemySpawnPoints;
	[SerializeField] PhotonView phView;
	[SerializeField] int enemyNumberToSpawn;

	private void Start()
	{
		for(int i = 0; i < enemyNumberToSpawn; i++)
		{
			if(phView.IsMine)
			{
				int randomNumber = Random.Range(0, enemySpawnPoints.Length);
				Transform enemySpawnPoint = enemySpawnPoints[randomNumber];
				GameObject e_Instance = PhotonNetwork.Instantiate(enemyPrefab.name, enemySpawnPoint.position, Quaternion.identity);

				e_Instance.GetComponent<EnemyHpController>().SetEnemyID(phView.ViewID);     //Setting view ID for spawned enemy for ownership transfer

				NavMeshAgent e_NavMeshAgent = e_Instance.GetComponent<NavMeshAgent>();
				e_NavMeshAgent.SetDestination(ReferenceManager.instance.enemyWaypoints.waypoints[0].position);
			}
		}
	}
}

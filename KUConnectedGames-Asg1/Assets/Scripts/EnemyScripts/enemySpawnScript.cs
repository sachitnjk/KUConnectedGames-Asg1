using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class enemySpawnScript : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private Transform[] enemySpawnPoints;

	private void Start()
	{
		int randomNumber = Random.Range(0, enemySpawnPoints.Length);
		Transform enemySpawnPoint = enemySpawnPoints[randomNumber];
		PhotonNetwork.Instantiate(enemyPrefab.name, enemySpawnPoint.position, Quaternion.identity);
	}
}

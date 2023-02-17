using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class enemySpawnScript : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject[] enemyPrefab;
	[SerializeField] private Transform[] enemySpawnPoints;

	private void Start()
	{
		int randomNumber = Random.Range(0, enemySpawnPoints.Length);
		Transform enemySpawnPoint = enemySpawnPoints[randomNumber];
		GameObject enemyToSpawn = enemyPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["enemyAvatar"]];
		PhotonNetwork.Instantiate(enemyToSpawn.name, enemySpawnPoint.position, Quaternion.identity);
	}
}

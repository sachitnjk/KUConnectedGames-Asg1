using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
	public GameObject[] playerPrefabs;
	public Transform[] spawnPoints;

	private void Start()
	{
		int randomNumber = Random.Range(0, spawnPoints.Length);
		Transform spawnPoint = spawnPoints[randomNumber];
		GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
		PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
	}

	//[SerializeField] private GameObject playerPrefab;

	//[SerializeField] private float minX;
	//[SerializeField] private float maxX;
	//[SerializeField] private float minY;
	//[SerializeField] private float maxY;

	//private void Start()
	//{

	//	Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
	//	PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

	//}

}

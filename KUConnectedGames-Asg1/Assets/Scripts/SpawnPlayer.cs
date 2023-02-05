using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{

	[SerializeField] private GameObject playerPrefab;

	[SerializeField] private float minX;
	[SerializeField] private float maxX;
	[SerializeField] private float minY;
	[SerializeField] private float maxY;

	private void Start()
	{
		
		Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
		PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

	}

}

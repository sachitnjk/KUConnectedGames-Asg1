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
		GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

		KillCounterScript killCounterScript = spawnedPlayer.GetComponentInChildren<KillCounterScript>();

		PlayerKillCounter.Instance.SetKillCounterSlider(killCounterScript); // setting kill counter script on spawned player as reference in playerkIllCounter script
	}

}

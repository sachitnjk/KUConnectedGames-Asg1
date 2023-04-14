using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
	[SerializeField] private TextMeshProUGUI player1Name;
	[SerializeField] private TextMeshProUGUI player2Name;
	[SerializeField] private TextMeshProUGUI player3Name;

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdatePlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdatePlayerList();
	}

	private void UpdatePlayerList()
	{
		Player[] players = PhotonNetwork.PlayerList;

		for(int i = 0; i < players.Length; i++)
		{
			switch(i)
			{
				case 0:
					player1Name.text = players[i].NickName;
					break;
				case 1:
					player1Name.text = players[i].NickName;
					break;
				case 2:
					player1Name.text = players[i].NickName;
					break;
			}
		}
	}
}

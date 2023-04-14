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

	private void Start()
	{
		UpdatePlayerList();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdatePlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdatePlayerList();
		ClearPlayerName(otherPlayer);
	}

	private void UpdatePlayerList()
	{
		Player[] players = PhotonNetwork.PlayerList;

		for(int i = 0; i < players.Length; i++)
		{

			if (players[i].ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
			{
				continue;
			}

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

	private void ClearPlayerName(Player player)
	{
		if (player1Name.text.Equals(player.NickName))
		{
			player1Name.text = "";
		}
		else if (player2Name.text.Equals(player.NickName))
		{
			player2Name.text = "";
		}
		else if (player3Name.text.Equals(player.NickName))
		{
			player3Name.text = "";
		}
	}
}

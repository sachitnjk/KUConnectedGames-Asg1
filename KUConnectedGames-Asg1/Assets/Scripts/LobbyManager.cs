using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private InputField roomInputField;
	[SerializeField] private GameObject createRoomPanel;
	[SerializeField] private GameObject roomListPanel;
	[SerializeField] private Text currentRoomName;

	private void Start()
	{
		PhotonNetwork.JoinLobby();
	}

	public void OnClickCreate()
	{
		if(roomInputField.text.Length >= 1)
		{
			PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4 });
		}
	}

	public override void OnJoinedRoom()
	{
		createRoomPanel.SetActive(false);
		roomListPanel.SetActive(true);
		currentRoomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
	}

}

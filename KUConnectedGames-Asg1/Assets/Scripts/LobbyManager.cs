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

	public RoomItemScript roomItem;
	List<RoomItemScript> roomItemList = new List<RoomItemScript>();
	public Transform contentObject;

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

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		UpdateRoomList(roomList);
	}

	public void UpdateRoomList(List<RoomInfo> list)
	{
		foreach(RoomItemScript item in roomItemList)
		{
			Destroy(item.gameObject);
		}
		roomItemList.Clear();

		foreach(RoomInfo room in list)
		{
			RoomItemScript newRoom = Instantiate(roomItem, contentObject);
			newRoom.SetRoomName(room.Name);
			roomItemList.Add(newRoom);
		}
	}

	public void JoinRoom(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}

	public void OnClickLeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		roomListPanel.SetActive(false);
		createRoomPanel.SetActive(true);
	}
}

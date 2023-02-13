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
	[SerializeField] public Text currentRoomName;

	[SerializeField] private float timeBetweenUpdates = 1.5f;
	float nextUpdateTime;

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
		if(Time.time > nextUpdateTime)
		{
			UpdateRoomList(roomList);
			nextUpdateTime = Time.time + timeBetweenUpdates;
		}
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

			if (room.PlayerCount == 0)
			{
				PhotonNetwork.Destroy(newRoom.gameObject);
				roomItemList.Remove(newRoom);
			}
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
		createRoomPanel.SetActive(true);
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();           //to join back photon lobby after leaving room
	}
}

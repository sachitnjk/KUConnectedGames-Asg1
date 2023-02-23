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

	public List<PlayerItem> playerItemsList = new List<PlayerItem>();
	public PlayerItem playerItemPrefab;
	public Transform playerItemParent;

	public GameObject playButton;

	private void Start()
	{
		PhotonNetwork.JoinLobby();
	}

	public void OnClickCreate()
	{
		if(roomInputField.text.Length >= 1)
		{
			PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true});
		}
	}

	public override void OnJoinedRoom()
	{
		createRoomPanel.SetActive(false);
		roomListPanel.SetActive(true);
		currentRoomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
		UpdatePlayerList();
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
			if (room.RemovedFromList)
			{
				return;
			}
			roomListPanel.SetActive(true);
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
		currentRoomName.text = "Current room name: ";
	}

	public override void OnLeftRoom()
	{
		createRoomPanel.SetActive(true);
		roomListPanel.SetActive(false);
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();           //to join back photon lobby after leaving room
	}

	void UpdatePlayerList()
	{
		foreach(PlayerItem item in playerItemsList)
		{
			Destroy(item.gameObject);
		}
		playerItemsList.Clear();

		if(PhotonNetwork.CurrentRoom == null)
		{
			return;
		}

		foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
		{
			PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
			newPlayerItem.SetPlayerInfo(player.Value);
			if(player.Value == PhotonNetwork.LocalPlayer)
			{
				newPlayerItem.ApplyLocalChanges();
			}
			playerItemsList.Add(newPlayerItem);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdatePlayerList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdatePlayerList();
	}

	private void Update()
	{
		if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
		{
			playButton.SetActive(true);
		}
		else
		{
			playButton.SetActive(false);
		}
	}

	public void OnClickPlayButton()
	{
		PhotonNetwork.LoadLevel("MainScene");
	}
}

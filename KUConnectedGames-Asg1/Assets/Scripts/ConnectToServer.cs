using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

	public InputField usernameInput;
	public Text buttonText;

	public void OnClickConnect()
	{
		if(usernameInput.text.Length >= 1)
		{
			PhotonNetwork.NickName = usernameInput.text;
			buttonText.text = "Connecting...";
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		SceneManager.LoadScene("LobbyScene");
	}

	//void Start()
	//{

	//	PhotonNetwork.ConnectUsingSettings();

	//}


	////any call inside this function is called when successfully connected to the server
	//public override void OnConnectedToMaster()
	//{
	//	PhotonNetwork.JoinLobby();
	//}


	//public override void OnJoinedLobby()
	//{
	//	SceneManager.LoadScene("LobbyScene");


	//}

}

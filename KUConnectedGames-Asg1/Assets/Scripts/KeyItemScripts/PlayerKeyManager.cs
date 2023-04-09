using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerKeyManager : MonoBehaviourPunCallbacks
{
	public bool playerHasKey = false;


	public void UpdateHasKey(bool hasKey)
	{
		playerHasKey = hasKey;
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if(targetPlayer == PhotonNetwork.LocalPlayer)
		{
			if(changedProps.ContainsKey("playerHasKey"))
			{
				bool hasKey = (bool)changedProps["playerHasKey"];
				UpdateHasKey(hasKey);
			}
		}
	}
}

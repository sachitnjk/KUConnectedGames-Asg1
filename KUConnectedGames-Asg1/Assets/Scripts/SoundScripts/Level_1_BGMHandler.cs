using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_BGMHandler : MonoBehaviour
{
	private void Awake()
	{
		BGMHandler.instance.bgMenusLobby.Stop();
	}
}

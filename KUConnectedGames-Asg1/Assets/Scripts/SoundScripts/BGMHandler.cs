using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMHandler : MonoBehaviour
{
	public static BGMHandler instance;

	//int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
	[SerializeField] public AudioSource bgMenusLobby; 
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}

		DontDestroyOnLoad(transform.gameObject);
	}
}

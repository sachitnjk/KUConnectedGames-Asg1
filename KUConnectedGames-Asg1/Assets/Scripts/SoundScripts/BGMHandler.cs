
using UnityEngine;

public class BGMHandler : MonoBehaviour
{
	public static BGMHandler instance;

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

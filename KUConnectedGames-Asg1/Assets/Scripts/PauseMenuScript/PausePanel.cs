using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PausePanel : MonoBehaviourPunCallbacks
{
	PlayerInput _input;
	ThirdPersonController player_ThirdPersonController;

	[SerializeField] GameObject pausePanel;
	private bool isPaused;

	private void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players)
		{
			_input = player.GetComponent<PlayerInput>();
			if(_input != null)
			{
				player_ThirdPersonController = player.GetComponent<ThirdPersonController>();
				break;
			}
		}

		player_ThirdPersonController.enabled = true;

		pausePanel.SetActive(false);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}
	private void Update()
	{
		PauseGame();
	}

	public void PauseGame()
	{
		InputAction pauseAction = _input.actions["Pause"];
		if (pauseAction.triggered && isPaused == false)
		{
			pausePanel.SetActive(true);
			player_ThirdPersonController.enabled = false;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			isPaused = true;
		}
	}

	//[PunRPC]
	//public void PauseGame_RPC()
	//{
	//	pausePanel.SetActive(true);
	//	player_ThirdPersonController.enabled = false;

	//	Cursor.visible = true;
	//	Cursor.lockState = CursorLockMode.None;
	//}

	//public void OnResumeButtonClicked()
	//{
	//	photonView.RPC("OnResumeButtonClicked_RPC", RpcTarget.All);
	//}

	//[PunRPC]
	public void OnResumeButtonClicked()
	{
		isPaused = false;
		pausePanel.SetActive(false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;

		player_ThirdPersonController.enabled = true;
	}

	public void OnExitButtonClick()
	{
		Debug.Log("Exit is called");
		Application.Quit();
	}

	//[PunRPC]
	//public void OnExitButtonClick_RPC()
	//{
	//	Debug.Log("Exit is called");
	//	Application.Quit();
	//}
}

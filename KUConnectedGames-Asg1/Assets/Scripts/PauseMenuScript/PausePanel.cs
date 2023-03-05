using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PausePanel : MonoBehaviourPunCallbacks
{
	PlayerInput _input;
	ThirdPersonController player_ThirdPersonController;

	[SerializeField] GameObject pausePanel;
	GameObject player;
	private bool isPaused;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		_input = player.GetComponent<PlayerInput>();

		player_ThirdPersonController = player.GetComponent<ThirdPersonController>();

		if (photonView.IsMine)
		{
			player_ThirdPersonController.enabled = true;

			pausePanel.SetActive(false);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}
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
			photonView.RPC("PauseGame_RPC", RpcTarget.All);

			isPaused = true;
		}
	}

	[PunRPC]
	public void PauseGame_RPC()
	{
		pausePanel.SetActive(true);
		player_ThirdPersonController.enabled = false;

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void OnResumeButtonClicked()
	{
		photonView.RPC("OnResumeButtonClicked_RPC", RpcTarget.All);
	}

	[PunRPC]
	public void OnResumeButtonClicked_RPC()
	{
		isPaused = false;
		pausePanel.SetActive(false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;

		player_ThirdPersonController.enabled = true;
	}

	public void OnExitButtonClick()
	{
		photonView.RPC("OnExitButtonClick_RPC", RpcTarget.All);
	}

	[PunRPC]
	public void OnExitButtonClick_RPC()
	{
		Debug.Log("Exit is called");
		Application.Quit();
	}
}

using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausePanel : MonoBehaviour
{
	//StarterAssetsInputs _input;
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

	}
	private void Update()
	{
		Pause();
	}

	public void Pause()
	{
		InputAction pauseAction = _input.actions["Pause"];
		if (pauseAction.triggered && isPaused == false)
		{
			Debug.Log("pause is pressed");

			pausePanel.SetActive(true);
			player_ThirdPersonController.enabled = false;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			isPaused = true;
		}
	}

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
}

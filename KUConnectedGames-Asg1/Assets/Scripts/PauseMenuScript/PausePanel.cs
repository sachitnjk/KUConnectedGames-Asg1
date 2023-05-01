using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PausePanel : MonoBehaviourPunCallbacks
{
	PlayerInput _input;
	ThirdPersonController _thirdPersonController;
	ThirdPersonShooter _thirdPersonShooter;
	GunSwitcher _gunSwitcher;

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
				_thirdPersonController = player.GetComponent<ThirdPersonController>();
				_thirdPersonShooter = player.GetComponent<ThirdPersonShooter>();
				_gunSwitcher = player.GetComponentInChildren<GunSwitcher>();
				break;
			}
		}

		_thirdPersonController.enabled = true;
		_thirdPersonShooter.enabled = true;
		_gunSwitcher.currentGun.enabled = true;
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
			_thirdPersonController.enabled = false;
			_thirdPersonShooter.enabled = false;
			_gunSwitcher.currentGun.enabled = false;

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

		_thirdPersonController.enabled = true;
		_thirdPersonShooter.enabled = true;
		_gunSwitcher.currentGun.enabled = true;		

	}

	public void OnExitButtonClick()
	{
		Debug.Log("Exit is called");
		Application.Quit();
	}
}

using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviourPunCallbacks
{
	PlayerInput _input;
	InputAction _pauseAction;
	ThirdPersonController _thirdPersonController;
	ThirdPersonShooter _thirdPersonShooter;
	GunSwitcher _gunSwitcher;

	[SerializeField] GameObject pausePanel;
	[SerializeField] GameObject objectivesPanel;
	private bool isPaused;

	private void Start()
	{
		SetPlayerRefData();
		pausePanel.SetActive(false);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}

	private void SetPlayerRefData()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players)
		{
			_input = player.GetComponent<PlayerInput>();
			if (_input != null)
			{
				_thirdPersonController = player.GetComponent<ThirdPersonController>();
				_thirdPersonShooter = player.GetComponent<ThirdPersonShooter>();
				_gunSwitcher = player.GetComponentInChildren<GunSwitcher>();

				break;
			}
		}

		if(_input != null)
		{
			_pauseAction = _input.actions["Pause"];
		}

	}

	private void Update()
	{
		PauseGame();
	}

	public void PauseGame()
	{
		if(_input == null)
		{
			SetPlayerRefData();
		}

		if (_pauseAction.triggered && isPaused == false)
		{
			objectivesPanel.SetActive(false);
			pausePanel.SetActive(true);
			_thirdPersonController.enabled = false;
			_thirdPersonShooter.enabled = false;
			_gunSwitcher.currentGun.enabled = false;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			isPaused = true;
		}
		else
		{
			objectivesPanel.SetActive(true);
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
		Application.Quit();
	}
}

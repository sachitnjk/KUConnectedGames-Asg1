using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviourPunCallbacks
{
	public static PlayerInput _input;
	InputAction _pauseAction;
	ThirdPersonController _thirdPersonController;
	ThirdPersonShooter _thirdPersonShooter;
	GunSwitcher _gunSwitcher;

	[SerializeField] GameObject pausePanel;
	private bool isPaused;

	private void Awake()
	{
		if( _input == null )
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

					_pauseAction = _input.actions["Pause"];
					break;
				}
			}
		}

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
		if(_input == null) return;

		if (_pauseAction.triggered && isPaused == false)
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

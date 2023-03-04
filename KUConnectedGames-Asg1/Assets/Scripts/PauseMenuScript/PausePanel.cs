using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausePanel : MonoBehaviour
{
	StarterAssetsInputs _input;
	[SerializeField] GameObject pausePanel;
	private bool isPaused;

	private void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		_input = player.GetComponent<StarterAssetsInputs>();
	}
	private void Update()
	{
		if(_input != null && _input.pause && !isPaused)
		{
			pausePanel.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			isPaused = true;
		}
		else if(_input != null && !_input.pause && isPaused)
		{
			pausePanel.SetActive(false);
			Cursor.visible=false;
		}
	}

	public void OnResumeButtonClicked()
	{
		isPaused = false;
		Debug.Log(isPaused);
	}

	public void OnExitButtonClick()
	{
		Debug.Log("Exit is called");
		Application.Quit();
	}
}

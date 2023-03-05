using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausePanel : MonoBehaviour
{
	StarterAssetsInputs _input;

	[SerializeField] GameObject pausePanel;
	public bool isPaused;
	public bool resumeButtomClicked = false;

	private void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		_input = player.GetComponent<StarterAssetsInputs>();

	}
	private void Update()
	{
		if (_input.pause && isPaused == false)
		{
			Debug.Log("pause is pressed");

			pausePanel.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			isPaused = true;
		}
		if(resumeButtomClicked)
		{
			Debug.Log("your mom");
			pausePanel.SetActive(false);
			resumeButtomClicked = false;
		}
		if(_input.pause)
		{
			Debug.Log("input for pause is still active");
		}
	}

	public void OnResumeButtonClicked()
	{
		isPaused = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
		resumeButtomClicked = true;
		Debug.Log(isPaused);
	}

	public void OnExitButtonClick()
	{
		Debug.Log("Exit is called");
		Application.Quit();
	}
}

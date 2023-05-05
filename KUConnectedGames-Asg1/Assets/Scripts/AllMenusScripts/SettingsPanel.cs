using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
	[SerializeField] GameObject gameLogo;
	[SerializeField] GameObject mainMenuPanel;
	[SerializeField] GameObject settingsPanel;
	[SerializeField] GameObject keybindingsSelectionPanel;
	[SerializeField] GameObject audioSettingsPanel;
	[SerializeField] GameObject keyboardMouseControlsPanel;
	[SerializeField] GameObject controllerControlsPanel;

	private void Awake()
	{
		mainMenuPanel.SetActive(true);
	}

	public void OnSettingsButtonClick()
	{
		mainMenuPanel.SetActive(false);
		settingsPanel.SetActive(true);
	}

	public void OnMainMenuButtonClick()
	{
		settingsPanel.SetActive(false);
		audioSettingsPanel.SetActive(false);
		mainMenuPanel.SetActive(true);
	}

	public void OnKeybindingsClick()
	{
		keybindingsSelectionPanel.SetActive(true);
		settingsPanel.SetActive(false);
	}

	public void OnAudioSettingsClick()
	{
		settingsPanel.SetActive(false);
		audioSettingsPanel.SetActive(true);
	}

	public void OnKeyboardMouseControlsClick()
	{
		gameLogo.SetActive(false);
		keybindingsSelectionPanel.SetActive(false);
		controllerControlsPanel.SetActive(false);
		keyboardMouseControlsPanel.SetActive(true);
	}

	public void OnControllerControlsClick() 
	{
		gameLogo.SetActive(false);
		keybindingsSelectionPanel.SetActive(false);
		keyboardMouseControlsPanel.SetActive(false);
		controllerControlsPanel.SetActive(true);
	}

	public void OnReturnToSettingsClick()
	{
		gameLogo.SetActive(true);
		settingsPanel.SetActive(true);
		keybindingsSelectionPanel.SetActive(false);
		keyboardMouseControlsPanel.SetActive(false);
		controllerControlsPanel.SetActive(false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelMainScene : MonoBehaviour
{
	[SerializeField] GameObject pauseMainPanel;
	[SerializeField] GameObject pauseSettingsPanel;
	[SerializeField] GameObject keybindingsSelectionPanel;
	[SerializeField] GameObject pauseAudioSettingsPanel;
	[SerializeField] GameObject keyboardMouseControlsPanel;
	[SerializeField] GameObject controllerControlsPanel;

	public void OnSettingsButtonClick()
	{
		pauseMainPanel.SetActive(false);
		pauseSettingsPanel.SetActive(true);
	}

	public void OnReturnToPauseMainPanelButtonClick()
	{
		pauseSettingsPanel.SetActive(false);
		pauseAudioSettingsPanel.SetActive(false);
		keybindingsSelectionPanel.SetActive(false);
		keyboardMouseControlsPanel.SetActive(false);
		controllerControlsPanel.SetActive(false);
		pauseAudioSettingsPanel.SetActive(false);
		pauseMainPanel.SetActive(true);
	}

	public void OnKeybindingsClick()
	{
		keybindingsSelectionPanel.SetActive(true);
		pauseSettingsPanel.SetActive(false);
	}

	public void OnAudioSettingsClick()
	{
		pauseSettingsPanel.SetActive(false);
		pauseAudioSettingsPanel.SetActive(true);
	}

	public void OnKeyboardMouseControlsClick()
	{
		keybindingsSelectionPanel.SetActive(false);
		controllerControlsPanel.SetActive(false);
		keyboardMouseControlsPanel.SetActive(true);
	}

	public void OnControllerControlsClick()
	{
		keybindingsSelectionPanel.SetActive(false);
		keyboardMouseControlsPanel.SetActive(false);
		controllerControlsPanel.SetActive(true);
	}
}

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
	private StarterAssetsInputs _input;
	private ThirdPersonController _thirdPersonController;
	private ThirdPersonShooter _thirdPersonShooter;
	private Animator _playerAnimator;
	private GunScripts _gunScript;

	private GameObject inventoryPanel;

	private void Start()
	{
		inventoryPanel = ReferenceManager.instance.InventoryPanelUI;

		_input = GetComponent<StarterAssetsInputs>();
		_thirdPersonController = GetComponent<ThirdPersonController>();
		_thirdPersonShooter = GetComponent<ThirdPersonShooter>();
		_playerAnimator = GetComponent<Animator>();
		_gunScript = GetComponentInChildren<GunScripts>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;	
	}

	private void Update()
	{
		InventoryTrigger();
	}

	private void InventoryTrigger()
	{
		if(_input.InventoryPanel.WasPressedThisFrame())
		{
			Debug.Log("Getting triggered");
			inventoryPanel.SetActive(true);

			_thirdPersonController.enabled = false; 
			_thirdPersonShooter.enabled = false;
			_playerAnimator.enabled = false;
			_gunScript.enabled = false;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else if (_input.InventoryPanel.WasReleasedThisFrame())
		{
			Debug.Log("Getting released");
			inventoryPanel.SetActive(false);

			_thirdPersonController.enabled = true; 
			_thirdPersonShooter.enabled = true;
			_playerAnimator.enabled = true;
			_gunScript.enabled = true;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;

		}
	}
}

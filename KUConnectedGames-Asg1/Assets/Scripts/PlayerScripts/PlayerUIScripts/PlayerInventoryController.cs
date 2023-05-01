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
	[SerializeField] private GunSwitcher _gunSwitcher;

	private GameObject inventoryPanel;
	public InventoryObject inventoryObject;

	private void OnTriggerEnter(Collider other)
	{
		var item = other.GetComponent<ItemObjectCaller>();
		if (item)
		{
			inventoryObject.AddItem(item.item, 1);
			Destroy(other.gameObject);
		}
	}

	private void OnApplicationQuit()
	{
		inventoryObject.Container.Clear();
	}

	private void Start()
	{
		inventoryPanel = ReferenceManager.instance.InventoryPanelUI;

		_input = GetComponent<StarterAssetsInputs>();
		_thirdPersonController = GetComponent<ThirdPersonController>();
		_thirdPersonShooter = GetComponent<ThirdPersonShooter>();
		_playerAnimator = GetComponent<Animator>();

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
			inventoryPanel.SetActive(true);

			_thirdPersonController.enabled = false; 
			_thirdPersonShooter.enabled = false;
			_gunSwitcher.currentGun.enabled = false;

			_playerAnimator.enabled = false;


			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else if (_input.InventoryPanel.WasReleasedThisFrame())
		{
			inventoryPanel.SetActive(false);

			_thirdPersonController.enabled = true; 
			_thirdPersonShooter.enabled = true;
			_gunSwitcher.currentGun.enabled = true;

			_playerAnimator.enabled = true;


			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;

		}
	}
}

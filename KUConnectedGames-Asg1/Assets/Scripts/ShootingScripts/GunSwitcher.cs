using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
	private GameObject weaponSlot1;
	private GameObject weaponSlot2;

	[SerializeField] StarterAssetsInputs _input;

	private void Start()
	{
		weaponSlot1 = ReferenceManager.instance.PrimaryWeapon;
		weaponSlot2 = ReferenceManager.instance.SecondaryWeapon;

		EquipPrimary();
	}

	private void Update()
	{
		if(_input.PrimaryWeapon.WasPressedThisFrame())
		{
			EquipPrimary();
		}
		else if(_input.SecondaryWeapon.WasPressedThisFrame()) 
		{
			EquipSecondary();
		}
	}

	private void EquipPrimary()
	{
		weaponSlot1.SetActive(true);
		weaponSlot2.SetActive(false);
	}

	private void EquipSecondary()
	{
		weaponSlot1.SetActive(false);
		weaponSlot2.SetActive(true);
	}
}

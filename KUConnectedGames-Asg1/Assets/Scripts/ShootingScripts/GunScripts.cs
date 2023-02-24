using Photon.Pun.Demo.Asteroids;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GunScripts : MonoBehaviour
{
	//	Bullet prefab
	//RoF
	//Magazine size
	//Bullet damage
	//Bullet speed
	//Reload speed
	//bullet spread  -> will need to look more into this
	//bullet range   -> delete after this range
	//bullet impact

	[Header("All gun realated attributes")]
	[SerializeField] private float gun_RateOfFire;
	[SerializeField] private float gun_NextTimeToFire;
	[SerializeField] private float gun_ReloadTime;
	[SerializeField] private float gun_MagazineSize;
	[SerializeField] private Camera gun_CamToShootAlong;

	private bool isReloading;

	StarterAssetsInputs _input;

	[Header("Ammo control")]
	[SerializeField] private int gun_MaxAmmo;
	[SerializeField] private int gun_CurrentAmmo;

	[Header("Fire Modes")]
	[SerializeField] private bool Automatic;
	[SerializeField] private bool semiAutomatic;

	[Header("All bullet attributes")]
	[SerializeField] private GameObject bullet_Prefab;
	[SerializeField] private GameObject bullet_Impact;
	[SerializeField] private float bullet_Damage;
	[SerializeField] private float bullet_Spread;
	[SerializeField] private float bullet_Range;

	private void Start()
	{
		//gun_CurrentAmmo = gun_MaxAmmo;
		_input = GetComponent<StarterAssetsInputs>();

	}

	private void Update()
	{
		if(isReloading)
		{
			return;
		}
		if(gun_CurrentAmmo <= 1)
		{
			StartCoroutine(Reload());
			return;
		}
		if(_input.shoot && Time.time >= gun_NextTimeToFire)
		{
			gun_NextTimeToFire = Time.time + 1f / gun_RateOfFire;
			Debug.Log("shoot is being called");
			//Shoot();
		}
		
	}

	IEnumerator Reload()
	{
		isReloading = true;
		yield return new WaitForSeconds(gun_ReloadTime - 0.25f);
		yield return new WaitForSeconds(0.25f);
		gun_CurrentAmmo = gun_MaxAmmo;
		isReloading = false;

	}

	private void Shoot()
	{
	}

}

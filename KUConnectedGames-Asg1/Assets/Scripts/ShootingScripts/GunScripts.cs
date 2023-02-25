using Cinemachine;
using ClipperLib;
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
	[Header("All gun realated attributes")]
	[SerializeField] private float gun_RateOfFire;
	[SerializeField] private float gun_NextTimeToFire;
	[SerializeField] private float gun_ReloadTime;
	[SerializeField] private float gun_MagazineSize;
	[SerializeField] private CinemachineVirtualCamera gun_PlayerVirtualCam;
	[SerializeField] private Camera gun_PlayerMainCam;
	[SerializeField] private GameObject gun_ShootPoint;

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

	[SerializeField] private float ray_Range;

	[SerializeField] private GameObject screenCenter_GO;
	private Vector3 screenCenter_Vector;

	private void Start()
	{
		//gun_CurrentAmmo = gun_MaxAmmo;
		_input = GetComponent<StarterAssetsInputs>();
		screenCenter_Vector = gun_PlayerMainCam.ScreenToWorldPoint(screenCenter_GO.transform.position);

	}

	private void Update()
	{
		if (_input.shoot/*&& Time.time >= gun_NextTimeToFire*/)
		{
			/*gun_NextTimeToFire = Time.time + 2f / gun_RateOfFire*/;
			Debug.Log("shoot is being called");
			Shoot();
			_input.shoot = false;
		}

		if (isReloading)
		{
			return;
		}

		if(gun_CurrentAmmo <= 1)
		{
			StartCoroutine(Reload());
			return;
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
		Vector3 ray_PlayerZAxis = gun_PlayerVirtualCam.Follow.position - gun_PlayerVirtualCam.transform.position;
		ray_PlayerZAxis.y = 0;
		ray_PlayerZAxis.Normalize();

		Vector3 ray_Origin = gun_ShootPoint.transform.position;

		Ray ray = new Ray(ray_Origin, ray_PlayerZAxis);

		if (Physics.Raycast(ray, out RaycastHit ray_hit))
		{
			Debug.Log(ray_hit.collider.gameObject.name);
		}
	}

}

using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

	[SerializeField] private LayerMask ray_RaycastMask;

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
		screenCenter_Vector = gun_PlayerMainCam.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));

		Debug.Log(screenCenter_Vector);

		Vector3 ray_PlayerZAxis = screenCenter_Vector - gun_ShootPoint.transform.position;
		ray_PlayerZAxis.Normalize();

		Vector3 ray_Origin = gun_ShootPoint.transform.position;

		Ray ray = new Ray(ray_Origin, -ray_PlayerZAxis);

		if (Physics.Raycast(ray, out RaycastHit ray_hit, ray_Range, ray_RaycastMask))
		{
			Debug.Log(ray_hit.collider.gameObject.name, ray_hit.collider.gameObject);
			Debug.DrawLine(ray_Origin, ray_hit.point, Color.red, 5f);

		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(screenCenter_Vector, 0.5f);
	}

}

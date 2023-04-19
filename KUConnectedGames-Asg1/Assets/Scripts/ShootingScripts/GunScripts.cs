using Cinemachine;
using StarterAssets;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;

public class GunScripts : MonoBehaviourPunCallbacks
{
	[Header("All gun realated attributes")]
	[SerializeField] private float gun_RateOfFire;
	[SerializeField] private float gun_ReloadTime;
	[SerializeField] private float gun_MagazineSize;
	[SerializeField] private CinemachineVirtualCamera gun_PlayerVirtualCam;
	[SerializeField] private Camera gun_PlayerMainCam;
	[SerializeField] private GameObject gun_ShootPoint;

	[SerializeField] private LayerMask ray_RaycastMask;

	private bool isReloading;

	[SerializeField] StarterAssetsInputs _input;

	[Header("Ammo control")]
	[SerializeField] private int gun_MaxAmmo;
	[SerializeField] private int gun_CurrentAmmo;

	[Header("Fire Modes")]
	[SerializeField] private FireModes currentFireMode;

	[Header("All bullet attributes")]
	[SerializeField] bool addProjectile;
	[SerializeField] private GameObject bullet_Prefab;
	[SerializeField] private VisualEffect bullet_Impact;
	[SerializeField] private float bullet_Speed;
	[SerializeField] private int bullet_Damage;
	[SerializeField] private float bullet_Spread;

	private Vector3 bullet_Target;

	[SerializeField] private float ray_Range;

	private enum FireModes
	{
		SingleShot,
		BurstFire,
		AutoFire
	}

	private void Start()
	{
		gun_CurrentAmmo = gun_MaxAmmo;
	}

	private void Update()
	{
		if (_input.Shoot.WasPressedThisFrame())
		{
			StartCoroutine(ShootWithFireMode());
		}

		if (_input.SwitchFireMode.WasPressedThisFrame())
		{
			ChangeFireMode();
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

	private void ChangeFireMode()
	{
		switch(currentFireMode)
		{
			case FireModes.SingleShot:
				currentFireMode = FireModes.BurstFire;
				break;
			case FireModes.BurstFire:
				currentFireMode = FireModes.AutoFire;
				break;
			case FireModes.AutoFire:
				currentFireMode = FireModes.SingleShot;
				break;
		}
	}

	private IEnumerator ShootWithFireMode()
	{
		int shotsFiredInBurst = 0;

		switch(currentFireMode)
		{
			case FireModes.SingleShot:
				Shoot();
				yield return new WaitForSeconds(1f);
				break;
			case FireModes.BurstFire:
				for(int i = 0; i < 3; i++)
				{
					while(shotsFiredInBurst < 4)
					{
						Shoot();
						shotsFiredInBurst++;
						yield return new WaitForSeconds(0.1f);
					}
				}
				break;
			case FireModes.AutoFire:
				while (_input.Shoot.ReadValue<float>() > 0)
				{
					Shoot();
					yield return new WaitForSeconds(0.1f);
				}
				break;
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
		if(gun_CurrentAmmo > 0)
		{
			Ray ray = gun_PlayerMainCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

			Vector2 bulletSpreadOffset = Random.insideUnitCircle * bullet_Spread;
			Quaternion spreadRotation = Quaternion.Euler(bulletSpreadOffset.x, bulletSpreadOffset.y, 0);
			ray.direction = spreadRotation * ray.direction;


			RaycastHit hitResult;
			if (Physics.Raycast(ray, out hitResult, 1000.0f))
			{
				GameObject hitObject = hitResult.collider.gameObject;
				bullet_Target = hitResult.point;
				GameObject hitImpact_basic = PhotonNetwork.Instantiate(bullet_Impact.name, bullet_Target, Quaternion.identity);

				if (hitObject.CompareTag("Enemy"))
				{
					Enemy_AiBehaviour aiBehaviour = hitObject.GetComponent<Enemy_AiBehaviour>();
					PhotonView enemyPhotonView = hitObject.GetComponent<PhotonView>();
					if (aiBehaviour != null && aiBehaviour.GetCurrentState() != Enemy_AiBehaviour.State.IsHit)
					{
						enemyPhotonView.RPC("TriggerHitAnimation", RpcTarget.All);
					}

					if(enemyPhotonView != null)
					{
						enemyPhotonView.RPC("EnemyDamageTake", RpcTarget.AllBuffered, bullet_Damage);
					
					}
				}
				StartCoroutine(DestroyHitImpact(hitImpact_basic));

			}
			else
			{
				bullet_Target = gun_PlayerMainCam.transform.position + (ray.direction * 1000.0f);
			}

			gun_CurrentAmmo--;

			if(addProjectile)
			{
				GameObject bullet_Instance = Instantiate(bullet_Prefab, gun_ShootPoint.transform.position, gun_ShootPoint.transform.rotation);
				Vector3 bullet_Direction = (bullet_Target - gun_ShootPoint.transform.position).normalized;
				bullet_Instance.GetComponent<Rigidbody>().velocity = bullet_Direction * bullet_Speed;
				Destroy(bullet_Instance, 1.5f);
			}
		}
	}
	IEnumerator DestroyHitImpact(GameObject hitImpactObject)
	{
		yield return new WaitForSeconds(.5f);
		if(bullet_Impact != null)
		{
			bullet_Impact.Stop();
			Destroy(hitImpactObject.gameObject);
		}
	}
}

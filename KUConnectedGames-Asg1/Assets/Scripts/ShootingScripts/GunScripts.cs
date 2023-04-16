using Cinemachine;
using StarterAssets;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;
using UnityEngine.Rendering;

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

	private float gun_NextTimeToFire = 0f;

	private bool isReloading;

	[SerializeField] StarterAssetsInputs _input;

	[Header("Ammo control")]
	[SerializeField] private int gun_MaxAmmo;
	[SerializeField] private int gun_CurrentAmmo;

	[Header("Fire Modes")]
	[SerializeField] private bool Automatic;
	[SerializeField] private bool semiAutomatic;

	[Header("All bullet attributes")]
	[SerializeField] bool addProjectile;
	[SerializeField] private GameObject bullet_Prefab;
	[SerializeField] private GameObject bullet_Impact;
	[SerializeField] private float bullet_Speed;
	[SerializeField] private int bullet_Damage;

	private VisualEffect hitImpact_1;

	private Vector3 bullet_Target;

	[SerializeField] private float ray_Range;

	[SerializeField] private GameObject screenCenter_GO;

	private void Start()
	{
		gun_CurrentAmmo = gun_MaxAmmo;

		hitImpact_1 = bullet_Impact.GetComponent<VisualEffect>();
	}

	private void Update()
	{
		if (_input.shoot && Time.time >= gun_NextTimeToFire)
		{
			gun_NextTimeToFire = Time.time + 2f / gun_RateOfFire;
			Shoot();
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
		if(gun_CurrentAmmo > 0)
		{
			Ray ray = gun_PlayerMainCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
			RaycastHit hitResult;
			if (Physics.Raycast(ray, out hitResult, 1000.0f))
			{
				GameObject hitObject = hitResult.collider.gameObject;
				bullet_Target = hitResult.point;
				VisualEffect hitImpact_basic = Instantiate(hitImpact_1, bullet_Target, Quaternion.identity);

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
						Debug.Log("This weird photon call through gun script");
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
	IEnumerator DestroyHitImpact(VisualEffect hitImpactObject)
	{
		yield return new WaitForSeconds(.5f);
		if(hitImpact_1 != null)
		{
			hitImpact_1.Stop();
			Destroy(hitImpactObject.gameObject);
		}
	}
}

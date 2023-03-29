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
	[SerializeField] private GameObject bullet_Prefab;
	[SerializeField] private GameObject bullet_Impact;
	[SerializeField] private int bullet_Damage;
	[SerializeField] private float bullet_Spread;

	private VisualEffect hitImpact_1;

	private Vector3 bullet_Target;

	[SerializeField] private float ray_Range;

	[SerializeField] private GameObject screenCenter_GO;

	private void Start()
	{
		gun_CurrentAmmo = gun_MaxAmmo;

		hitImpact_1 = ReferenceManager.instance.hitImpactVisualEffects;
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
		Ray ray = gun_PlayerMainCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		RaycastHit hitResult;
		if (Physics.Raycast(ray, out hitResult, 1000.0f))
		{
			GameObject hitObject = hitResult.collider.gameObject;
			bullet_Target = hitResult.point;
			VisualEffect hitImpact_basic =  Instantiate(hitImpact_1, bullet_Target, Quaternion.identity);

			Debug.Log(hitResult.collider.gameObject.name);

			if (hitObject.CompareTag("Enemy"))
			{

				PhotonView enemyPhotonView = hitObject.GetComponent<PhotonView>();
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
	}
	IEnumerator DestroyHitImpact(VisualEffect hitImpactObject)
	{
		yield return new WaitForSeconds(1.0f);
		hitImpactObject.Stop();
		Destroy(hitImpactObject);
	}
}

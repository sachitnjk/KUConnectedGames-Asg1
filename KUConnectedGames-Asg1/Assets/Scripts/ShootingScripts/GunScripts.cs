using Cinemachine;
using StarterAssets;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;
using TMPro;

public class GunScripts : MonoBehaviourPunCallbacks
{
	[Header("All gun realated attributes")]
	[SerializeField] private float gun_RateOfFire;
	[SerializeField] private float gun_ReloadTime;
	[SerializeField] private int gun_MagazineSize;
	[SerializeField] private int gun_MagazineCount;
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

	private GunRecoil _recoilScript;

	private TextMeshProUGUI currentAmmotext;
	private TextMeshProUGUI maxAmmotext;

	private GameObject singleshotIcon;
	private GameObject burstFireIcon;
	private GameObject autoFireIcon;

	AudioManager.AudioClipEnum gunAudio;

	private enum FireModes
	{
		SingleShot,
		BurstFire,
		AutoFire
	}

	private void Awake()
	{
		singleshotIcon = ReferenceManager.instance.SingleshotIcon;
		burstFireIcon = ReferenceManager.instance.BurstFireIcon;
		autoFireIcon = ReferenceManager.instance.AutoFireIcon;

		singleshotIcon.SetActive(true);
		burstFireIcon.SetActive(false);
		autoFireIcon.SetActive(false);
	}

	private void Start()
	{
		_recoilScript = GetComponent<GunRecoil>();

		if (currentAmmotext == null)
		{
			currentAmmotext = ReferenceManager.instance.gunCurrentAmmoField;
			maxAmmotext = ReferenceManager.instance.gunMaxAmmoField;
		}

		gun_MaxAmmo = gun_MagazineSize * gun_MagazineCount;
		gun_CurrentAmmo = gun_MagazineSize;

		currentAmmotext.text = gun_CurrentAmmo.ToString();
		maxAmmotext.text = gun_MaxAmmo.ToString();

		gunAudio = AudioManager.AudioClipEnum.SingleShot;
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

		if(gun_CurrentAmmo <= 1 && gun_MaxAmmo >= 1)
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
		gun_CurrentAmmo = gun_MagazineSize;
		gun_MaxAmmo -= gun_MagazineSize;

		currentAmmotext.text = gun_CurrentAmmo.ToString();
		maxAmmotext.text = gun_MaxAmmo.ToString();

		isReloading = false;

	}

	private void ChangeFireMode()
	{
		switch(currentFireMode)
		{
			case FireModes.SingleShot:
				singleshotIcon.SetActive(false);
				burstFireIcon.SetActive(true);
				autoFireIcon.SetActive(false);
				currentFireMode = FireModes.BurstFire; 
				break;
			case FireModes.BurstFire:
				singleshotIcon.SetActive(false);
				burstFireIcon.SetActive(false);
				autoFireIcon.SetActive(true);
				currentFireMode = FireModes.AutoFire;
				break;
			case FireModes.AutoFire:
				singleshotIcon.SetActive(true);
				burstFireIcon.SetActive(false);
				autoFireIcon.SetActive(false);
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


	private void Shoot()
	{
		if(gun_CurrentAmmo > 0 && gun_MaxAmmo >= 0)
		{
			Ray ray = gun_PlayerMainCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

			_recoilScript.TriggerRecoil();

			Vector2 bulletSpreadOffset = Random.insideUnitCircle * bullet_Spread;
			Quaternion spreadRotation = Quaternion.Euler(bulletSpreadOffset.x, bulletSpreadOffset.y, 0);
			ray.direction = spreadRotation * ray.direction;


			RaycastHit hitResult;
			if (Physics.Raycast(ray, out hitResult, 1000.0f))
			{
				GameObject hitObject = hitResult.collider.gameObject;
				bullet_Target = hitResult.point;
				GameObject hitImpact_basic = PhotonNetwork.Instantiate(bullet_Impact.name, bullet_Target, Quaternion.identity);

				AudioManager.instance.PlayOneShotAudio(gunAudio); //Gunshot pew sound

				if (hitObject.CompareTag("Enemy"))
				{
					Enemy_AiBehaviour aiBehaviour = hitObject.GetComponent<Enemy_AiBehaviour>();
					PhotonView enemyPhotonView = hitObject.GetComponent<PhotonView>();
					if (aiBehaviour != null && aiBehaviour.GetCurrentState() != Enemy_AiBehaviour.State.IsHit)
					{
						enemyPhotonView.RPC("TriggerHitAnimation", RpcTarget.All);
						aiBehaviour.navMeshAgent.SetDestination(aiBehaviour.TargetPosition());
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
			currentAmmotext.text = gun_CurrentAmmo.ToString();

			if (addProjectile)
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

	public void AddMaxAmmo(int addedAmmo)
	{
		gun_MaxAmmo = gun_MaxAmmo + addedAmmo;
		maxAmmotext.text = gun_MaxAmmo.ToString();
	}

	public void UpdateAmmoDisplay()
	{
		if(currentAmmotext == null)
		{
			currentAmmotext = ReferenceManager.instance.gunCurrentAmmoField;
			maxAmmotext = ReferenceManager.instance.gunMaxAmmoField;
		}
		currentAmmotext.text = gun_CurrentAmmo.ToString();
		maxAmmotext.text = gun_MaxAmmo.ToString();
	}

	public void UpdateFireModeIcons()
	{
		if(currentFireMode == FireModes.SingleShot) 
		{
			singleshotIcon.SetActive(true);
			burstFireIcon.SetActive(false);
			autoFireIcon.SetActive(false);
		}
		else if(currentFireMode == FireModes.BurstFire) 
		{
			burstFireIcon.SetActive(true);
			singleshotIcon.SetActive(false);
			autoFireIcon.SetActive(false);
		}
		else
		{
			autoFireIcon.SetActive(true);
			singleshotIcon.SetActive(false);
			burstFireIcon.SetActive(false);
		}
	}

}

using Cinemachine;
using StarterAssets;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonShooter : MonoBehaviourPunCallbacks
{
	StarterAssetsInputs _input;
	ThirdPersonController _TPController;
	
	[Header("Component references")]
	[SerializeField] CinemachineVirtualCamera cm_AimVirtualCamera;
	[SerializeField] LayerMask aimColliderLayerMask;


	[Header("Sensitivity controls")]
	[SerializeField] float aimSensitivity;
	[SerializeField] float normalSensitivity;

	private bool shouldAim = false;
	private bool isAiming = false;
	private float targetSpeed;
	private Animator _animator;

	private void Awake()
	{
		_input = GetComponent<StarterAssetsInputs>();
		_TPController = GetComponent<ThirdPersonController>();
		_animator = GetComponent<Animator>();

		targetSpeed = Mathf.Abs(_TPController.targetSpeed);
	}

	private void Update()
	{
		if(photonView.IsMine)
		{
			Vector3 mouseWorldPosition = Vector3.zero;

			Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
			Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
			if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
			{
				mouseWorldPosition = raycastHit.point;
			}

			shouldAim = _input.isAiming;
			if(shouldAim != isAiming)
			{
				photonView.RPC("SetAiming", RpcTarget.All, shouldAim);
			}

			if (isAiming || _input.Shoot.IsPressed())
			{
				cm_AimVirtualCamera.gameObject.SetActive(true);
				_TPController.SetSensitivity(aimSensitivity);
				_TPController.SetRotateOnMove(false);

				//Aim blend tree anims
				targetSpeed = Mathf.Abs(_TPController.targetSpeed);
				_animator.SetFloat("Speed", targetSpeed);

				_animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

				float weight = Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f);
				photonView.RPC("SetLayerWeight", RpcTarget.All, weight);

				Vector3 worldAimTarget = mouseWorldPosition;
				worldAimTarget.y = transform.position.y;
				Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

				transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
			}
			else
			{
				cm_AimVirtualCamera.gameObject.SetActive(false);
				_TPController.SetSensitivity(normalSensitivity);
				_TPController.SetRotateOnMove(true);

				_animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

				float weight = Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f);
				photonView.RPC("SetLayerWeight", RpcTarget.All, weight);
			}

			if(_input.Shoot.IsPressed())
			{
				_TPController.SetRotateOnMove(false);

				Vector3 worldAimTarget = mouseWorldPosition;
				worldAimTarget.y = transform.position.y;
				Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

				transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
			}
		}
	}

	[PunRPC]
	private void SetAiming(bool aiming)
	{
		isAiming = aiming;
	}

	[PunRPC]
	private void SetLayerWeight(float weight)
	{
		_animator.SetLayerWeight(1, weight);
	}

}

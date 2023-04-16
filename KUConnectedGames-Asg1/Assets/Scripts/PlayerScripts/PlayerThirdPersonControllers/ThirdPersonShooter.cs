using Cinemachine;
using StarterAssets;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonShooter : MonoBehaviourPunCallbacks
{
	[SerializeField] CinemachineVirtualCamera cm_AimVirtualCamera;
	StarterAssetsInputs _input;
	ThirdPersonController _TPController;

	[SerializeField] LayerMask aimColliderLayerMask;
	[SerializeField] float aimSensitivity;
	[SerializeField] float normalSensitivity;

	private Animator _animator;

	private void Awake()
	{
		_input = GetComponent<StarterAssetsInputs>();
		_TPController = GetComponent<ThirdPersonController>();
		_animator = GetComponent<Animator>();
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

			if (_input.isAiming)
			{
				cm_AimVirtualCamera.gameObject.SetActive(true);
				_TPController.SetSensitivity(aimSensitivity);
				_TPController.SetRotateOnMove(false);
				_animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

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

}

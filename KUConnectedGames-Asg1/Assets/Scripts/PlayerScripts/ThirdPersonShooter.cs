using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonShooter : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera cm_AimVirtualCamera;
	StarterAssetsInputs _input;
	ThirdPersonController _TPController;

	[SerializeField] LayerMask aimColliderLayerMask;
	[SerializeField] float aimSensitivity;
	[SerializeField] float normalSensitivity;

	private void Awake()
	{
		_input = GetComponent<StarterAssetsInputs>();
		_TPController = GetComponent<ThirdPersonController>();
	}

	private void Update()
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
		}

		if(_input.shoot)
		{
			_TPController.SetRotateOnMove(false);

			Vector3 worldAimTarget = mouseWorldPosition;
			worldAimTarget.y = transform.position.y;
			Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

			transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
		}
	}

}

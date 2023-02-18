using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimTurnToCamera : MonoBehaviour
{
	[SerializeField] private Transform cameraTransform;

	PlayerInput playerInput;
	InputAction aim;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		aim = playerInput.actions["Aim"];
	}

	void Update()
	{
		// Check if the player is aiming
		if (aim.triggered)
		{
			// Calculate the rotation towards the camera
			Vector3 cameraForward = cameraTransform.forward;
			cameraForward.y = 0; // Zero out the y component to prevent tilting
			Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

			// Apply the rotation to the character
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
		}
	}
}

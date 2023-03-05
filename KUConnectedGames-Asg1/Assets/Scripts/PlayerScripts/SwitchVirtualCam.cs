using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVirtualCam : MonoBehaviour
{

	[SerializeField] private PlayerInput playerInput;

	private CinemachineVirtualCamera cinemachineVirtCam;

	private InputAction aimInput;

	[SerializeField] private int priorityBumped = 10;

	private void Awake()
	{
		cinemachineVirtCam = GetComponent<CinemachineVirtualCamera>();
		aimInput = playerInput.actions["Aim"];
	}

	private void OnEnable()
	{
		aimInput.started += _ => StartAim();
		aimInput.canceled += _ => EndAim();
	}

	private void OnDisable()
	{
		aimInput.started -= _ => StartAim();
		aimInput.canceled -= _ => EndAim();
	}

	private void StartAim()
	{
		cinemachineVirtCam.Priority += priorityBumped;
	}

	private void EndAim()
	{
		cinemachineVirtCam.Priority -= priorityBumped;
	}

}

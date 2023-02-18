using Cinemachine;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform firePoint;

	PlayerInput playerInput;
	InputAction shoot;

	public float bulletSpeed = 20f;
	public float fireRate = 0.5f;
	private float nextFireTime = 0f;

	private CinemachineVirtualCamera virtualCamera;

	private void Start()
	{
		virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

		playerInput = GetComponent<PlayerInput>();
		shoot = playerInput.actions["Shoot"];

	}
	void Update()
	{
		if (shoot.triggered && Time.time >= nextFireTime)
		{
			nextFireTime = Time.time + 0f / fireRate;
			Shoot();
		}
	}

	void Shoot()
	{
		Vector3 cameraForward = virtualCamera.transform.forward;
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bullet.GetComponent<Rigidbody>().velocity = cameraForward * bulletSpeed;
		BulletController bulletController = bullet.GetComponent<BulletController>();
		if (bulletController != null)
		{
			bulletController.speed = bulletSpeed;
		}
	}
}

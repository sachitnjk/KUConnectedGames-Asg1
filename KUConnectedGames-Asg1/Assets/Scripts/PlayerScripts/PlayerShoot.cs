using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 25f;

	private CharacterController charController;
	private PlayerInput playerInput;
    private InputAction shootAction;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
		shootAction.performed -= _ => ShootGun();
	}

    private void ShootGun()
    {
        RaycastHit hit;
		GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
		BulletController bulletController = bullet.GetComponent<BulletController>();
		if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = camTransform.position + camTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
    }
}

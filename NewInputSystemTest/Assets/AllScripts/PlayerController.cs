using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();  
    }

    private void Start()
    {

    }

    private void Update()
    {
		Vector2 move = playerControls.Ground.Move.ReadValue<Vector2>();
		Debug.Log(move);

		if (playerControls.Ground.Jump.triggered)
		{
			Debug.Log("Jump called");
		}
	}


}

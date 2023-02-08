using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStaminaBar : MonoBehaviour
{

    public float maxStam = 100;
	public float currentStam;

    [SerializeField] private float stamDecay;
    [SerializeField] private float stamRegen;

    public StaminaBarScript stamBarScript;
    private PlayerInput playerInput;
    //private ThirdPersonController tpController;

    InputAction sprintEnabled;

    void Start()
    {
		playerInput = gameObject.GetComponent<PlayerInput>();
        //tpController = gameObject.GetComponent<ThirdPersonController>();
        currentStam = maxStam;
        sprintEnabled = playerInput.actions["Sprint"];
        stamBarScript.SetMaxStamina(maxStam);

	}

    void Update()
    {
        StaminaExecute();
    }

    void StaminaExecute()
    {

        if(sprintEnabled.IsPressed() && currentStam > 0)
        {
            SprintStamDepletion(stamDecay);
            //Debug.Log("Toooob");
        }
        else if(!sprintEnabled.IsPressed() && currentStam < maxStam)
        {
            //tpController._speed = tpController.MoveSpeed;
            SprintStamRegen(stamRegen);
            //Debug.Log("Hedfones");
        }
    }

    void SprintStamRegen(float stamRegen)
    {
        currentStam += stamRegen * Time.deltaTime;
        stamBarScript.SetStamina(currentStam);
    }

    void SprintStamDepletion(float stamDecay)
    {

        currentStam -= stamDecay * Time.deltaTime;
		stamBarScript.SetStamina(currentStam);
        //Debug.Log(currentStam);

    }

}

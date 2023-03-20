using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStaminaBar : MonoBehaviour
{

    public float maxStam = 100;
	public float currentStam;

    //private bool isSprinting;

    [SerializeField] private float stamDecay;
    [SerializeField] private float stamRegen;

    public StaminaBarScript stamBarScript;
    public ThirdPersonController tpController;
    private StarterAssetsInputs _input;

    void Start()
    {
        currentStam = maxStam;
        stamBarScript?.SetMaxStamina(maxStam);

        tpController = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        //isSprinting = false;
	}

    void Update()
    {
        StaminaExecute();
    }

    void StaminaExecute()
    {
        if (_input.sprint && tpController.targetSpeed > 0 && currentStam > 0)
        {
            SprintStamDepletion(stamDecay);
        }
        else if (!_input.sprint && currentStam < maxStam)
        {
            SprintStamRegen(stamRegen);
        }
    }

    void SprintStamRegen(float stamRegen)
    {
        currentStam += stamRegen * Time.deltaTime;
        stamBarScript?.SetStamina(currentStam);
    }

    void SprintStamDepletion(float stamDecay)
    {
        currentStam -= stamDecay * Time.deltaTime;
		stamBarScript?.SetStamina(currentStam);
    }

}

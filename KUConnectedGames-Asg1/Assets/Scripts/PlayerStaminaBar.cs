using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStaminaBar : MonoBehaviour
{

    public float maxStam = 100;
	public float currentStam;

    private bool isSprinting;

    [SerializeField] private float stamDecay;
    [SerializeField] private float stamRegen;

    public StaminaBarScript stamBarScript;

    void Start()
    {
        currentStam = maxStam;
        stamBarScript?.SetMaxStamina(maxStam);

	}

    void Update()
    {
        StaminaExecute();
    }

    void StaminaExecute()
    {

        if (isSprinting && currentStam > 0)
        {
            SprintStamDepletion(stamDecay);
        }
        else if (!isSprinting && currentStam < maxStam)
        {
            SprintStamRegen(stamRegen);
        }
    }

    private void OnAim(InputValue value)
    {

        Debug.Log("I am aiming now");

    }

    public void OnSprint(InputValue value)
    {
       isSprinting = value.isPressed;
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

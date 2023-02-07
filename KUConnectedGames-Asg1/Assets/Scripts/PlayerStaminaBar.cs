using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStaminaBar : MonoBehaviour
{

    public int maxStam = 100;
	public int currentStam;

    [SerializeField] private int stamDecay;
    [SerializeField] private int stamRegen;

    public StaminaBarScript stamBarScript;
    private PlayerInput playerInput;

    InputAction sprintEnabled;

    void Start()
    {
		playerInput = gameObject.GetComponent<PlayerInput>();
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
        if(sprintEnabled.triggered)
        {
            SprintStamDepletion(stamDecay);
            Debug.Log("sprint Decay");
            Debug.Log(currentStam);
        }
        else
        {
            SprintStamRegen(stamRegen);
			Debug.Log("sprint Regen");
		}
    }

    void SprintStamRegen(int stamRegen)
    {
        currentStam += stamRegen;
        currentStam = (int)Mathf.Clamp(currentStam, 0, maxStam);
        stamBarScript.SetStamina(currentStam);
    }

    void SprintStamDepletion(int stamDecay)
    {

        currentStam -= stamDecay;
		currentStam = (int)Mathf.Clamp(currentStam, 0, maxStam);
		stamBarScript.SetStamina(currentStam);

    }

}

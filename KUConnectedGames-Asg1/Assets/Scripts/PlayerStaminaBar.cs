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

		//currentStam = (int)Mathf.Clamp(currentStam, 0, maxStam);
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
            Debug.Log("Toooob");
        }
        else if(!sprintEnabled.IsPressed() && currentStam < maxStam)
        {
            SprintStamRegen(stamRegen);
            Debug.Log("Hedfones");
        }
    }

    void SprintStamRegen(int stamRegen)
    {
        currentStam += stamRegen /** (int)Time.deltaTime*/;
        stamBarScript.SetStamina(currentStam);
    }

    void SprintStamDepletion(int stamDecay)
    {

        currentStam -= stamDecay /** (int)Time.deltaTime*/;
		stamBarScript.SetStamina(currentStam);
        Debug.Log(currentStam);

    }

}

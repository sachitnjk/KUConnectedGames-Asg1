using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityController : MonoBehaviour
{

    private PlayerInput _input;

    private InputAction abilityAction;

    [HideInInspector] public bool abilityTriggered;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        abilityAction = _input.actions["Ability"];
    }
    void Update()
    {
        abilityTriggered = false;

        if(PlayerKillCounter.Instance.player_AbilityUse && abilityAction.WasPressedThisFrame())
        {
            abilityTriggered = true;
            PlayerKillCounter.Instance.KillCounterReset();
        }
        else if(!PlayerKillCounter.Instance.player_AbilityUse && abilityAction.WasPressedThisFrame())
        {
            abilityTriggered= false;
        }
        Debug.Log(abilityTriggered);
        Debug.Log(PlayerKillCounter.Instance.player_AbilityUse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IAbilityController
{
	void AbilityUse(Vector3 playerPosition);
}

public class AbilityController : MonoBehaviour
{
	private IAbilityController activeAbility;

	private PlayerInput _input;
    private InputAction abilityAction;

    [HideInInspector] public bool abilityTriggered;
	[SerializeField] AbilityType playerAbilityType;

	private float lastAbilityTriggeredTime;
	[SerializeField] float abilityCooldown;

	[SerializeField] ParticleSystem dashAbilityVisualEffect;
	ParticleSystem currentAbilityVisualEffect;

	AudioManager.AudioClipEnum abilitySound;

	private enum AbilityType
	{
		Impulse,
		Attacker
	}

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        abilityAction = _input.actions["Ability"];

		if (playerAbilityType == AbilityType.Impulse)
		{
			Ability_Impulse abilityImpulse = gameObject.AddComponent<Ability_Impulse>();
			SetActiveAbility(abilityImpulse);
			abilitySound = AudioManager.AudioClipEnum.Impulse;
		}
		else if (playerAbilityType == AbilityType.Attacker)
		{
			Ability_AttackerDash attackerDash = gameObject.AddComponent<Ability_AttackerDash>();
			SetActiveAbility(attackerDash);
			SetActiveAbilityVisual(dashAbilityVisualEffect);
			abilitySound = AudioManager.AudioClipEnum.Dash;
		}

		currentAbilityVisualEffect.Stop();
	}

	public void SetActiveAbility(IAbilityController ability)
	{
		activeAbility = ability;
	}

	public void SetActiveAbilityVisual(ParticleSystem abilityVisual)
	{
		currentAbilityVisualEffect = abilityVisual;
	}
	void Update()
    {
		if(Time.time > lastAbilityTriggeredTime + abilityCooldown)
		{
			abilityTriggered = false;

			if(abilityAction.WasPressedThisFrame())
			{
				abilityTriggered = true;
				Vector3 playerPosition = transform.position;
				activeAbility.AbilityUse(playerPosition);

				currentAbilityVisualEffect.Play();

				AudioManager.instance.PlayOneShotAudio(abilitySound);

				lastAbilityTriggeredTime = Time.time;
			}
		}
		if(!abilityTriggered)
		{
			currentAbilityVisualEffect.Stop();
		}
    }
}

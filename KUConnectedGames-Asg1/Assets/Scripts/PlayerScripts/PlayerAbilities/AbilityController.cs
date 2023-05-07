using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public interface IAbilityController
{
	void AbilityUse(Vector3 playerPosition);
}

public class AbilityController : MonoBehaviourPunCallbacks
{
	private IAbilityController activeAbility;

	private PlayerInput _input;
    private InputAction abilityAction;

    [HideInInspector] public bool abilityTriggered;
	[SerializeField] AbilityType playerAbilityType;

	private float lastAbilityTriggeredTime;
	[SerializeField] float abilityCooldown;

	[SerializeField] ParticleSystem dashAbilityVisualEffect;
	[SerializeField] ParticleSystem impulseAbilityVisualEffect;
	ParticleSystem currentAbilityVisualEffect;

	private GameObject AbilityIcon;

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
			SetActiveAbilityVisual(impulseAbilityVisualEffect);
			abilitySound = AudioManager.AudioClipEnum.Impulse;
		}
		else if (playerAbilityType == AbilityType.Attacker)
		{
			Ability_AttackerDash attackerDash = gameObject.AddComponent<Ability_AttackerDash>();
			SetActiveAbility(attackerDash);
			SetActiveAbilityVisual(dashAbilityVisualEffect);
			abilitySound = AudioManager.AudioClipEnum.Dash;
		}

		if(currentAbilityVisualEffect != null) 
		{
			currentAbilityVisualEffect.Stop();
		}
		AbilityIcon = ReferenceManager.instance.AbilityIcon;
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

				if(currentAbilityVisualEffect != null) 
				{
					photonView.RPC("PlayAbilityVisualEffect", RpcTarget.AllBuffered);
				}

				AudioManager.instance.PlayOneShotAudio(abilitySound);

				lastAbilityTriggeredTime = Time.time;
			}
			AbilityIcon.SetActive(true);
		}
		else
		{
			photonView.RPC("StopAbilityVisualEffect", RpcTarget.All);
			AbilityIcon.SetActive(false);
		}
    }

	[PunRPC]
	private void PlayAbilityVisualEffect()
	{
		currentAbilityVisualEffect.Play();
	}

	[PunRPC]
	private void StopAbilityVisualEffect() 
	{
		currentAbilityVisualEffect.Stop();
	}
}

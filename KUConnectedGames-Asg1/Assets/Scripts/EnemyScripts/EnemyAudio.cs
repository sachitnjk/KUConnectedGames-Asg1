using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
	[Header("Enemy Audio")]
	[SerializeField] private AudioClip enemy_PatrolSound;
	[SerializeField] private AudioClip enemy_ChaseSound;
	[SerializeField] private AudioClip enemy_AttackSound;
	[SerializeField] private AudioClip enemy_HitSound;
	[SerializeField] private AudioClip enemy_DeathSound;

	AudioSource audioSource;

	Enemy_AiBehaviour enemy_AiBehaviour;
	Enemy_AiBehaviour.State currentState;

	private void Start()
	{
		enemy_AiBehaviour = GetComponent<Enemy_AiBehaviour>();
		currentState = enemy_AiBehaviour.GetCurrentState();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		PlayPatrolSound();
		PlayChaseSound();
		PlayAttackSound();
		PlayOnHitSound();
		PlayOnDeathSound();
	}

	private void PlayPatrolSound()
	{
		if(currentState == Enemy_AiBehaviour.State.Patrol)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = enemy_PatrolSound;
				audioSource.loop = true;
				audioSource.volume = 0.2f;
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}
	private void PlayChaseSound()
	{
		if (currentState == Enemy_AiBehaviour.State.Chase)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = enemy_ChaseSound;
				audioSource.loop = true;
				audioSource.volume = 0.25f;
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}


	private void PlayAttackSound()
	{
		if(currentState == Enemy_AiBehaviour.State.Attack)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = enemy_AttackSound;
				audioSource.loop = true;
				audioSource.volume = 0.35f;
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void PlayOnHitSound()
	{
		if (currentState == Enemy_AiBehaviour.State.IsHit)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = enemy_HitSound;
				audioSource.loop = true;
				audioSource.volume = 0.35f;
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void PlayOnDeathSound()
	{
		if(currentState == Enemy_AiBehaviour.State.Dead) 
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = enemy_DeathSound;
				audioSource.loop = true;
				audioSource.volume = 0.3f;
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}
}

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

	private AudioSource audioSource;
	private AudioClip currentClip;

	private Enemy_AiBehaviour enemy_AiBehaviour;

	private void Awake()
	{
		enemy_AiBehaviour = GetComponent<Enemy_AiBehaviour>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		PlaySound();
	}

	private void PlaySound()
	{
		AudioClip clipToPlay = null;

		if(enemy_AiBehaviour.GetCurrentState() == Enemy_AiBehaviour.State.Patrol)
		{
			clipToPlay = enemy_PatrolSound;
		}
		else if (enemy_AiBehaviour.GetCurrentState() == Enemy_AiBehaviour.State.Chase)
		{
			clipToPlay = enemy_ChaseSound;
		}

		if(clipToPlay != null && clipToPlay != currentClip) 
		{
			currentClip = clipToPlay;
			audioSource.Stop();
			audioSource.clip = clipToPlay;
			audioSource.volume = 0.7f;
			audioSource.loop = true;
			audioSource.Play();
		}
		else if (clipToPlay == null && currentClip != null) 
		{
			currentClip = null;
			audioSource.Stop();
		}
	}
}

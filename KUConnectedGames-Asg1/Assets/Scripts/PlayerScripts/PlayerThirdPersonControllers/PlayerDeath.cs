using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviourPunCallbacks
{
	ThirdPersonController _tpControllerScript;
	ThirdPersonShooter _tpShooterScript;
	GunScripts _gunScripts;
	PlayerHealthBar _tpHealthBar;
	Animator _animator;

	[SerializeField] private GameObject playerDeathPanel;

	private bool playerDeathActive;

	private void Start()
	{
		_tpControllerScript = GetComponent<ThirdPersonController>();
		_tpShooterScript = GetComponent<ThirdPersonShooter>();
		_gunScripts = GetComponentInChildren<GunScripts>();
		_tpHealthBar = GetComponentInChildren<PlayerHealthBar>();
		_animator = GetComponent<Animator>();

		playerDeathPanel.SetActive(false);

		playerDeathActive = false;
	}

	private void Update()
	{
		PlayerDeathTrigger();
	}

	private void PlayerDeathTrigger()
	{
		if (!playerDeathActive && _tpHealthBar != null && _tpHealthBar.currentHealth <= 1) 
		{
			playerDeathActive = true;
			_animator.SetTrigger("PlayerDeathTrigger");
			_tpControllerScript.enabled = false;
			_tpShooterScript.enabled = false;
			_gunScripts.enabled = false;
			_tpHealthBar.enabled = false;

			StartCoroutine(DeletePlayer());
		}
	}

	private IEnumerator DeletePlayer()
	{
		playerDeathPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		Debug.Log("Death deletion being called");
		if(photonView.IsMine)
		{
			PhotonNetwork.Destroy(this.gameObject);
			PhotonNetwork.LoadLevel(0);
		}

	}

	public bool GetPlayerDeathState()
	{
		return playerDeathActive;
	}


}

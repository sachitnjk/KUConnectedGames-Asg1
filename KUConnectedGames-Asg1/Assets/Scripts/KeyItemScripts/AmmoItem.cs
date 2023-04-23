using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
	[SerializeField] private int ammoInBox;

	[HideInInspector]public string playerTag = "Player";

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(playerTag))
		{
			GunScripts gunScript = other.GetComponentInChildren<GunScripts>();
			if(gunScript != null )
			{
				gunScript.AddMaxAmmo(ammoInBox);
			}
			Destroy(gameObject);
		}
	}
}

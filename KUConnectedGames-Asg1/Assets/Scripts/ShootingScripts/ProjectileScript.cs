using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Destroy(this.gameObject);
	}
}

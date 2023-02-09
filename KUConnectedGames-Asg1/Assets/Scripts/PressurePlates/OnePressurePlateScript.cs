using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePressurePlateScript : MonoBehaviour
{
	public bool plateOneIsActive = false;

	void OnTriggerEnter(Collider other)
	{
		plateOneIsActive = true;
		Debug.Log("Plate active");
	}
	void OnTriggerExit(Collider other)
	{
		plateOneIsActive=false;
		Debug.Log("Plate not active");
	}

}

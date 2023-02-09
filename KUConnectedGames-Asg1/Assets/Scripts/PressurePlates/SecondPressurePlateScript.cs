using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPressurePlateScript : MonoBehaviour
{
	public bool plateTwoIsActive = false;

	void OnTriggerEnter(Collider other)
	{
		plateTwoIsActive = true;
	}
	void OnTriggerExit(Collider other)
	{
		plateTwoIsActive = false;
	}
}

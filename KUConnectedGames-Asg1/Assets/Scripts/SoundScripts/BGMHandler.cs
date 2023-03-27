using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMHandler : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}

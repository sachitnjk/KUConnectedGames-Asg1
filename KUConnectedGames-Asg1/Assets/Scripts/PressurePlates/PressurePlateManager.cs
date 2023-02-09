using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{

    [SerializeField]private GameObject triggeredObject;
    [SerializeField] OnePressurePlateScript ppOne;
    [SerializeField] SecondPressurePlateScript ppTwo;

    void OnTriggerEnter(Collider other)
    {
		if (ppOne.plateOneIsActive && ppTwo.plateTwoIsActive)
        {
			triggeredObject.transform.position += new Vector3(0f, 5f, 0f);
            Debug.Log("Both pressed");
        }
    }

    void OnTriggerExit(Collider other)
    {
        triggeredObject.transform.position -= new Vector3(0f, 5f, 0f);
    }
}

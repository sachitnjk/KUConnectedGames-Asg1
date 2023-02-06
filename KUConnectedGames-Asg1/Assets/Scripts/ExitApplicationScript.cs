using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApplicationScript : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}

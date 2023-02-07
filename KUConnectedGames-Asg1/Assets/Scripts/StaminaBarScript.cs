using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{

    [SerializeField] Slider stamSlider;

    public void SetMaxStamina(int stamina)
    {

        stamSlider.maxValue = stamina;
        stamSlider.value = stamina;

    }

    public void SetStamina(int stamina)
    {

        stamSlider.value = stamina;

    }

}

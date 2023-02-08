using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{

    [SerializeField] Slider stamSlider;

    public void SetMaxStamina(float stamina)
    {

        stamSlider.maxValue = stamina;
        stamSlider.value = stamina;

    }

    public void SetStamina(float stamina)
    {

        stamSlider.value = stamina;

    }

}

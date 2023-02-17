using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{

    [SerializeField] Slider stamSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public void SetMaxStamina(float stamina)
    {

        stamSlider.maxValue = stamina;
        stamSlider.value = stamina;

        fill.color = gradient.Evaluate(1f);

    }

    public void SetStamina(float stamina)
    {
        stamSlider.value = stamina;

        fill.color = gradient.Evaluate(stamSlider.normalizedValue);

    }

}

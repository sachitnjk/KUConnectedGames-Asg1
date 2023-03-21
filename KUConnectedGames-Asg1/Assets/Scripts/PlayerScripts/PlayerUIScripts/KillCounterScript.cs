using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounterScript : MonoBehaviour
{
	[SerializeField] Slider killCounterSlider;

	public void SetMaxSliderCounter(int counter)
	{
		killCounterSlider.maxValue = counter;
		killCounterSlider.value = counter;
	}

	public void SetSliderCounter(int counter)
	{
		killCounterSlider.value = counter;
	}
}

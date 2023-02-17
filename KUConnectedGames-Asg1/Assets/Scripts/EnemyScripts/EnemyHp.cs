using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
	public Slider e_HpSlider;
	public Gradient e_Gradient;
	public Image e_HpFill;

	public void SetMaxHealth(int health)
	{
		e_HpSlider.maxValue = health;
		e_HpSlider.value = health;

		e_HpFill.color = e_Gradient.Evaluate(1f);
	}

	public void SetHealth(int health)
	{
		e_HpSlider.value = health;

		e_HpFill.color = e_Gradient.Evaluate(e_HpSlider.normalizedValue);
	}
}

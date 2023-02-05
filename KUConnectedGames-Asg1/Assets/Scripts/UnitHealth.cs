using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{

    [Header("Health Attributes")]
    int currentHealth;
    int currentMaxHealth;

    public int Health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int MaxHealth
    {
        get { return currentMaxHealth; }
        set { currentMaxHealth = value; }
    }


    //setting up the constructor

    public UnitHealth(int health, int maxHealth)
    {

        currentHealth = health;
        currentMaxHealth = maxHealth;   

    }

    public void DamageUnitHealth(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }
    }

	public void HealUnitHealth(int healAmount)
	{
		if (currentHealth > currentMaxHealth)
		{
			currentHealth += healAmount;
		}

        if(currentHealth > currentMaxHealth)
        {
            currentHealth = (int)Mathf.Clamp(currentHealth, 0f, currentMaxHealth);
            //currentHealth = currentMaxHealth;
        }
	}

}

using Photon.Pun;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealthBar : MonoBehaviourPunCallbacks
{

    [HideInInspector]public int currentHealth;
    public int maxHealth= 100;

    public HealthBarScript healthBar;


    void Start()
    {
		currentHealth = maxHealth;
        healthBar?.SetMaxHealth(maxHealth);     //run if not null
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("player taking damage");
        currentHealth -= damage;
        healthBar?.SetHealth(currentHealth);
    }
}

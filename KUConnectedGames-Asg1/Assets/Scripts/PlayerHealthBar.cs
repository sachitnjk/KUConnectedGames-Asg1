using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealthBar : MonoBehaviour
{
    private PlayerInput playerInput;

    public int maxHealth= 100;
    public int currentHealth;

    public HealthBarScript healthBar;

    InputAction damageTaken;

    void Start()
    {
		playerInput = gameObject.GetComponent<PlayerInput>();
        damageTaken = playerInput.actions["DamageTaken"];
		currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        
        if(damageTaken.triggered)
        {
            TakeDamage(20);
            Debug.Log(currentHealth);
        }

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

}

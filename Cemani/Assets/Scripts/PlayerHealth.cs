using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum health of the player
    public int currentHealth;  // Current health of the player
    public Image healthBar;  // UI health bar to display player's health
    public GameOverMenu gameOverMenu;  // Reference to the GameOverMenu script

    void Start()
    {
        currentHealth = maxHealth;  // Set current health to max health at the start of the game
        UpdateHealthBar();  // Update the UI health bar
    }

    public void TakeDamage(int damage)
    {
        PlayerMove playerMove = GetComponent<PlayerMove>();

        // Check if the player is rolling
        if (playerMove.isRolling)
        {
            return;  // If yes, do not take damage
        }

        currentHealth -= damage;  // Reduce current health by the damage taken
        if (currentHealth < 0)
        {
            currentHealth = 0;  // Ensure health does not go below 0
        }
        UpdateHealthBar();  // Update the UI health bar

        if (currentHealth == 0)
        {
            // Handle player death
            HandleDeath();
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;  // Ensure health does not exceed maximum
        }
        UpdateHealthBar();  // Update the UI health bar
    }

    private void UpdateHealthBar()
    {
        // Update the fill amount of the UI health bar based on the current health percentage
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    private void HandleDeath()
    {
        gameOverMenu.ShowGameOver();  // Call the instance method
        // Optionally, disable player controls or other components here
        GetComponent<PlayerMove>().enabled = false;
        // Optionally, display other game over effects here
    }
}

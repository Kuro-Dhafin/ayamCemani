using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Health maksimum pemain
    public int currentHealth;  // Health saat ini pemain
    public Image healthBar;  // UI health bar untuk menampilkan health pemain

    void Start()
    {
        currentHealth = maxHealth;  // Mengatur health saat ini ke health maksimum saat game dimulai
        UpdateHealthBar();  // Memperbarui UI health bar
    }

    public void TakeDamage(int damage)
    {
        PlayerMove playerMove = GetComponent<PlayerMove>();

        // Cek apakah pemain sedang melakukan roll
        if (playerMove.isRolling)
        {
            return;  // Jika ya, jangan tidak menerima damage
        }

        currentHealth -= damage;  // Mengurangi health saat ini sebesar damage yang diterima
        if (currentHealth < 0)
        {
            currentHealth = 0;  // Memastikan health tidak kurang dari 0
        }
        UpdateHealthBar();  // Memperbarui UI health bar

        if (currentHealth == 0)
        {
            // Menangani kematian pemain, misalnya menghancurkan objek pemain
            Destroy(gameObject);
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;  // health tidak melebihi maksimum
        }
        UpdateHealthBar();  // Memperbarui UI health bar
    }

    private void UpdateHealthBar()
    {
        // Memperbarui fill amount dari UI health bar berdasarkan persentase health saat ini
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}

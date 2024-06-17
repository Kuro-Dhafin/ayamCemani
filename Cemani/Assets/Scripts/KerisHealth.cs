using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KerisHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Health maksimum bos
    public int currentHealth;  // Health saat ini bos
    public Image healthBar;  // UI health bar untuk menampilkan health bos

    void Start()
    {
        currentHealth = maxHealth;  // Mengatur health saat ini ke health maksimum saat game dimulai
        UpdateHealthBar();  // Memperbarui UI health bar
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Mengurangi health saat ini sebesar damage yang diterima
        if (currentHealth < 0)
        {
            currentHealth = 0;  // Memastikan health tidak kurang dari 0
        }
        UpdateHealthBar();  // Memperbarui UI health bar

        if (currentHealth == 0)
        {
            // Menangani kematian bos, misalnya menghancurkan objek bos
            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        // Memperbarui fill amount dari UI health bar berdasarkan persentase health saat ini
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}

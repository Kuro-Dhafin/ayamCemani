using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHealt : MonoBehaviour
{
    [SerializeField]
    private int healthAmount = 10;  // Jumlah health yang ditambahkan

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthAmount);  // Tambah health ke pemain
                Destroy(gameObject);  // Hancurkan item health setelah collect
            }
        }
    }
}

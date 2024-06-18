using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keris : MonoBehaviour
{
    private KerisHealth kerisHealth;

    private void Start()
    {
        kerisHealth = GetComponent<KerisHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika musuh bertabrakan dengan peluru pemain, musuh akan hancur / destroy
        if (collision.gameObject.CompareTag("Bullet"))
        {
            kerisHealth.TakeDamage(10);  // Mengurangi health pemain sebesar 10
            Destroy(collision.gameObject);  // matikan / destroy peluru musuh
        }
    }
}

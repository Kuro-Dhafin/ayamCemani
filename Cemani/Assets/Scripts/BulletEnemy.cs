using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField]
    // Kecepatan peluru
    private float speed = 10f;

    [Range(1, 10)]
    [SerializeField]
    
    private float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        // Mendapatkan komponen Rigidbody2D dari GameObject
        rb = GetComponent<Rigidbody2D>();

        // Menghancurkan GameObject peluru 
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        // Mengatur kecepatan peluru berdasarkan arah dan kecepatan yang ditentukan
        rb.velocity = transform.right * speed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);  // Debug log untuk memeriksa tabrakan

        // Jika peluru bertabrakan dengan musuh, musuh akan dihancurkan
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
           // Debug.Log("Hit Enemy: " + collision.gameObject.name);  // Debug log untuk memeriksa tabrakan dengan musuh
            //Destroy(collision.gameObject);  // Menghancurkan musuh
            //Destroy(gameObject);  // Menghancurkan peluru
        //}

        // Jika peluru bertabrakan dengan pemain, kurangi health pemain
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);  // Mengurangi health pemain sebesar 10
            }
            Destroy(gameObject);  // Menghancurkan peluru
        }
    }
}

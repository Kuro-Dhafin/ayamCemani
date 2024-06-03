using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField]
    // Kecepatan peluru
    private float speed = 10f;

    [Range(1, 10)]
    [SerializeField]
    
    private float lifeTime = 3f;

    public bool isPlayerBullet;

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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);  // Debug log untuk memeriksa collicion

        // Jika peluru bertabrakan dengan musuh, musuh akan mati / destroy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy: " + collision.gameObject.name);  // Debug log untuk memeriksa collision dengan musuh
            Destroy(collision.gameObject);  // destroy musuh
            Destroy(gameObject);  // destroy peluru
        }
    }

}

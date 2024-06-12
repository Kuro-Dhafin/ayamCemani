using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField]
    private float speed = 10f;

    [Range(1, 10)]
    [SerializeField]
    private float lifeTime = 3f;

    private Rigidbody2D rb;
    private bool isPaused = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the bullet when paused
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void SetPauseState(bool paused)
    {
        isPaused = paused;
    }
}

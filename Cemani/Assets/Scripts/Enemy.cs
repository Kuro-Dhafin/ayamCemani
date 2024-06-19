using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;  // Kecepatan gerakan musuh

    [SerializeField]
    private float chaseRange = 5.0f;  // Jarak di mana musuh mulai mengejar pemain

    [SerializeField]
    private float attackRange = 3.0f;  // Jarak di mana musuh mulai menembak pemain

    [SerializeField]
    private GameObject bulletPrefab;  // Prefab peluru musuh

    [SerializeField]
    private Transform firePoint;  // Titik tembak peluru

    

    private Transform player;  // ref ke transform pemain
    private Rigidbody2D rb;  
    private bool facingRight = true;  // Menyimpan arah hadap musuh
    private float fireCooldown = 1.0f;  // Waktu jeda antara tembakan
    private float lastFireTime;  // Waktu akhir musuh menembak
    private Animator animator;  // ref ke komponen Animator
    private StageManager stageManager;
    private AudioManager audioManager;  // Reference to AudioManager
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Mendapatkan komponen Rigidbody2D dari GameObject
        animator = GetComponent<Animator>();  // Mendapatkan komponen Animator dari GameObject
        stageManager = FindObjectOfType<StageManager>();
        if (stageManager != null)
        {
            stageManager.RegisterEnemy(gameObject);
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;  // Mencari pemain berdasarkan tag
        }

        audioManager = AudioManager.instance;  // Get the instance of AudioManager
    }

    void Update()
    {
        if (player == null) return;  // Jika pemain sudah dihancurkan, hentikan fungsi

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isMoving = false;

        if (distanceToPlayer < chaseRange)
        {
            // Menghitung arah ke pemain
            Vector2 direction = (player.position - transform.position).normalized;

            // Menggerakkan musuh ke arah pemain
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

            isMoving = true;

            // Mengubah arah hadap musuh berdasarkan posisi pemain
            if (player.position.x < transform.position.x && facingRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingRight = false;
            }
            else if (player.position.x > transform.position.x && !facingRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingRight = true;
            }

            // Jika jarak ke pemain kurang dari attackRange, musuh akan menembak pemain
            if (distanceToPlayer < attackRange && Time.time > lastFireTime + fireCooldown)
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }

        animator.SetBool("IsMoving", isMoving);
    }

    private void Shoot()
    {
        if (player == null) return;  // Jika pemain sudah dihancurkan, hentikan fungsi

        // Menghitung sudut untuk peluru berdasarkan posisi pemain
        Vector2 shootingDirection = player.position - firePoint.position;
        float shootingAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, shootingAngle);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        bullet.tag = "EnemyBullet";  // Set tag peluru musuh
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika musuh bertabrakan dengan peluru pemain, musuh akan hancur / destroy
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //audioManager.PlayHurt(audioManager.hurt);  // Play hurt sound effect
            Destroy(gameObject);
           
        }
    }

    void OnDestroy()
    {
        if (stageManager != null)
        {
            stageManager.UnregisterEnemy(gameObject);
        }
    }

    


}
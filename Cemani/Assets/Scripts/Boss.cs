using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;  // Kecepatan gerakan bos

    [SerializeField]
    private float chaseRange = 5.0f;  // Jarak di mana bos mulai mengejar pemain

    [SerializeField]
    private float attackRange = 3.0f;  // Jarak di mana bos mulai menembak pemain

    [SerializeField]
    private GameObject bulletPrefab;  // Prefab peluru musuh

    [SerializeField]
    private Transform firePoint;  // Titik tembak peluru

    [SerializeField]
    private GameObject enemyPrefab;  // Prefab musuh

    [SerializeField]
    private Transform[] spawnPoints;  // Titik spawn kroco

    [SerializeField]
    private float attackInterval = 5.0f;  // Interval waktu antara serangan

    [SerializeField]
    private float bulletSpeed = 10.0f;  // Kecepatan peluru yang ditembakkan oleh bos

    [SerializeField]
    private int shotsPerCycle = 3;  // Jumlah tembakan per siklus sebelum memanggil kroco

    private int currentShotCount = 0;  // Jumlah tembakan saat ini

    [SerializeField]
    private Transform[] teleportLocations;

    private Vector3 originalScale;
    private BossHealth bossHealth;

    private Transform player;  // ref ke transform pemain
    private Rigidbody2D rb;
    private bool facingRight = true;  // Menyimpan arah hadap musuh
    private float fireCooldown = 1.0f;  // Waktu jeda antara tembakan
    private float lastFireTime;  // Waktu akhir musuh menembak
    private Animator animator;  // ref ke komponen Animator
    private bool isSpawningEnemies = false;  // Status apakah bos sedang memanggil musuh
    private float nextAttackTime;  // Waktu untuk serangan berikutnya

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Mendapatkan komponen Rigidbody2D dari GameObject
        bossHealth = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();  // Mendapatkan komponen Animator dari GameObject
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;  // Mencari pemain berdasarkan tag
        }
        originalScale = transform.localScale;  // Simpan scale asli
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();  // Memastikan bos selalu menghadap ke pemain

        if (Time.time >= nextAttackTime)
        {
            if (isSpawningEnemies)
            {
                SpawnEnemies();
                isSpawningEnemies = false;
                currentShotCount = 0;  // Reset jumlah tembakan setelah memanggil musuh
            }
            else
            {
                //Shoot();
                StartCoroutine(ShootRoutine());
                currentShotCount++;
                if (currentShotCount >= shotsPerCycle)
                {
                    isSpawningEnemies = true;
                    //Teleport();
                    StartCoroutine(ApproachAndTeleport());
                }
            }
            nextAttackTime = Time.time + attackInterval;
        }

        
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            // Periksa apakah pemain berada di sisi kanan atau kiri bos
            bool playerIsRight = player.position.x > transform.position.x;
            // Balik arah hadap bos berdasarkan posisi pemain
            transform.localScale = new Vector3(playerIsRight ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    private IEnumerator ApproachAndTeleport()
    {
        float approachTime = 1.0f;  // Durasi bos bergerak mendekati pemain
        float startTime = Time.time;

        animator.SetBool("IsWalking", true);

        while (Time.time < startTime + approachTime)
        {
            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            yield return null;
        }

        animator.SetBool("IsWalking", false);

        Teleport();
    }

    private void Teleport()
    {
        if (teleportLocations.Length > 0)
        {
            int index = Random.Range(0, teleportLocations.Length);
            transform.position = teleportLocations[index].position;
        }
    }

    private IEnumerator ShootRoutine()
    {
        animator.SetBool("IsShooting", true);  // Set animasi speak

        // jeda sebelum mulai menembak untuk memastikan animasi speak dimulai
        yield return new WaitForSeconds(0.1f);

        Shoot();

        // jda setelah menembak untuk memastikan animasi speak selesai
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsShooting", false);  // Kembali ke animasi idle
    }

    private void Shoot()
    {
        if (player == null) return;  // Jika pemain sudah dihancurkan, hentikan fungsi

        int bulletCount = 5;  // Jumlah peluru yang ditembakkan
        float spreadAngle = 45f;  // Sudut penyebaran

        for (int i = 0; i < bulletCount; i++)
        {
            // Menghitung sudut untuk setiap peluru
            float angleStep = spreadAngle / (bulletCount - 1);
            float angle = -spreadAngle / 2 + angleStep * i;

            // Menghitung arah tembakan berdasarkan sudut
            Vector2 shootingDirection = (player.position - firePoint.position).normalized;
            shootingDirection = Quaternion.Euler(0, 0, angle) * shootingDirection;
            float shootingAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, shootingAngle);

            // Membuat instance peluru di titik tembak dengan rotasi yang dihitung
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            bullet.tag = "EnemyBullet";  // Set tag peluru musuh

            // Menambahkan kecepatan ke peluru
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.isKinematic = true; 
                bulletRb.velocity = shootingDirection * bulletSpeed;  // Atur kecepatan peluru
            }
        }
        // Memicu animasi speak
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }

    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika musuh bertabrakan dengan peluru pemain, musuh akan hancur / destroy
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bossHealth.TakeDamage(10);  // Mengurangi health pemain sebesar 10
            Destroy(collision.gameObject);  // matikan / destroy peluru musuh
        }
    }
}

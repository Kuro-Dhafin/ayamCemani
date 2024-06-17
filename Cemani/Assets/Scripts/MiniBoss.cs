using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;  // Kecepatan gerakan mini bos

    [SerializeField]
    private float verticalRange = 3.0f;  // Jarak gerakan vertikal

    [SerializeField]
    private float horizontalRange = 2.0f;  // Jarak gerakan horizontal

    [SerializeField]
    private GameObject bulletPrefab;  // Prefab peluru mini bos

    [SerializeField]
    private Transform firePoint;  // Titik tembak peluru

    [SerializeField]
    private GameObject spawnPrefab;  // Prefab spawn mini bos

    [SerializeField]
    private float fireCooldown = 1.0f;  // Waktu jeda tembakan

    [SerializeField]
    private float spawnInterval = 10.0f;  // Interval waktu untuk spawn

    [SerializeField]
    private float spawnSpacing = 1.0f;  // Jarak antara spawn

    private Rigidbody2D rb;

    private Transform player;  // ref ke transform pemain
    private float lastFireTime;  // Waktu akhir mini bos menembak
    private float lastSpawnTime;  // Waktu akhir mini bos menspawn
    private Vector3 originalPosition;  // Posisi asli mini bos
    private int moveStage = 0;  // Tahap gerakan
    private bool movingUp = true;  // Arah gerakan vertikal
    private SepuhHealth sepuhHealth;

    private List<GameObject> currentSpawns = new List<GameObject>();  // Daftar spawn
    private bool isSpawn = false;  // cek Apakah spawn atau mini bos asli

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sepuhHealth = GetComponent<SepuhHealth>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;  // Mencari pemain berdasarkan tag
        }
        originalPosition = transform.position;  // Simpan posisi
    }

    void Update()
    {
        if (player == null) return;  // Jika pemain sudah dihancurkan, hentikan fungsi

        MovePattern();  // Gerakan pola mini bos
        ShootAtPlayer();  // Tembakan ke pemain
        if (!isSpawn) SpawnAndShoot();  // Spawn dan tembakan hanya untuk mini bos asli
    }

    //pola gerakan mini bos
    private void MovePattern()
    {
        switch (moveStage)
        {
            case 0:
                // Gerak maju
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (transform.position.x >= originalPosition.x + horizontalRange)
                {
                    moveStage = 1;
                }
                break;
            case 1:
                // Gerak ke atas
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y >= originalPosition.y + verticalRange)
                {
                    moveStage = 2;
                }
                break;
            case 2:
                // Gerak mundur
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (transform.position.x <= originalPosition.x)
                {
                    moveStage = 3;
                }
                break;
            case 3:
                // Gerak ke bawah
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y <= originalPosition.y)
                {
                    moveStage = 0;
                }
                break;
        }
    }

    private void ShootAtPlayer()
    {
        if (Time.time > lastFireTime + fireCooldown)
        {
            if (player != null)
            {
                // Menghitung sudut untuk peluru berdasarkan posisi pemain
                Vector2 shootingDirection = player.position - firePoint.position;
                float shootingAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
                Quaternion bulletRotation = Quaternion.Euler(0, 0, shootingAngle);

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
                bullet.tag = "EnemyBullet";  // Set tag peluru musuh
            }
            lastFireTime = Time.time;
        }
    }

    private void SpawnAndShoot()
    {
        // Hanya spawn jika spawn sebelumnya telah mati
        if (Time.time > lastSpawnTime + spawnInterval && currentSpawns.Count == 0)
        {
            StartCoroutine(SpawnRoutine());
            lastSpawnTime = Time.time;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < 1; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(1.0f, i * spawnSpacing, 0);  // Posisi spawn di depan mini bos dan berbaris ke atas
            GameObject spawn = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            spawn.GetComponent<MiniBoss>().isSpawn = true;  // Tandai sebagai spawn
            spawn.GetComponent<MiniBoss>().SetParent(this);  // Set parent untuk spawn
            currentSpawns.Add(spawn);
        }
        yield return new WaitForSeconds(1.0f);  // Waktu jeda sebelum spawn mulai menembak
    }

    public void SetParent(MiniBoss parent)
    {
        this.transform.parent = parent.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika musuh bertabrakan dengan peluru pemain, musuh akan hancur / destroy
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (isSpawn)
            {
                Destroy(gameObject);  // Hancurkan spawn langsung
            }
            else
            {
                sepuhHealth.TakeDamage(10);  // Mengurangi health pemain sebesar 10
            }
            Destroy(collision.gameObject);  // matikan / destroy peluru musuh
        }
    }

    void OnDestroy()
    {
        if (isSpawn)
        {
            if (transform.parent != null)
            {
                MiniBoss parentBoss = transform.parent.GetComponent<MiniBoss>();
                if (parentBoss != null)
                {
                    parentBoss.currentSpawns.Remove(gameObject);  // Hapus spawn dari mini bos asli
                }
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefabs;  // Prefab peluru yang akan ditembakkan oleh pemain
    [SerializeField] private Transform firePoint;  // Titik tembak peluru

    [SerializeField] private float moveSpeed = 5.0f;  // Kecepatan gerakan pemain

    //test dash
    [SerializeField] private float dashSpeed = 10.0f;  // Kecepatan dash pemain
    [SerializeField] private float dashDuration = 0.2f;  // Durasi dash pemain
    private bool isDashing = false;  // Status apakah pemain sedang dash
    private float dashTime;  // Waktu tersisa untuk dash
    private Collider2D playerCollider;
    private Vector3 originalScale;

    public float rollBoost = 2f;
    private float rollTime;
    public float RollTime;
    bool rollOnce = false;

    public bool isRolling = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;  // Input gerakan dari pemain
    private Vector2 mousePos;  // Posisi mouse di dunia game
    private PlayerHealth playerHealth;  // Referensi ke komponen PlayerHealth
    private Animator animator;  // Referensi ke komponen Animator

    private bool isPaused = false;  // Status apakah permainan sedang dijeda

    private int originalLayer;  // Store the original layer of the player

    private AudioManager audioManager;  // Reference to AudioManager
    private bool isWalking = false;  // Flag to track if walking sound is playing
    private Coroutine walkingSoundCoroutine;  // Coroutine reference for delaying walking sound

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // Mendapatkan komponen Rigidbody2D dari GameObject
        playerHealth = GetComponent<PlayerHealth>();  // Mendapatkan komponen PlayerHealth dari GameObject
        animator = GetComponent<Animator>();  // Mendapatkan komponen Animator dari GameObject
        playerCollider = GetComponent<Collider2D>();
        originalLayer = gameObject.layer;  // Store the original layer
        audioManager = AudioManager.instance;  // Get the instance of AudioManager
    }

    void Update()
    {
        // Jangan lakukan apa-apa jika permainan dijeda
        if (isPaused) return;

        // Mendapatkan input gerakan dari pemain
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();  // Menormalisasi input gerakan untuk menjaga kecepatan konstan di semua arah

        // Mengurangi rollTime terus menerus
        if (rollTime > 0)
        {
            rollTime -= Time.deltaTime;
        }

        // Mengurangi dashTime terus menerus
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
        }
        else
        {
            isDashing = false;  // Menghentikan dash jika waktu habis
        }

        // Mengaktifkan roll jika tombol spasi ditekan dan rollTime habis
        if (Input.GetKey(KeyCode.Space) && rollTime <= 0 && !rollOnce)
        {
            animator.SetBool("Roll", true);
            moveSpeed += rollBoost;
            rollTime = RollTime;
            rollOnce = true;
            isRolling = true;
            isDashing = true;  // Aktifkan dash
            dashTime = dashDuration;  // Set durasi dash
            gameObject.layer = LayerMask.NameToLayer("RollingPlayer");  // Change the layer
        }

        // Menonaktifkan roll jika waktu habis
        if (rollTime <= 0 && rollOnce)
        {
            animator.SetBool("Roll", false);
            moveSpeed -= rollBoost;
            rollOnce = false;
            isRolling = false;
            gameObject.layer = originalLayer;  // Revert the layer
        }

        // Mengatur parameter isMoving di Animator berdasarkan input gerakan
        bool IsMoving = moveInput != Vector2.zero;
        animator.SetBool("IsMoving", IsMoving);

        // Play the walking sound when moving
        if (IsMoving && !isWalking)
        {
            if (walkingSoundCoroutine != null)
                StopCoroutine(walkingSoundCoroutine);  // Stop previous coroutine if running

            walkingSoundCoroutine = StartCoroutine(DelayedWalkingSound());
        }
        else if (!IsMoving)
        {
            isWalking = false;
        }

        // Mendapatkan posisi mouse 
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Membalikkan sprite pemain berdasarkan arah gerakan
        if (moveInput.x != 0)
        {
            float localScaleX = Mathf.Abs(transform.localScale.x); // Mengambil nilai absolut dari skala X untuk mempertahankan ukuran
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * localScaleX, transform.localScale.y, transform.localScale.z);
        }

        // Menembak peluru saat tombol mouse kiri ditekan
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            audioManager.PlayShoot();
        }
    }

    void FixedUpdate()
    {
        // Jangan lakukan apa-apa jika permainan dijeda
        if (isPaused) return;

        if (isDashing)
        {
            // Menggerakkan pemain dengan kecepatan dash
            rb.velocity = moveInput * dashSpeed;
        }
        else
        {
            // Menggerakkan pemain dengan kecepatan normal
            rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        }

        // Menghitung arah pandang berdasarkan posisi mouse
        Vector2 lookDir = mousePos - rb.position;
    }

    private void Shoot()
    {
        // Menghitung arah tembakan berdasarkan posisi mouse
        Vector2 shootingDirection = mousePos - (Vector2)firePoint.position;
        float shootingAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, shootingAngle);

        // Membuat instance peluru di titik tembak dengan rotasi
        Instantiate(bulletPrefabs, firePoint.position, bulletRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jika pemain bertabrakan dengan peluru musuh, kurangi health pemain
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            playerHealth.TakeDamage(10);  // Mengurangi health pemain sebesar 10
            Destroy(collision.gameObject);  // matikan / destroy peluru musuh
        }
    }

    // Method to set the pause state
    public void SetPause(bool pause)
    {
        isPaused = pause;
        rb.velocity = Vector2.zero;  // Stop player movement when paused
    }

    // Coroutine to play walking sound after a delay
    private IEnumerator DelayedWalkingSound()
    {
        isWalking = true;  // Set walking flag to true
        audioManager.PlayWalk();
        yield return new WaitForSeconds(0.3f);  // Adjust delay time as needed
    }
}

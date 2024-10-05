using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    // Speed of the spaceship movement
    public float moveSpeed = 3f;  // Keeps it slow

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Smooth interpolation factor (very slight smoothing)
    public float smoothingFactor = 0.1f; // Slight smoothing, no acceleration over time

    // Target velocity for smooth movement
    private Vector2 targetVelocity;

    void Start()
    {
        // Get the Rigidbody2D component attached to the spaceship
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        // Get input for horizontal and vertical movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Calculate target velocity based on input and movement speed
        targetVelocity = new Vector2(moveX, moveY).normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // Interpolate slightly towards target velocity (no buildup or over-speeding)
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, smoothingFactor);

        // Ensure the velocity does not exceed the moveSpeed
        if (rb.velocity.magnitude > moveSpeed)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);

            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}

using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    public int damage = 1; // Damage the bullet deals
    public float lifetime = 5f; // Lifetime before the bullet is destroyed

    private Rigidbody rb; // Rigidbody reference

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Apply velocity to the bullet
            rb.velocity = transform.forward * speed;
        }

        // Destroy the bullet after a set time
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits the player
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
            }

            // Destroy the bullet on collision
            Destroy(gameObject);
        }
    }
}


using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 3.0f;  // Speed at which the enemy moves towards the player
    public float stoppingDistance = 2.0f;  // Distance to stop the enemy from the player
    public float shootingRange = 10.0f;  // Max distance at which the enemy can shoot
    public float shootCooldown = 1.0f;  // Time between shots
    public float damage = 10.0f;  // Amount of damage dealt to the player
    public float rotationSpeed = 5.0f;  // Speed at which the enemy rotates towards the player

    public ParticleSystem muzzleFlash;  // Reference to the muzzle flash particle system
    private Transform player;  // Player's transform
    private float timeSinceLastShot = 0f;  // Time elapsed since the last shot

    private void Start()
    {
        // Find the player GameObject by its tag and get its transform
        player = GameObject.FindWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        // Make sure player and muzzle flash are assigned
        if (player != null && muzzleFlash != null)
        {
            // Rotate the enemy to face the player
            RotateTowardsPlayer();

            // Move towards the player
            FollowPlayer();

            // Shoot at the player if within range
            ShootAtPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;  // Keep rotation only on the Y-axis (horizontal plane)

        // If the player is not at the same position, rotate towards the player
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FollowPlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = player.position - transform.position;

        // Move towards the player if farther than stopping distance
        if (direction.magnitude > stoppingDistance)
        {
            direction.Normalize();  // Normalize direction to prevent faster movement over distance
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void ShootAtPlayer()
    {
        // Calculate direction from the enemy to the player
        Vector3 direction = player.position - transform.position;

        // If the player is within shooting range and cooldown is over, shoot
        if (direction.magnitude <= shootingRange && timeSinceLastShot >= shootCooldown)
        {
            // Fire a ray from the enemy to the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, shootingRange))
            {
                // Check if the ray hits the player
                if (hit.transform == player)
                {
                    Debug.Log("Player hit!");
                    // You can implement player damage here, for example:
                    // player.GetComponent<PlayerHealth>().TakeDamage(damage);

                    // Play muzzle flash
                    muzzleFlash.Play();
                }
            }

            // Reset cooldown
            timeSinceLastShot = 0f;
        }
        else
        {
            // Increment the cooldown timer
            timeSinceLastShot += Time.deltaTime;
        }
    }
}

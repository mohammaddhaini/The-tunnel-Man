using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public AudioSource warningAudio; // Audio source to play warning sound
    public Light warningLight; // Spotlight to flash as a warning
    public Animator doorAnimator; // Animator for the door animation
    public CharacterController player; // Reference to the CharacterController

    public CountdownHandler countdownHandler; // Reference to the CountdownHandler script
    public float lightFlashDuration = 2f; // Time for the light to flash
    public float lightFlashInterval = 0.2f; // Interval between light flashes
    public float minSpawnTime = 180f; // Minimum spawn time
    public float maxSpawnTime = 300f; // Maximum spawn time

    private float spawnTimer; // Timer for the next spawn
    private bool isSpawning = false; // Ensures one spawn at a time

    void Start()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime); // Set the initial spawn timer to a random time within the range
        if (countdownHandler != null)
        {
            countdownHandler.OnCountdownEnd += HandleCountdownEnd; // Subscribe to countdown complete event
        }
    }

    void Update()
    {
        if (!isSpawning)
        {
            spawnTimer -= Time.deltaTime; // Decrease the spawn timer

            // Start countdown when there are 30 seconds left before the next spawn
            if (spawnTimer <= 30f && spawnTimer > 0f && countdownHandler != null)
            {
                isSpawning = true;

                countdownHandler.StartCountdown(); // Start the countdown
                StartCoroutine(StartWarningSoundAndLight()); // Start the warning sound and light
            }
        }

        // Teleport player to spawn point if on the "Player" layer
        if (player != null && player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Select a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Teleport the player by directly setting the position
     //       player.enabled = false; // Disable CharacterController to modify position directly
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation; // Optional: Reset rotation
         //   player.enabled = true; // Re-enable CharacterController

            // Debugging: Log the player's new position
            Debug.Log("Player teleported to spawn point: " + player.transform.position);
        }

    }

    // Coroutine to handle warning sound and light during countdown
    private IEnumerator StartWarningSoundAndLight()
    {
        if (warningAudio != null)
        {
            warningAudio.Play(); // Play warning sound
        }

        if (warningLight != null)
        {
            StartCoroutine(FlashWarningLight()); // Start flashing the light
        }

        yield return null; // Return immediately to continue with countdown logic
    }

    private IEnumerator FlashWarningLight()
    {
        float elapsedTime = 0f;
        bool isLightOn = false;

        while (elapsedTime < lightFlashDuration)
        {
            isLightOn = !isLightOn;
            warningLight.enabled = isLightOn;

            yield return new WaitForSeconds(lightFlashInterval);
            elapsedTime += lightFlashInterval;
        }

        warningLight.enabled = false; // Turn off the warning light after flashing
    }

    private void HandleCountdownEnd()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        // Trigger door animation (if any)
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("Close", true);
        }

        // Wait a small amount of time to sync with the countdown end
        yield return new WaitForSeconds(0.5f); // Small delay before spawning the enemy

        // Spawn the enemy at a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Reset spawn timer for the next spawn (random time between min and max spawn time)
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);

        isSpawning = false; // Allow spawning again after this one
    }
}

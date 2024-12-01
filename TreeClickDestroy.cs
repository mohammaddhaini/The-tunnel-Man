using System.Collections;
using UnityEngine;

public class TreeClickDestroy : MonoBehaviour
{
    private int clickCount = 0; // Counter for the number of clicks
    private Rigidbody treeRigidbody; // Reference to the Rigidbody component
    private bool isFalling = false; // To check if the tree has started falling
    private bool canBeHit = true; // To prevent consecutive hits
    private Vector3 initialPosition; // Save the initial position of the tree
    private Quaternion initialRotation; // Save the initial rotation of the tree

    [Header("Settings")]
    public GameObject trunkPrefab; // Single prefab for the trunk logs
    public float logSpacing = 1.0f; // Distance between each log segment along the length
    public int logsToSpawn = 3; // Number of logs to spawn
    public float fallForce = 3f; // Initial force to start the tree falling
    public float initialTorque = 5f; // Torque to initiate rotation
    public float destroyDelay = 2f; // Time before destroying the tree and spawning logs
    public float resetTime = 5f; // Time to reset the tree after falling
    public float hitCooldown = 0.5f; // Time between hits

    [Header("Audio")]
    public AudioSource audioSource; // Audio source to play sounds
    public AudioClip hitSound; // Sound played on each hit
    public AudioClip fallSound; // Sound played when the tree falls

    void Start()
    {
        // Get the Rigidbody component attached to the tree
        treeRigidbody = GetComponent<Rigidbody>();
        treeRigidbody.isKinematic = true; // Ensure the tree doesn't fall at start

        // Save the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void OnMouseDown()
    {
        if (!isFalling && canBeHit && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(HandleHit());
        }
    }

    private IEnumerator HandleHit()
    {
        // Play the hit sound
        if (audioSource && hitSound)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Increment the click count
        clickCount++;
        canBeHit = false; // Prevent consecutive hits

        if (clickCount >= 5)
        {
            StartTreeFall();
        }

        // Wait for the cooldown duration before allowing another hit
        yield return new WaitForSeconds(hitCooldown);
        canBeHit = true;
    }

    void StartTreeFall()
    {
        // Enable physics by making Rigidbody non-kinematic
        treeRigidbody.isKinematic = false;

        // Play the fall sound
        if (audioSource && fallSound)
        {
            audioSource.PlayOneShot(fallSound);
        }

        // Apply a small horizontal force to initiate tipping
        Vector3 force = transform.forward * fallForce;
        treeRigidbody.AddForceAtPosition(force, transform.position, ForceMode.Impulse);

        // Apply initial torque to start rotation
        treeRigidbody.AddTorque(Vector3.up * initialTorque, ForceMode.Impulse);

        // Mark that the tree has started falling
        isFalling = true;

        // Schedule tree destruction and log spawning
        Invoke(nameof(DestroyTreeAndSpawnLogs), destroyDelay);
    }

    void DestroyTreeAndSpawnLogs()
    {
        // Capture the final position and rotation of the tree
        Vector3 finalPosition = transform.position;
        Quaternion finalRotation = transform.rotation;

        // Destroy the tree object
        gameObject.SetActive(false); // Disable instead of destroy to allow for reset

        // Spawn logs
        for (int i = 0; i < logsToSpawn; i++)
        {
            Vector3 logPosition = finalPosition - transform.up * logSpacing * i; // Adjust position along the tree's length
            Instantiate(trunkPrefab, logPosition, finalRotation); // Spawn each log with the tree's final orientation
        }

        // Schedule tree reset
        Invoke(nameof(ResetTree), resetTime);
    }

void ResetTree()
    {
        // Reset the tree's position, rotation, and state
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true); // Re-enable the tree
        treeRigidbody.isKinematic = true; // Make it stationary again
        isFalling = false; // Reset falling state
        clickCount = 0; // Reset click count
        canBeHit = true; // Allow hits again
    }
}
using UnityEngine;

public class StoneClickDestroy1 : MonoBehaviour
{
    private int clickCount = 0;  // Counter for the number of clicks
    public GameObject trunkPrefab;  // Single prefab for the trunk logs
    public float logSpacing = 1.0f;  // Distance between each log segment along the length
    public float groundOffset = 0.5f;  // Adjust to lift logs slightly above ground
    private bool isDestroyed = false;  // To check if the tree has already been destroyed

    void OnMouseDown()
    {
        // Check if the left mouse button is clicked (0 is the left button)
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;  // Increment the click count

            if (clickCount >= 5 && !isDestroyed)  // Destroy tree on the 5th click
            {
                // Mark that the tree is destroyed to prevent further actions
                isDestroyed = true;

                // Start the process to destroy the tree and spawn logs
                DestroyTreeAndSpawnLogs();
            }
        }
    }

    void DestroyTreeAndSpawnLogs()
    {
        // Capture the final position and rotation of the tree
        Vector3 finalPosition = transform.position;
        Quaternion finalRotation = transform.rotation;

        // Destroy the tree object
        Destroy(gameObject);

        // Instantiate three logs along the tree's length (behind each other)
        for (int i = 0; i < 3; i++)
        {
            // Calculate the position for each log along the tree's length (using forward direction)
            Vector3 logPosition = finalPosition - transform.up * logSpacing * i;

            // Apply the ground offset to ensure logs are above ground level
            logPosition.y += groundOffset;

            Instantiate(trunkPrefab, logPosition, finalRotation);
        }
    }
}

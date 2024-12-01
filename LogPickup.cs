using UnityEngine;
using TMPro;

public class LogPickup : MonoBehaviour
{
    private bool isBeingHeld = false;            // Track if the log is being held
    private Transform originalParent;           // Store original parent to allow dropping
    private Rigidbody logRigidbody;             // Reference to the log's Rigidbody

    private Transform player;                   // Reference to the player
    private Animator playerAnimator;            // Reference to the player's Animator

    [SerializeField] private float throwForce = 500f;        // Force applied to throw the log
    private TextMeshProUGUI messageText;                      // Reference to the TMP Text for the message
    [SerializeField] private float messageDuration = 2f;     // Duration to show the message

    private Transform[] holdPositions;                        // Array of holding positions
    private static bool[] positionOccupied;                   // Track if each position is occupied
    private int currentPositionIndex = -1;                    // Index of the position this log occupies

    private static int totalStonesHeld = 0;                   // Static counter to track stones held across all instances
    private static readonly int maxStones = 3;                // Max number of stones that can be held across all types

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindWithTag("Player").transform;

        // Reference the player's Animator component
        playerAnimator = player.GetComponent<Animator>();

        // Find all hold positions with the tag "StonePosition"
        GameObject[] holdPositionObjects = GameObject.FindGameObjectsWithTag("HoldPosition");

        // Initialize the holdPositions array and positionOccupied array
        holdPositions = new Transform[holdPositionObjects.Length];
        positionOccupied = new bool[holdPositionObjects.Length];
        for (int i = 0; i < holdPositionObjects.Length; i++)
        {
            holdPositions[i] = holdPositionObjects[i].transform;
            positionOccupied[i] = false;  // Initially, all positions are unoccupied
        }

        // Ensure the Rigidbody is properly referenced
        logRigidbody = GetComponent<Rigidbody>();
        originalParent = transform.parent;  // Save the log's original parent
    }

    void Update()
    {
        // Check for a right mouse button click to throw while holding the log
        if (isBeingHeld && Input.GetMouseButtonDown(1))  // Right mouse button
        {
            Throw();
        }
    }

    void OnMouseDown()
    {
        if (!isBeingHeld)
        {
            // Attempt to pick up the log
            PickUp();
        }
        else
        {
            // Drop the log if it's already being held
            Drop();
        }
    }

    private void PickUp()
    {
        // Check if there is an available hold position
        int availableIndex = -1;
        for (int i = 0; i < positionOccupied.Length; i++)
        {
            if (!positionOccupied[i])
            {
                availableIndex = i;
                break;
            }
        }

        // Set the log as held
        isBeingHeld = true;

        // Disable physics on the log to keep it in place while held
        logRigidbody.isKinematic = true;
        logRigidbody.useGravity = false;

        // Assign the available position to this log
        currentPositionIndex = availableIndex;
        Transform holdPosition = holdPositions[currentPositionIndex];
        transform.position = holdPosition.position;
        transform.rotation = Quaternion.Euler(0, 40, -90);

        // Mark this position as occupied and increment total stones
        positionOccupied[currentPositionIndex] = true;
        totalStonesHeld++; // Increase the shared counter

        // Make it a child of the selected hold position to follow the player
        transform.parent = holdPosition;

        // Trigger the "pickUpWood" animation on the player
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("pickUpWood");
        }
    }

    private void Drop()
    {
        if (!isBeingHeld) return;

        // Set the log as not held
        isBeingHeld = false;

        // Re-enable physics so the log can fall
        logRigidbody.isKinematic = false;
        logRigidbody.useGravity = true;

        // Free up the occupied position and decrement the total counter
        if (currentPositionIndex != -1)
        {
            positionOccupied[currentPositionIndex] = false;
            currentPositionIndex = -1;  // Reset the position index for this log
            totalStonesHeld--; // Decrease the shared counter
        }

        // Detach from the hold position, restoring original parent (optional)
        transform.parent = originalParent;

        // Optionally, you can trigger a "drop" animation if needed
        // For example: playerAnimator.SetTrigger("dropWood");
    }

    private void Throw()
    {
        // Drop the log first to re-enable physics
        Drop();

        // Apply force to the log in the direction the player is facing
        logRigidbody.AddForce(player.forward * throwForce);

        // Optionally add some rotation for a more dynamic throw
        logRigidbody.AddTorque(Random.insideUnitSphere * throwForce * 0.1f);
    }
}

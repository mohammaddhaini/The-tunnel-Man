using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using System.Collections;

public class CubeController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public bool isStopped = false; // Tracks whether the cube is stopped
    public Camera playerCamera; // Reference to the player's camera
    public TMP_Text powerText; // Reference to the TMP text for displaying power
    public TMP_Text decreaseText; // Reference to the TMP text for displaying decrease counter
    public int totalPower = 100; // Total initial power
    private int destroyedTargetsCount = 0; // Counter for destroyed targets

    public GameObject cubePrefab; // Prefab for the cube to throw
    public float throwForce = 10f; // Force for throwing the cube

    public StorageCounter storageCounter; // Reference to the StorageCounter script
    private int decreaseCounter = 0; // Counter for decrease events
    public GameObject gameOverPanel; // Reference to the UI panel for game over

    public BoxCollider boxTrigger; // Reference to the Box Collider

    void Start()
    {
        // Add or get the Box Collider component and configure it

        boxTrigger.isTrigger = true; // Set it as a trigger
        boxTrigger.size = new Vector3(1f, 1f, 2f); // Set default size, adjust as needed

        UpdatePowerText(); // Initialize the power display
        UpdateDecreaseText(); // Initialize the decrease display
    }

    void Update()
    {
        // Check if energy is less than 5
        if (totalPower < 5)
        {
            StopMovement();
            return; // Prevent further execution of the update loop
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Cast a ray from the player's camera to check if we're looking at the cube
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
            {
                // Check if the ray hits an object with the tag "Cube"
                if (hit.collider.CompareTag("Cube"))
                {
                    ToggleMovement();
                }
            }
        }
        if (decreaseCounter > 50)
        {
            lose();
        }
        // Move the cube forward if not stopped
        if (!isStopped)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void ToggleMovement()
    {
        isStopped = !isStopped;
        if (isStopped)
        {
            moveSpeed = 0f; // Pause movement
            IncreaseDecreaseCounter(); // Increment the decrease counter when stopping
        }
        else
        {
            moveSpeed = 5f; // Resume movement
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && totalPower >= 10)
        {
            StartCoroutine(HandleCubeCollision(other.gameObject));
        }
    }

    IEnumerator HandleCubeCollision(GameObject target)
    {
        isStopped = true; // Stop movement
        moveSpeed = 0f; // Pause movement speed
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (target != null && totalPower >= 5) // Ensure the object still exists and enough energy is available
        {
            Destroy(target); // Destroy the target cube
            destroyedTargetsCount++; // Increment the destroyed targets counter

            if (destroyedTargetsCount % 9 == 0) // Every 4 destroyed targets
            {
                DecreasePower(10); // Decrease power by 5
            }

            if (destroyedTargetsCount % 18 == 0) // Throw a cube every 8 destroyed targets
            {
                ThrowCube();
            }
        }

        moveSpeed = 5f; // Resume movement speed
        isStopped = false; // Allow movement again
    }

    void ThrowCube()
    {
        if (storageCounter != null && storageCounter.woodCount >= 1)
        {
            storageCounter.woodCount -= 1;
            storageCounter.UpdateCounters();

            if (cubePrefab != null)
            {
                GameObject thrownCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
                Rigidbody rb = thrownCube.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
                }
            }
        }
        else
        {
            Debug.Log("Not enough resources to throw the cube!");
            IncreaseDecreaseCounter();
        }
    }

    void DecreasePower(int amount)
    {
        totalPower -= amount;
        if (totalPower < 0)
        {
            totalPower = 0;
        }

        UpdatePowerText();
    }

    void IncreaseDecreaseCounter()
    {
        decreaseCounter++;
        UpdateDecreaseText();
    }

    public void UpdatePowerText()
    {
        powerText.text = totalPower + " %";
    }

    public void UpdateDecreaseText()
    {
        if (decreaseText != null)
        {
            decreaseText.text = "Chance to fall: " + decreaseCounter;
        }
    }
    public void lose() 
    {

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true); // Show the game over panel
                StopMovement(); // Stop any movement or actions
            }
        
    }

    void StopMovement()
    {
        isStopped = true;
        moveSpeed = 0f;
    }

    void ResumeMovement()
    {
        isStopped = false;
        moveSpeed = 5f;
    }
}

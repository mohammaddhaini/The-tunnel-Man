using UnityEngine;
using UnityEngine.UI; // Add this for UI

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float switchTime = 15f;
    public string unsafeLayerName = "Unsafe";
    private int playerLayer;
    private int unsafeLayer;
    public Animator animator;
    public GameObject deathPanel; // Reference to the death screen UI panel
    public FirstPersonController movementController; // Reference to the movement script

    private float switchTimer;
    private bool isInPlayerLayer = false;
    private bool isTimerRunning = false;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        unsafeLayer = LayerMask.NameToLayer(unsafeLayerName);
        currentHealth = maxHealth;
        if (deathPanel != null)
            deathPanel.SetActive(false); // Ensure the death panel is hidden at start
    }

    void Update()
    {


    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        Debug.Log("Player health: " + currentHealth);
        Die();
    }

    private void Die()
    {
        Debug.Log("Player died.");
        animator.SetTrigger("death");

        // Disable movement
        if (movementController != null)
        {
            movementController.enabled = false;
        }

        // Show the death UI
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }

        // Pause the game
      //  Time.timeScale = 0f; // Freeze the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make cursor visible
    }



    }



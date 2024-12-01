using UnityEngine;

public class TrampolineBehavior : MonoBehaviour
{
    public float bounceHeight = 10f; // Height of the bounce

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Access the player's FirstPersonController script
            FirstPersonController playerController = other.GetComponent<FirstPersonController>();
            if (playerController != null && playerController.IsGrounded())
            {
                // Bounce the player only if grounded
                playerController.Bounce(bounceHeight);
            }
        }
    }
}

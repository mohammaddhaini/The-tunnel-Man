using UnityEngine;

public class TapDoorController : MonoBehaviour
{
    public float openAngle = 90f;        // Angle to open to (rotation in degrees on Z-axis)
    public float openSpeed = 3f;         // Speed of door opening
    public bool isOpen = false;          // Track if door is open or closed
    private Quaternion closedRotation;   // Initial rotation (closed)
    private Quaternion openRotation;     // Target rotation (open)
    private float rotationVelocity;      // Used for smooth damp easing

    void Start()
    {
        // Store initial rotation as closed position (rotation on Z-axis)
        closedRotation = transform.rotation;
        // Calculate the open rotation based on the desired open angle on Z-axis
        openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + openAngle);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for specific objects (stone, wood, iron) to trigger opening
        if ((other.CompareTag("stone") || other.CompareTag("wood") || other.CompareTag("iron")) && !isOpen)
        {
            isOpen = true;
            StopAllCoroutines();
            StartCoroutine(RotateDoor(openRotation));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Close the door when object leaves the area
        if (other.CompareTag("stone") || other.CompareTag("wood") || other.CompareTag("iron"))
        {
            isOpen = false;
            StopAllCoroutines();
            StartCoroutine(RotateDoor(closedRotation));
        }
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            // Smoothly interpolate the door's rotation on Z-axis only
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.z,
                targetRotation.eulerAngles.z,
                ref rotationVelocity,
                1f / openSpeed
            );

            // Apply rotation on the Z-axis, keeping X and Y unchanged
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            yield return null;
        }

        // Set final rotation to exact target rotation
        transform.rotation = targetRotation;
    }
}

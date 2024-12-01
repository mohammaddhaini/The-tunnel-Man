using UnityEngine;

public class PickupAndThrow : MonoBehaviour
{
    public Camera playerCamera; // Assign the player's camera
    public Transform holdPoint; // Empty GameObject where the object will be held
    public float throwForce = 10f; // Force applied when throwing
    public Transform targetLocation; // Predetermined position for connected objects

    private Rigidbody pickedObject;
    private bool isHoldingObject = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            if (isHoldingObject)
            {
                ThrowObject();
            }
            else
            {
                TryPickUpObject();
            }
        }
    }

    void TryPickUpObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f)) // Adjust the raycast distance
        {
            if (hit.collider.CompareTag("Pickup")) // Check for the correct tag
            {
                pickedObject = hit.collider.GetComponent<Rigidbody>();
                if (pickedObject != null)
                {
                    // Disable physics while holding the object
                    pickedObject.useGravity = false;
                    pickedObject.isKinematic = true;

                    // Parent the object to the holdPoint
                    pickedObject.transform.SetParent(holdPoint);
                    pickedObject.transform.localPosition = Vector3.zero; // Center it at the holdPoint
                    pickedObject.transform.localRotation = Quaternion.identity; // Reset rotation

                    isHoldingObject = true;
                }
            }
        }
    }

    void ThrowObject()
    {
        if (pickedObject != null)
        {
            // Check if looking at an "Energy" object
            bool isLookingAtEnergy = CheckIfLookingAtEnergy();

            if (isLookingAtEnergy)
            {
                // Log connection status
                Debug.Log("Connected");

                // Move object to the predetermined location
                pickedObject.transform.SetParent(null);
                pickedObject.transform.position = targetLocation.position;
                pickedObject.transform.rotation = targetLocation.rotation;

                // Re-enable physics without adding force
                pickedObject.useGravity = true;
                pickedObject.isKinematic = false;
            }
            else
            {
                // Log disconnection status
                Debug.Log("Not connected");

                // Unparent the object and throw it
                pickedObject.transform.SetParent(null);

                // Re-enable physics
                pickedObject.useGravity = true;
                pickedObject.isKinematic = false;

                // Add force to throw it
                pickedObject.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
            }

            // Reset state
            pickedObject = null;
            isHoldingObject = false;
        }
    }

    bool CheckIfLookingAtEnergy()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f)) // Adjust the raycast distance if needed
        {
            if (hit.collider.CompareTag("Energy"))
            {
                return true;
            }
        }
        return false;
    }
}

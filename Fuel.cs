using System.Collections;
using UnityEngine;
using TMPro; // Include TextMeshPro namespace

public class Fuel : MonoBehaviour
{
    public CubeController cubeController; // Reference to CubeController
    public TextMeshPro miningText; // Reference to TextMeshPro text
    private bool isMining = false; // Tracks whether mining is active
    private Coroutine miningCoroutine = null; // Keeps reference to the active coroutine

    private void Start()
    {
        // Ensure the text is hidden initially
        if (miningText != null)
        {
            miningText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            StartMining(); // Start mining
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            StopMining(); // Stop mining
        }
    }

    private void StartMining()
    {
        if (!isMining)
        {
            isMining = true;
            if (miningText != null)
            {
                miningText.gameObject.SetActive(false); // Show the text
            }
            miningCoroutine = StartCoroutine(IncreasePowerOverTime());
            Debug.Log("miningOn");
        }
    }

    private void StopMining()
    {
        if (isMining)
        {
            isMining = false;
            if (miningText != null)
            {
                miningText.gameObject.SetActive(true); // Hide the text
            }
            if (miningCoroutine != null)
            {
                StopCoroutine(miningCoroutine);
                miningCoroutine = null;
            }
            Debug.Log("miningOff");
        }
    }

    private IEnumerator IncreasePowerOverTime()
    {
        while (isMining)
        {
            if (cubeController.totalPower < 100) // Check if power is below 100
            {
                cubeController.totalPower += 2; // Increment power
                cubeController.UpdatePowerText(); // Update display
            }
            yield return new WaitForSeconds(1f); // Wait before checking/incrementing again
        }
    }
}

using UnityEngine;
using System.Collections; // Required for IEnumerator

public class SpotlightController : MonoBehaviour
{
    private Light spotlight; // Reference to the spotlight component

    void Start()
    {
        spotlight = GetComponent<Light>();
        StartCoroutine(BlinkLight());
    }

    private System.Collections.IEnumerator BlinkLight()
    {
        // Times for blinking (fast to slow)
        float[] blinkIntervals = { 0.1f, 0.2f, 0.7f };
        int blinkCount = 3; // Number of times to blink per interval

        foreach (float interval in blinkIntervals)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                spotlight.enabled = !spotlight.enabled; // Toggle light on/off
                yield return new WaitForSeconds(interval); // Wait for interval
            }
        }

        // Ensure the light stays on after blinking
        spotlight.enabled = true;
    }
}

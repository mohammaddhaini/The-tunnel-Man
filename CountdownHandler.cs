using UnityEngine;
using TMPro;
using System.Collections; // Required for IEnumerator

public class CountdownHandler : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // UI element to display countdown
    public float countdownTime = 30f; // Countdown duration
    private bool isCountingDown = false;

    public delegate void CountdownComplete(); // Event delegate for when the countdown ends
    public event CountdownComplete OnCountdownEnd; // Event to notify the spawner

    public void StartCountdown()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            StartCoroutine(CountdownRoutine());
        }
    }

    private IEnumerator CountdownRoutine()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            // Update UI
            if (countdownText != null)
            {
                countdownText.text = $"Enemy spawning in {Mathf.Ceil(remainingTime)}";
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        // Clear UI message when done
        if (countdownText != null)
        {
            countdownText.text = "";
        }

        // Trigger event to notify the spawner
        OnCountdownEnd?.Invoke();
    }
}

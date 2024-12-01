using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class StorageCounter : MonoBehaviour
{
    // Variables to track the count of each item type
    public int stoneCount = 0;
    public int woodCount = 0;

    // References to the TextMeshPro components for displaying counts
    public TextMeshPro stoneTextMesh;
    public TextMeshPro woodTextMesh;

    private void Start()
    {
        // Initialize text displays when the game starts
        UpdateCounters();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect objects entering the trigger collider and update counts
        if (other.CompareTag("stone"))
        {
            stoneCount++;
            UpdateCounters();
            Destroy(other.gameObject);  // Remove the object after counting
        }
        else if (other.CompareTag("wood"))
        {
            woodCount++;
            UpdateCounters();
            Destroy(other.gameObject);  // Remove the object after counting
        }

    }

    public void UpdateCounters()
    {
        // Update the TextMeshPro components with the current counts
        if (stoneTextMesh != null)
            stoneTextMesh.text = "Stone: " + stoneCount;

        if (woodTextMesh != null)
            woodTextMesh.text = "Wood: " + woodCount;

       
    }
}

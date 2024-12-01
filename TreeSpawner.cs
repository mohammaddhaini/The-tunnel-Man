using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    // Reference to the tree prefab (Assign this in the inspector)
    public GameObject treePrefab;

    // Array of empty GameObjects placed in the scene to define spawn points
    public GameObject[] spawnPoints;

    void Start()
    {
        // Call the function to spawn trees at each spawn point
        SpawnTrees();
    }

    void SpawnTrees()
    {
        // Check if the tree prefab and spawn points are assigned
        if (treePrefab != null && spawnPoints.Length > 0)
        {
            // Loop through each spawn point and instantiate a tree
            foreach (GameObject spawnPoint in spawnPoints)
            {
                // Instantiate tree at the position and rotation of the spawn point
                Instantiate(treePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
        }
        else
        {
            Debug.LogWarning("Tree prefab or spawn points are not assigned.");
        }
    }
}

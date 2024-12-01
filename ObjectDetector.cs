using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius around the player to detect cubes
    public string cubeTag = "Cube";   // Tag to identify cube objects

    void Update()
    {
        HandleCubeVisibility();
    }

    void HandleCubeVisibility()
    {
        // Find all objects within the detection radius
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in nearbyObjects)
        {
            if (collider.CompareTag(cubeTag))
            {
                // Enable MeshRenderer if within range
                SetCubeVisibility(collider.gameObject, true);
            }
        }

        // Find cubes that are out of range and disable them
        Collider[] allCubes = FindObjectsOfType<Collider>();
        foreach (Collider cubeCollider in allCubes)
        {
            if (cubeCollider.CompareTag(cubeTag) && Vector3.Distance(transform.position, cubeCollider.transform.position) > detectionRadius)
            {
                SetCubeVisibility(cubeCollider.gameObject, false);
            }
        }
    }

    void SetCubeVisibility(GameObject cube, bool isVisible)
    {
        MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = isVisible;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

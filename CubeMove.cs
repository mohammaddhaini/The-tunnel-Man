using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float detectionRange = 5f; // Distance of the detection ray
    public KeyCode interactKey = KeyCode.E; // Key to interact with the machine

    private GameObject detectedMachine; // Stores the detected machine object

    void Update()
    {
        DetectMachine();

        if (detectedMachine != null && Input.GetKeyDown(interactKey))
        {
            ToggleCubeController(detectedMachine);
        }
    }

    void DetectMachine()
    {
        RaycastHit hit;

        // Cast a ray forward from the player
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("Machine"))
            {
                detectedMachine = hit.collider.gameObject;

                // Optionally, highlight or indicate detection
                Debug.DrawLine(transform.position, hit.point, Color.green);
            }
            else
            {
                detectedMachine = null;
            }
        }
        else
        {
            detectedMachine = null;
        }
    }

    void ToggleCubeController(GameObject machine)
    {
        CubeController cubeController = machine.GetComponent<CubeController>();
        if (cubeController != null)
        {
            cubeController.enabled = !cubeController.enabled; // Toggle the script's enabled state

            if (cubeController.enabled)
            {
                Debug.Log("CubeController script enabled.");
            }
            else
            {
                Debug.Log("CubeController script disabled.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection ray in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}

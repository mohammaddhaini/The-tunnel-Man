using UnityEngine;

public class ToggleLayerOnTrigger : MonoBehaviour
{
    // Specify the layers to toggle between
    public string layer1Name = "Layer1";
    public string layer2Name = "Layer2";

    private int layer1;
    private int layer2;

    void Start()
    {
        // Convert layer names to layer indices
        layer1 = LayerMask.NameToLayer(layer1Name);
        layer2 = LayerMask.NameToLayer(layer2Name);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Get the current layer of the player
            int currentLayer = other.gameObject.layer;

            // Toggle the layer
            if (currentLayer == layer1)
            {
                ChangeLayerRecursively(other.gameObject, layer2);
            }
            else if (currentLayer == layer2)
            {
                ChangeLayerRecursively(other.gameObject, layer1);
            }
        }
    }

    // Function to recursively change the layer of the object and its children
    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }
}

using UnityEngine;

public class CubeDivider : MonoBehaviour
{
    public GameObject cubePrefab;   // Reference to the small cube prefab
    public int gridSize = 5;        // Number of cubes along one axis
    public Vector3 cubeScale = new Vector3(5, 5, 5); // Overall size of the large cube (x, y, z)

    void Start()
    {
        GenerateLargeCube();
    }

    void GenerateLargeCube()
    {
        // Calculate spacing based on the desired scale and grid size
        float spacingX = cubeScale.x / gridSize;
        float spacingY = cubeScale.y / gridSize;
        float spacingZ = cubeScale.z / gridSize;

        // Offset to center the large cube on the CubeManager
        Vector3 offset = new Vector3(
            (gridSize - 1) * spacingX * 0.5f,
            (gridSize - 1) * spacingY * 0.5f,
            (gridSize - 1) * spacingZ * 0.5f
        );

        // Loop through the 3D grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    // Calculate position for each small cube
                    Vector3 position = new Vector3(
                        x * spacingX - offset.x,
                        y * spacingY - offset.y,
                        z * spacingZ - offset.z
                    );

                    // Instantiate the cube at the calculated position
                    GameObject cube = Instantiate(cubePrefab, transform.position + position, Quaternion.identity, transform);

                    // Scale each small cube so they fit the desired size
                    cube.transform.localScale = new Vector3(spacingX, spacingY, spacingZ);
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Reference to the Play and Exit buttons
    public Button playButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Add listeners to the buttons to respond to click events
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    // Method to start the game (replace "GameScene" with your actual game scene name)
    public void OnPlayButtonClicked()
    {
        // Example: Load the scene named "GameScene"
        SceneManager.LoadScene("SampleScene");
    }

    // Method to exit the game
    public void OnExitButtonClicked()
    {
        // If running in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If the game is running as a build
            Application.Quit();
#endif
    }
}

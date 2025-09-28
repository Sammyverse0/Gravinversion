using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called by Play Button
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // replace with your gameplay scene name
    }

    // Called by Settings Button
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene"); // load settings scene
    }

    // Called by Quit Button
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // stop in editor
        #else
            Application.Quit(); // quit in build
        #endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject aboutPanel;

    private bool AboutOpen = false;
    private bool isPaused = false;

    // Key for the intro video PlayerPrefs
    private const string VideoPlayedKey = "IntroVideoPlayed";

    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the Escape key was pressed
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        // Resume time in the game
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Hide the pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Called by Play Button
    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MainScene"); // replace with your gameplay scene name
    }

    // Called by Settings Button
    public void OpenSettings()
    {
        SceneManager.LoadScene("OptionScene"); // load settings scene
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
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

    public void StoryScene()
    {
        SceneManager.LoadScene("IntroVideo");
    }


    // Optional: Reset and immediately go to the video scene
    public void ResetAndPlayIntroVideo()
    {
        PlayerPrefs.DeleteKey(VideoPlayedKey);
        PlayerPrefs.Save();
        Debug.Log("Intro video reset. Loading video scene now.");
        SceneManager.LoadScene("IntroVideo");
    }

    public void About()
    {
        if (aboutPanel != null)
        {
            aboutPanel.SetActive(true);
        }

    }

    public void closeabout()
    {
        if (aboutPanel != null)
        {
            aboutPanel.SetActive(false);
        }
    }
}
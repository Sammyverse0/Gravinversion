using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Required for using the VideoPlayer

public class VideoManager : MonoBehaviour
{
    [Header("Video Settings")]
    [Tooltip("The VideoPlayer component that will play the intro.")]
    public VideoPlayer videoPlayer;
    [Tooltip("The name of the scene to load after the video (e.g., MainMenu).")]
    public string nextSceneName;

    // A key to save in PlayerPrefs so we remember if the video was shown.
    private const string VideoPlayedKey = "IntroVideoPlayed";

    void Start()
    {
        // Check if the intro video has already been played before.
        // The '1' signifies 'true'. PlayerPrefs doesn't store booleans directly.
        if (PlayerPrefs.GetInt(VideoPlayedKey, 0) == 1)
        {
            // If it has been played, skip the video and load the next scene immediately.
            Debug.Log("Intro video already played. Skipping to next scene.");
            LoadNextScene();
        }
        else
        {
            // If it hasn't been played, start the video.
            // We'll use an event to know when the video has finished.
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
    }

    // This method is called automatically when the video clip ends.
    void OnVideoFinished(VideoPlayer vp)
    {
        // Mark that the video has now been played.
        PlayerPrefs.SetInt(VideoPlayedKey, 1);
        PlayerPrefs.Save(); // Make sure to save the change to disk.

        Debug.Log("Video finished. Loading next scene.");
        LoadNextScene();
    }

    void LoadNextScene()
    {
        // Unsubscribe from the event to prevent memory leaks.
        videoPlayer.loopPointReached -= OnVideoFinished;
        // Load the main menu or your first game scene.
        SceneManager.LoadScene(nextSceneName);
    }
}

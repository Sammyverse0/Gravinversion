using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Video Settings")]
    [Tooltip("The VideoPlayer component that will play the intro.")]
    public VideoPlayer videoPlayer;
    [Tooltip("The name of the scene to load after the video (e.g., MainMenu).")]
    public string nextSceneName;
    [Tooltip("Should the video load the next scene when finished? (Disable for manual button playback)")]
    public bool loadSceneAfterVideo = true;

    
    private const string VideoPlayedKey = "IntroVideoPlayed";

    void Start()
    {
        
        if (PlayerPrefs.GetInt(VideoPlayedKey, 0) == 1)
        {
           
            Debug.Log("Intro video already played. Skipping to next scene.");
            LoadNextScene();
        }
        else
        {
            
            PlayVideo(true); 
        }
    }

        public void PlayVideoFromButton()
    {
        
        bool originalLoadSetting = loadSceneAfterVideo;
        loadSceneAfterVideo = false;
        
        PlayVideo(false); 
        
        
        loadSceneAfterVideo = originalLoadSetting;
    }

    
    private void PlayVideo(bool markAsPlayed)
    {
        // Unsubscribe first to avoid duplicate subscriptions
        videoPlayer.loopPointReached -= OnVideoFinished;
        
        // Subscribe to the finish event
        videoPlayer.loopPointReached += OnVideoFinished;
        
        // Mark as played if this is the first automatic playback
        if (markAsPlayed)
        {
            PlayerPrefs.SetInt(VideoPlayedKey, 1);
            PlayerPrefs.Save();
        }
        
        videoPlayer.Play();
        Debug.Log("Video started playing.");
    }

    // This method is called automatically when the video clip ends.
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished.");
        
        // Only load the next scene if enabled (automatic first-time playback)
        if (loadSceneAfterVideo)
        {
            LoadNextScene();
        }
        else
        {
            // If playing from button, just stop and reset
            videoPlayer.Stop();
        }
    }

    void LoadNextScene()
    {
        // Unsubscribe from the event to prevent memory leaks.
        videoPlayer.loopPointReached -= OnVideoFinished;
        // Load the main menu or your first game scene.
        SceneManager.LoadScene(nextSceneName);
    }

    // Optional: Call this from a button to reset and see the intro again
    public void ResetIntroVideo()
    {
        PlayerPrefs.DeleteKey(VideoPlayedKey);
        PlayerPrefs.Save();
        Debug.Log("Intro video reset. It will play again on next game start.");
    }
}
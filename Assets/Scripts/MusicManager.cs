using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // Required for using the AudioMixer

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Settings")]
    public string gameSceneName;
    public AudioMixer mainMixer; // NEW: Reference to the mixer

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        audioSource = GetComponent<AudioSource>();

        // NEW: Load the saved volume as soon as the game starts
        LoadVolume();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName) {
            audioSource.Stop();
        } else {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
    }

    // NEW: This function now lives in the persistent manager
    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    // NEW: Helper function to load and apply the volume
    void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        mainMixer.SetFloat("Volume", Mathf.Log10(savedVolume) * 20);
    }
}
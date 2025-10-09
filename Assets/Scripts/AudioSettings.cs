using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Assign the music volume slider UI element here.")]
    public Slider musicSlider;

    void Start()
    {
    
        musicSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    
    public void SetMusicVolumeFromSlider()
    {
        
        float volume = musicSlider.value;

        
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusicVolume(volume);
        }
    }
}
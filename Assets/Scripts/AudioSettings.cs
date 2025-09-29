using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Assign the music volume slider UI element here.")]
    public Slider musicSlider;

    void Start()
    {
        // When the settings scene loads, set the slider's position
        // to the value that was saved in PlayerPrefs.
        musicSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    // This function is called by the slider's OnValueChanged event
    public void SetMusicVolumeFromSlider()
    {
        // Get the slider's current value
        float volume = musicSlider.value;

        // Tell the persistent MusicManager to update the volume
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusicVolume(volume);
        }
    }
}
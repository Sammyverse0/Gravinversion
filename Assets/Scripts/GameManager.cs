using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Transition Settings")]
    [Tooltip("The Animator component on your fader UI object.")]
    public Animator faderAnimator;
    [Tooltip("The duration of your fade-out animation clip. This must match the animation's length.")]
    public float fadeDuration = 1.0f;

    void Awake()
    {
        // --- Singleton Logic ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Public method to be called from other scripts to start a scene transition
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeToScene(sceneName));
    }

    private IEnumerator FadeToScene(string sceneName)
    {
        // 1. Trigger the "start" animation (your fade-out)
        faderAnimator.SetTrigger("end");

        // 2. Wait for the duration of the fade-out animation to complete
        yield return new WaitForSeconds(fadeDuration);

        // 3. Load the new scene
        SceneManager.LoadScene(sceneName);

        // 4. Trigger the "end" animation (your fade-in)
        // This will play automatically if the Animator is set up correctly,
        // but calling it ensures it works if the Animator state is complex.
        faderAnimator.SetTrigger("start");
    }
}


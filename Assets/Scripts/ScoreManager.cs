using UnityEngine;
using TMPro; // Namespace for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Tooltip("Assign the UI Text element for the score here.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Assign the UI Text element for the lives here.")]
    public TextMeshProUGUI livesText; // NEW: Reference for the lives text

    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // NEW: A function to update the lives display
    public void UpdateLivesText(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }
}
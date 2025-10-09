using UnityEngine;
using TMPro; // Namespace for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Tooltip("Assign the UI Text element for the score here.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Assign the UI Text element for the lives here.")]
    public TextMeshProUGUI livesText; // Reference for the lives text

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

        // 🔥 Increase level speed slightly whenever score increases
        LevelGenerator levelGen = FindObjectOfType<LevelGenerator>();
        if (levelGen != null)
        {
            // A very small increase per collectible (tune this value)
            float speedIncrease = amount * 0.002f;  // +20 score => +0.04 speed
            levelGen.moveSpeed = Mathf.Min(levelGen.moveSpeed + speedIncrease, levelGen.maxMoveSpeed);
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void UpdateLivesText(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    public int GetCurrentScore()
    {
        return score;
    }
}

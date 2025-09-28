using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxLives = 3;
    [Tooltip("The X position the player resets to after taking damage.")]
    public float resetXPosition = 1.88f;

    // --- Private Variables ---
    private int currentLives;
    private CharacterController characterController;

    void Start()
    {
        // Get references to other components on the player
        characterController = GetComponent<CharacterController>();
        
        currentLives = maxLives;
        
        // Update the UI with the starting number of lives
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.UpdateLivesText(currentLives);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the object we collided with has the "Obstacle" tag
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        // Safety check to prevent taking damage multiple times while already dying
        if (currentLives <= 0) return;

        currentLives--;

        // Update the UI with the new number of lives
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.UpdateLivesText(currentLives);
        }

        if (currentLives > 0)
        {
            // --- Reset player state ---
            // NOTE: This version is missing the momentum reset, which may cause a sliding bug.
            characterController.enabled = false;
            Vector3 resetPosition = new Vector3(resetXPosition, transform.position.y, transform.position.z);
            transform.position = resetPosition;
            characterController.enabled = true;
        }
        else
        {
            // If no lives are left, call the Die function
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // Pause the game
    }
}
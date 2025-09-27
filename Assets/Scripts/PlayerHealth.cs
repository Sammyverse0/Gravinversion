using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    // NEW: Add a reference to the CharacterController
    private CharacterController characterController;

    void Start()
    {
        // NEW: Get the CharacterController component at the start
        characterController = GetComponent<CharacterController>();

        currentLives = maxLives;
        Debug.Log("Game started! Lives: " + currentLives);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentLives--;
        Debug.Log("Hit an obstacle! Lives remaining: " + currentLives);

        if (currentLives > 0)
        {
            // --- NEW: RESET POSITION LOGIC ---
            // This is the safest way to teleport a CharacterController
            characterController.enabled = false; // Disable controller
            Vector3 resetPosition = new Vector3(1.88f, transform.position.y, transform.position.z);
            transform.position = resetPosition;
            characterController.enabled = true; // Re-enable controller
        }
        else
        {
            // If no lives are left, then call the Die function
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
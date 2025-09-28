using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private bool isDead = false; // To prevent Die() from being called multiple times

    void Update()
    {
        if ((transform.position.y > 10f || transform.position.y < -10f) && !isDead)
        {
            Die();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If we hit an object with the "Obstacle" tag and we are not already dead...
        Debug.Log("Collided with: " + hit.gameObject.name + " which has tag: '" + hit.gameObject.tag + "'");
        if (hit.gameObject.CompareTag("Obstacle") && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }
}
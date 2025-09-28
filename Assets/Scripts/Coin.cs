using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;

    // This function is called when another collider enters this object's trigger zone
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coin touched by: " + other.name); // Add this line
        // Check if the object that touched the coin has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Find the ScoreManager and add score
            ScoreManager.Instance.AddScore(scoreValue);

            // TODO: Add a coin collect sound effect here if you have one
            // AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Deactivate the coin so it can be reused by the pool
            gameObject.SetActive(false);
        }
    }
}
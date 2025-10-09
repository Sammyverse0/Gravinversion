using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;

        private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coin touched by: " + other.name); 
        if (other.CompareTag("Player"))
        {
           
            ScoreManager.Instance.AddScore(scoreValue);

            
            gameObject.SetActive(false);
        }
    }
}
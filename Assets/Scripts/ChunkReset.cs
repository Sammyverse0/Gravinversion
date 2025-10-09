using UnityEngine;
using System.Collections.Generic;

public class ChunkReset : MonoBehaviour
{
    
    private List<GameObject> coinsToReset;

    
    void Awake()
    {
        coinsToReset = new List<GameObject>();
        
      
        foreach (Coin coin in GetComponentsInChildren<Coin>(true))
        {
            coinsToReset.Add(coin.gameObject);
        }
    }

   
    private void OnEnable()
    {
       
        foreach (GameObject coin in coinsToReset)
        {
            coin.SetActive(true);
        }
    }
}
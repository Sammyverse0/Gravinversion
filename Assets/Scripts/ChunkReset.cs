using UnityEngine;
using System.Collections.Generic;

public class ChunkReset : MonoBehaviour
{
    // A list to hold all the coin objects within this chunk
    private List<GameObject> coinsToReset;

    // Awake is called once when the prefab is first created (even if it's inactive).
    // This is more efficient than searching every time the chunk is enabled.
    void Awake()
    {
        coinsToReset = new List<GameObject>();
        
        // Find every component of type "Coin" within this chunk's children,
        // including any that are already inactive, and add them to our list.
        foreach (Coin coin in GetComponentsInChildren<Coin>(true))
        {
            coinsToReset.Add(coin.gameObject);
        }
    }

    // This function is automatically called by Unity whenever the chunk is enabled.
    private void OnEnable()
    {
        // Go through our pre-filled list and re-activate every coin.
        foreach (GameObject coin in coinsToReset)
        {
            coin.SetActive(true);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    public float moveSpeed = 12f;
    public float chunkLength = 50f; 
    public int startingChunks = 5;

    [Header("Chunk Prefabs")]
    [Tooltip("The first chunk that is already placed in the scene.")]
    public GameObject startingChunk; // NEW: To hold our pre-placed chunk
    [Tooltip("The list of prefabs to spawn randomly.")]
    public GameObject[] chunkPrefabs; 

    private List<GameObject> activeChunks = new List<GameObject>();
    private Vector3 nextSpawnPoint; // CHANGED: We'll calculate this based on the startingChunk

    void Start()
    {
        Time.timeScale = 1f;

        // --- NEW STARTING LOGIC ---
        if (startingChunk == null)
        {
            Debug.LogError("The 'Starting Chunk' has not been assigned in the LevelGenerator Inspector!");
            return;
        }

        // 1. Add the pre-placed chunk to our active list
        activeChunks.Add(startingChunk);
        // 2. Set the next spawn point to be at the end of the starting chunk
        nextSpawnPoint = startingChunk.transform.position + new Vector3(0, 0, chunkLength);

        // 3. Spawn the rest of the runway, but one less than before
        for (int i = 0; i < startingChunks - 1; i++)
        {
            if (chunkPrefabs.Length > 0)
            {
                SpawnChunk(Random.Range(0, chunkPrefabs.Length));
            }
        }
    }

    void Update()
    {
        if (activeChunks.Count == 0) return;

        foreach (GameObject chunk in activeChunks)
        {
            chunk.transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        
        if (activeChunks[0].transform.position.z < -chunkLength)
        {
            RecycleOldestChunk();
        }
    }

    // --- The rest of the script is unchanged ---

    void SpawnChunk(int prefabIndex)
    {
        string chunkName = chunkPrefabs[prefabIndex].name;
        GameObject nextChunk = ObjectPooler.Instance.SpawnFromPool(chunkName, nextSpawnPoint, Quaternion.identity);
        
        if (nextChunk != null)
        {
            activeChunks.Add(nextChunk);
            nextSpawnPoint.z += chunkLength;
        }
        else
        {
            Debug.LogError("SPAWN FAILED: ObjectPooler returned null for tag: '" + chunkName + "'. Check your ObjectPooler setup!");
        }
    }

    void RecycleOldestChunk()
    {
        activeChunks.RemoveAt(0);
        int randomIndex = Random.Range(0, chunkPrefabs.Length);
        SpawnChunk(randomIndex);
    }
}
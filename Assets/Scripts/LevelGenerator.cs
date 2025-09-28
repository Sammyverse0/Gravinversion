using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    public float moveSpeed = 12f;
    public float chunkLength = 38.763f;
    public int visibleChunks = 5;
    public float despawnBuffer = 20f;

    [Header("Chunk Prefabs")]
    public GameObject startingChunk;
    public GameObject[] chunkPrefabs;

    // --- Private Variables ---
    private List<GameObject> pooledChunks = new List<GameObject>();
    private List<GameObject> activeChunks = new List<GameObject>();
    private Quaternion chunkRotation;
    private Vector3 moveDirection = Vector3.forward;

    // REMOVED: We no longer need a static nextSpawnPoint variable.

    void Start()
    {
        Time.timeScale = 1f;

        if (startingChunk == null || chunkPrefabs.Length == 0) {
            Debug.LogError("FATAL ERROR: Starting Chunk or Chunk Prefabs are not assigned in the Inspector!");
            return;
        }

        // --- Pooling Logic ---
        for (int i = 0; i < visibleChunks + 2; i++)
        {
            GameObject chunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]);
            chunk.SetActive(false);
            pooledChunks.Add(chunk);
        }

        // --- Initialization ---
        chunkRotation = startingChunk.transform.rotation;
        activeChunks.Add(startingChunk);
        
        // Spawn the initial runway of chunks.
        for (int i = 0; i < visibleChunks - 1; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // 1. Move every active chunk.
        foreach (GameObject chunk in activeChunks)
        {
            chunk.transform.position -= moveDirection * moveSpeed * Time.deltaTime;
        }

        // 2. Check if the oldest chunk should be recycled.
        if (activeChunks.Count > 0)
        {
            GameObject oldestChunk = activeChunks[0];
            if (Vector3.Dot(oldestChunk.transform.position, -moveDirection) > (chunkLength + despawnBuffer))
            {
                oldestChunk.SetActive(false);
                activeChunks.RemoveAt(0);
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        GameObject chunkToSpawn = GetPooledChunk();

        if (chunkToSpawn != null)
        {
            // --- THE CORE FIX IS HERE ---
            // 1. Find the last chunk currently in the scene.
            GameObject lastChunk = activeChunks[activeChunks.Count - 1];

            // 2. Calculate the new spawn position based on the LAST CHUNK'S CURRENT position.
            Vector3 spawnPosition = lastChunk.transform.position + moveDirection * chunkLength;

            // 3. Position, rotate, and activate the new chunk.
            chunkToSpawn.transform.position = spawnPosition;
            chunkToSpawn.transform.rotation = chunkRotation;
            chunkToSpawn.SetActive(true);

            // 4. Add the new chunk to the active list.
            activeChunks.Add(chunkToSpawn);
        }
    }

    // Helper method to find an available chunk from the pool.
    GameObject GetPooledChunk()
    {
        foreach (GameObject chunk in pooledChunks)
        {
            if (!chunk.activeInHierarchy)
            {
                return chunk;
            }
        }
        Debug.LogWarning("No inactive chunks available in the pool. Consider increasing the pool size.");
        return null;
    }
}
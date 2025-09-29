using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("How fast the chunks move towards the player.")]
    public float moveSpeed = 12f;
    [Tooltip("How many chunks should be visible on screen at once.")]
    public int visibleChunks = 5;
    [Tooltip("How far a chunk must be behind the player before it disappears.")]
    public float despawnBuffer = 20f;
    [Tooltip("The player's score must reach this value to unlock hard chunks.")]
    public int scoreToUnlockHardChunks = 500;

    [Header("Chunk Prefabs")]
    [Tooltip("The first chunk that is already placed in the scene. MUST have ChunkData script.")]
    public GameObject startingChunk;
    [Tooltip("The list of standard, easy prefabs. EACH must have ChunkData script.")]
    public GameObject[] easyChunkPrefabs;
    [Tooltip("The list of hard prefabs. EACH must have ChunkData script.")]
    public GameObject[] hardChunkPrefabs;

    // --- Private Variables ---
    private List<GameObject> pooledChunks = new List<GameObject>();
    private List<GameObject> activeChunks = new List<GameObject>();
    private Quaternion chunkRotation;
    private Vector3 moveDirection = Vector3.forward;

    void Start()
    {
        Time.timeScale = 1f;

        // --- Safety Checks ---
        if (startingChunk == null || easyChunkPrefabs.Length == 0) {
            Debug.LogError("FATAL ERROR: Starting Chunk or Easy Chunk Prefabs are not assigned in the Inspector!");
            return;
        }
        if (startingChunk.GetComponent<ChunkData>() == null) {
            Debug.LogError("FATAL ERROR: The 'Starting Chunk' is missing the ChunkData script!");
            return;
        }

        // --- Pooling and Initialization ---
        CreatePool(easyChunkPrefabs, visibleChunks + 2);
        CreatePool(hardChunkPrefabs, visibleChunks + 2);
        
        chunkRotation = startingChunk.transform.rotation;
        activeChunks.Add(startingChunk);

        // Spawn the initial runway of chunks
        for (int i = 0; i < visibleChunks - 1; i++) {
            SpawnChunk();
        }
    }

    void Update()
    {
        // 1. Move all active chunks
        foreach (GameObject chunk in activeChunks) {
            chunk.transform.position -= moveDirection * moveSpeed * Time.deltaTime;
        }

        // 2. Check if the oldest chunk needs to be recycled
        if (activeChunks.Count > 0) {
            GameObject oldestChunk = activeChunks[0];
            float oldestChunkLength = oldestChunk.GetComponent<ChunkData>().length;
            
            if (Vector3.Dot(oldestChunk.transform.position, -moveDirection) > (oldestChunkLength + despawnBuffer)) {
                oldestChunk.SetActive(false);
                activeChunks.RemoveAt(0);
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        // 1. Determine which prefabs are available based on the score
        List<GameObject> availablePrefabs = new List<GameObject>();
        availablePrefabs.AddRange(easyChunkPrefabs);
        if (ScoreManager.Instance != null && ScoreManager.Instance.GetCurrentScore() >= scoreToUnlockHardChunks) {
            availablePrefabs.AddRange(hardChunkPrefabs);
        }

        // 2. Pick a random prefab type from the available list
        GameObject prefabToSpawn = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        
        // 3. Get an inactive instance of that specific prefab from our pool
        GameObject chunkToSpawn = GetPooledChunk(prefabToSpawn);

        if (chunkToSpawn != null) {
            // 4. Calculate spawn position based on the CURRENT end of the last active chunk
            GameObject lastChunk = activeChunks[activeChunks.Count - 1];
            float lastChunkLength = lastChunk.GetComponent<ChunkData>().length;
            Vector3 spawnPosition = lastChunk.transform.position + moveDirection * lastChunkLength;

            // 5. Position, rotate, and activate the new chunk
            chunkToSpawn.transform.position = spawnPosition;
            chunkToSpawn.transform.rotation = chunkRotation;
            chunkToSpawn.SetActive(true);
            activeChunks.Add(chunkToSpawn);
        }
    }

    // Helper method to create the object pool
    void CreatePool(GameObject[] prefabs, int amount)
    {
        if (prefabs.Length == 0) return;
        foreach (GameObject prefab in prefabs) {
            for (int i = 0; i < Mathf.CeilToInt((float)amount / prefabs.Length); i++) {
                GameObject chunk = Instantiate(prefab);
                chunk.SetActive(false);
                pooledChunks.Add(chunk);
            }
        }
    }

    // Helper method to find a specific type of chunk in the pool
    GameObject GetPooledChunk(GameObject desiredPrefab)
    {
        foreach (GameObject chunk in pooledChunks) {
            if (!chunk.activeInHierarchy && chunk.name.StartsWith(desiredPrefab.name)) {
                return chunk;
            }
        }
        Debug.LogWarning("No inactive chunks of type " + desiredPrefab.name + " available. Consider increasing pool size.");
        return null;
    }
}

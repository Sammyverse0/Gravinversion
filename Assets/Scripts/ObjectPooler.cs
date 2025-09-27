using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // --- THE FIX IS HERE ---
    // All setup logic has been moved from Start() to Awake()
    void Awake()
    {
        Instance = this;

        Debug.Log("--- ObjectPooler AWAKE ---");
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
            Debug.Log("Pool created for tag: '" + pool.tag + "' with " + pool.size + " objects.");
        }
    }
    
    // Start() can now be removed or left empty.

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        Debug.Log("SpawnFromPool requested for tag: '" + tag + "'");

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("POOLER ERROR: Pool with tag '" + tag + "' doesn't exist. Check for typos in the ObjectPooler Inspector!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn;
    }
}
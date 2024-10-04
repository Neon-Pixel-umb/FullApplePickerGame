using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public List<Pool> pools; // List of pools, e.g., "Apple" and "Stick"
    public Dictionary<string, Queue<GameObject>> poolDictionary; // Dictionary to manage object pools

    [System.Serializable]
    public class Pool
    {
        public string tag; // Tag for the pool, must be unique
        public GameObject prefab; // Prefab to instantiate
        public int size; // Size of the pool
    }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of ObjectPooler exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            // Check for duplicate tags
            if (poolDictionary.ContainsKey(pool.tag))
            {
                Debug.LogError($"Duplicate pool tag found: {pool.tag}. Make sure all pool tags are unique.");
                continue; // Skip this pool to avoid adding it again
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new();

            // Create Queue full of prefabs
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            // Add pool to dictionary with tag as key
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object Pool with tag: " + tag + " does not exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            // custom Start
            pooledObject.OnObjectSpawn();
        }

        // FIXME: should be on destroy?
        // recycle
        //poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // FIXME: testing!
    public void Recycle(string tag, GameObject gameObject)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object Pool with tag: " + tag + " does not exist.");
            return;
        }
        // deactivate
        gameObject.SetActive(false);
        // recycle
        poolDictionary[tag].Enqueue(gameObject);
    }

}


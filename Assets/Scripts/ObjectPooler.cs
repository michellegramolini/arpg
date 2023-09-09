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

    // Dictionary for mapping object pool tags to the location of the prefab in Assets/Resources.
    // for this project all prefabs will be in the Poolable directory of Resources.
    // TODO: Enums baby.
    public Dictionary<string, string> tagToResourceMap = new()
    {
        // Popups
        { "damage_popup", "Popups/DamagePopup" },
        { "levelup_popup", "Popups/LevelUpPopup" },
        { "player_damage_popup", "Popups/PlayerDamagePopup" },
        // Effects
        { "kill_effect", "Effects/KillEffect" },
        { "splash_effect", "Effects/SplashEffect" },
        // Enemies
        { "spritz", "Enemies/Spritz" },
        // Drops / Pickups
        // TODO:
    };

    private void Activate(GameObject obj, bool isActivated)
    {
        obj.SetActive(isActivated);
        ChangeColliderState(obj, isActivated);
        ActivateSprites(obj, isActivated);
    }

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
                Activate(obj, false);
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

        IPooledObject pooledObject;

        if (poolDictionary[tag].TryDequeue(out GameObject objectToSpawn))
        {
            // Use the dequeuedObject
            Activate(objectToSpawn, true);
            objectToSpawn.transform.SetPositionAndRotation(position, rotation);
            pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        }
        else
        {
            // Handle when the queue is empty
            Debug.LogWarning("Object queue is empty.");

            // Get path to resource via pool tag
            if (tagToResourceMap.ContainsKey(tag))
            {
                string resource = tagToResourceMap[tag];
                // Prefabs will be in the Prefabs directory of Resources.
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/{resource}");
                if (!prefab)
                {
                    Debug.LogWarning($"Cannot find prefab in Resources with given path Assets/Resources/Poolable/{resource}");
                    return null;
                }

                objectToSpawn = Instantiate(prefab, position, rotation);
                Activate(objectToSpawn, true);
                pooledObject = objectToSpawn.GetComponent<IPooledObject>();
            }
            else
            {
                Debug.LogWarning($"Could not find Resources location for this tag {tag}.");
                return null;
            }
        }

        // custom Start
        pooledObject?.OnObjectSpawn();

        return objectToSpawn;
    }

    // TODO: I think this works like, yeah? probably?
    public void Recycle(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object Pool with tag: " + tag + " does not exist.");
            return;
        }
        // deactivate
        Activate(obj, false);
        // recycle
        poolDictionary[tag].Enqueue(obj);
    }


    private void ChangeColliderState(GameObject obj, bool state)
    {
        if (obj.TryGetComponent(out BoxCollider2D collider))
        {
            //Debug.Log($"Changing collider state of {gameObject.name} to {state}");
            collider.enabled = state;
        }
        if (obj.TryGetComponent(out Rigidbody2D rigidbody))
        {
            //Debug.Log($"Changing rigidbody state of {gameObject.name} to {state}");
            rigidbody.simulated = state;
        }
    }

    private void ActivateSprites(GameObject obj, bool state)
    {
        // Get all SpriteRenderer components attached to this GameObject and its children
        SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();

        // Loop through each SpriteRenderer and do something
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = state;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private Transform inactiveParent;

    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        Queue<GameObject> pool = GetOrCreatePool(prefab);

        GameObject instance = pool.Count > 0 ? pool.Dequeue() : CreateInstance(prefab);

        instance.transform.SetParent(null);
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);

        return instance;
    }

    public void Return(GameObject prefab, GameObject instance)
    {
        if (prefab == null || instance == null) return;

        instance.SetActive(false);

        if (inactiveParent != null)
        {
            instance.transform.SetParent(inactiveParent);
        }

        GetOrCreatePool(prefab).Enqueue(instance);
    }

    private GameObject CreateInstance(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);

        PooledObject pooledObject = instance.GetComponent<PooledObject>();

        if (pooledObject == null)
        {
            Debug.LogError($"{prefab.name} is missing PooledObject.");
            return instance;
        }

        pooledObject.Initialize(this, prefab);

        return instance;
    }

    private Queue<GameObject> GetOrCreatePool(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools.Add(prefab, pool);
        }

        return pool;
    }
}
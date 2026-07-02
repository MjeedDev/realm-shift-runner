using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private GameObjectPool pool;
    private GameObject prefab;

    public void Initialize(GameObjectPool pool, GameObject prefab)
    {
        this.pool = pool;
        this.prefab = prefab;
    }

    public void ReturnToPool()
    {
        if (pool == null || prefab == null)
        {
            gameObject.SetActive(false);
            return;
        }

        pool.Return(prefab, gameObject);
    }
}
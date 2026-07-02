using UnityEngine;

public class PooledObjectCleaner : MonoBehaviour
{
    [SerializeField] private float cleanDistanceBehindPlayer = 40f;

    private Transform player;
    private PooledObject pooledObject;

    private void Awake()
    {
        PlayerController playerController = FindFirstObjectByType<PlayerController>();

        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void OnEnable()
    {
        if (pooledObject == null)
        {
            TryGetComponent(out pooledObject);
        }
    }

    private void Update()
    {
        if (player == null) return;
        if (transform.position.z >= player.position.z - cleanDistanceBehindPlayer) return;

        if (pooledObject == null)
        {
            TryGetComponent(out pooledObject);
        }

        if (pooledObject == null)
        {
            enabled = false;
            return;
        }

        pooledObject.ReturnToPool();
    }
}
using Mirror;
using UnityEngine;

namespace _BallShooter._Scripts.ObjectPool
{
    public class PrefabPool : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject prefab;

        [Header("Debug")]
        public int currentCount;
        public Pool<GameObject> pool;

        public void Configure(GameObject prefab)
        {
            this.prefab = prefab;
        }
        
        void Start()
        {
            InitializePool();
            NetworkClient.RegisterPrefab(prefab, SpawnHandler, UnspawnHandler);
        }
        
        GameObject SpawnHandler(SpawnMessage msg) => Get(msg.position, msg.rotation);
        
        void UnspawnHandler(GameObject spawned) => Return(spawned);

        void OnDestroy()
        {
            NetworkClient.UnregisterPrefab(prefab);
        }

        void InitializePool()
        {
            pool = new Pool<GameObject>(CreateNew, 5);
        }

        GameObject CreateNew()
        {
            GameObject next = Instantiate(prefab, transform);
            next.name = $"{prefab.name}_pooled_{currentCount}";
            next.SetActive(false);
            currentCount++;
            return next;
        }
        
        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject next = pool.Get();

            // set position/rotation and set active
            next.transform.position = position;
            next.transform.rotation = rotation;
            next.SetActive(true);
            return next;
        }
        
        public void Return(GameObject spawned)
        {
            // disable object
            spawned.SetActive(false);

            // add back to pool
            pool.Return(spawned);
        }
    }
}

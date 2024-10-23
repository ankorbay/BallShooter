using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace _BallShooter._Scripts.ObjectPool
{
    public class PrefabPoolManager : NetworkBehaviour
    {
        [Header("Settings")]
        public int startSize = 5;
        public int maxSize = 20;

        [Header("Debug")]
        Queue<GameObject> pool;
        [SerializeField] int currentCount;
        
        private GameObject _prefab;
        
        void Start()
        {
            _prefab = NetworkManager.singleton.spawnPrefabs[1];
            
            InitializePool();
            NetworkClient.RegisterPrefab(_prefab, SpawnHandler, UnspawnHandler);
        }

        void OnDestroy()
        {
            NetworkClient.UnregisterPrefab(_prefab);
        }

        private void InitializePool()
        {
            pool = new Queue<GameObject>();
            for (int i = 0; i < startSize; i++)
            {
                GameObject next = CreateNew();

                pool.Enqueue(next);
            }
        }
        
        // used by ClientScene.RegisterPrefab
        GameObject SpawnHandler(SpawnMessage msg)
        {
            return GetFromPool(msg.position, msg.rotation);
        }

        // used by ClientScene.RegisterPrefab
        void UnspawnHandler(GameObject spawned)
        {
            PutBackInPool(spawned);
        }

        GameObject CreateNew()
        {
            if (currentCount > maxSize)
            {
                Debug.LogError($"Pool has reached max size of {maxSize}");
                return null;
            }

            // use this object as parent so that objects dont crowd hierarchy
            GameObject next = Instantiate(NetworkManager.singleton.spawnPrefabs[1], transform);
            next.name = $"{NetworkManager.singleton.spawnPrefabs[1].name}_pooled_{currentCount}";
            next.SetActive(false);
            currentCount++;
            return next;
        }
        
        /// <summary>
        /// Used to take Object from Pool.
        /// <para>Should be used on server to get the next Object</para>
        /// <para>Used on client by ClientScene to spawn objects</para>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject GetFromPool(Vector3 position, Quaternion rotation)
        {
            GameObject next = pool.Count > 0
                ? pool.Dequeue() // take from pool
                : CreateNew(); // create new because pool is empty

            // CreateNew might return null if max size is reached
            if (next == null) { return null; }

            // set position/rotation and set active
            next.transform.position = position;
            next.transform.rotation = rotation;
            next.SetActive(true);
            return next;
        }

        /// <summary>
        /// Used to put object back into pool so they can b
        /// <para>Should be used on server after unspawning an object</para>
        /// <para>Used on client by ClientScene to unspawn objects</para>
        /// </summary>
        /// <param name="spawned"></param>
        public void PutBackInPool(GameObject spawned)
        {
            // disable object
            spawned.SetActive(false);

            // add back to pool
            pool.Enqueue(spawned);
        }
    }
}

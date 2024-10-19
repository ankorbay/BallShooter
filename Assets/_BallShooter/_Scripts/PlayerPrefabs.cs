using UnityEngine;
public class PlayerPrefabs : MonoBehaviour
    {
        // A reference data script for most things character and customisation related.

        public static PlayerPrefabs playerPrefabsSingleton { get; private set; }

        public GameObject[] characterPrefabs;

        public void Awake()
        {
            playerPrefabsSingleton = this;
        }
    }

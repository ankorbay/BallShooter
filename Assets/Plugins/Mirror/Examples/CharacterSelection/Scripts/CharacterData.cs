using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
    public class CharacterData : MonoBehaviour
    {
        public static CharacterData characterDataSingleton { get; private set; }
        public GameObject[] characterPrefabs;

        public void Awake()
        {
            characterDataSingleton = this;
        }
    }

}
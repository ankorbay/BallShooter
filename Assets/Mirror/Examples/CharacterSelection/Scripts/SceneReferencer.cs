using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.CharacterSelection
{
    public class SceneReferencer : NetworkBehaviour
    {
        public Button buttonCharacterSelection;

        private CharacterData characterData;
        public GameObject characterSelectionObject;
        public GameObject lobbyUIObject;
        public GameObject environment;

        public override void OnStartClient()
        {
            base.OnStartClient();
            gameObject.SetActive(false);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            gameObject.SetActive(true);
        }

        private void Start()
        {
            characterData = CharacterData.characterDataSingleton;
            if (characterData == null)
            {
                Debug.Log("Add CharacterData prefab singleton into the scene.");
                return;
            }

            buttonCharacterSelection.onClick.AddListener(ButtonCharacterSelection);
        }

        public void ButtonCharacterSelection()
        {
            characterSelectionObject.SetActive(true);
            lobbyUIObject.SetActive(false);
            environment.SetActive(false);
            this.GetComponent<Canvas>().enabled = false;
        }

        public void CloseCharacterSelection()
        {
            characterSelectionObject.SetActive(false);
            lobbyUIObject.SetActive(true);
            environment.SetActive(true);
            this.GetComponent<Canvas>().enabled = true;
        }
    }
}
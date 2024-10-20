using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Mirror.Examples.CharacterSelection.NetworkManagerCharacterSelection;

namespace Mirror.Examples.CharacterSelection
{ 
    public class CanvasReferencer : MonoBehaviour
    {
        // Make sure to attach these Buttons in the Inspector
        public ColorButtonsController colorSelector;
        public Button buttonExit, buttonGo;

        public Transform podiumPosition;
        private int currentlySelectedCharacter = 1;
        private CharacterData characterData;
        private GameObject currentInstantiatedCharacter;
        private CharacterSelection characterSelection;
        public SceneReferencer sceneReferencer;
        public Camera cameraObj;

        private void Start()
        {
            characterData = CharacterData.characterDataSingleton;
            if (characterData == null)
            {
                Debug.Log("Add CharacterData prefab singleton into the scene.");
                return;
            }

            buttonExit.onClick.AddListener(ButtonExit);
            buttonGo.onClick.AddListener(ButtonGo);
            colorSelector.OnColorSelected += ColorSelectorOnOnColorSelected;
            LoadData();
            SetupCharacters();
        }

        private void ColorSelectorOnOnColorSelected(Color color)
        {
            StaticVariables.characterColour = color;
            SetupCharacterColours();
        }

        public void ButtonExit()
        {
            Application.Quit();
        }

        public void ButtonGo()
        {
            if (sceneReferencer && NetworkClient.active)
            {

                // You could check if prefab (character number) has not changed, and if so just update the sync vars and hooks of current prefab, this would call a command from your player.
                // this is not fully setup for this example, but provides a minor template to follow if needed
                //NetworkClient.localPlayer.GetComponent<CharacterSelection>().CmdSetupCharacter(StaticVariables.playerName, StaticVariables.characterColour);

                CreateCharacterMessage _characterMessage = new CreateCharacterMessage
                {
                    playerName = StaticVariables.playerName,
                    characterNumber = StaticVariables.characterNumber,
                    characterColour = StaticVariables.characterColour
                };

                ReplaceCharacterMessage replaceCharacterMessage = new ReplaceCharacterMessage
                {
                    createCharacterMessage = _characterMessage
                };
                NetworkManagerCharacterSelection.singleton.ReplaceCharacter(replaceCharacterMessage);
                sceneReferencer.CloseCharacterSelection();
            }
            else
            {
                SceneManager.LoadScene("LobbyGame");
            }
        }

        private void SetupCharacters()
        {
            if (currentInstantiatedCharacter)
            {
                Destroy(currentInstantiatedCharacter);
            }
            currentInstantiatedCharacter = Instantiate(characterData.characterPrefabs[currentlySelectedCharacter]);
            currentInstantiatedCharacter.transform.position = podiumPosition.position;
            currentInstantiatedCharacter.transform.rotation = podiumPosition.rotation;
            characterSelection = currentInstantiatedCharacter.GetComponent<CharacterSelection>();
            currentInstantiatedCharacter.transform.SetParent(this.transform.root);

            SetupCharacterColours();
        }

        public void SetupCharacterColours()
        {
           // Debug.Log("SetupCharacterColours");
            if (StaticVariables.characterColour != new Color(0, 0, 0, 0))
            {
                characterSelection.characterColour = StaticVariables.characterColour;
                characterSelection.AssignColours();
            }
        }

        public void LoadData()
        {
            // check that prefab is set, or exists for saved character number data
            if (StaticVariables.characterNumber > 0 && StaticVariables.characterNumber < characterData.characterPrefabs.Length)
            {
                currentlySelectedCharacter = StaticVariables.characterNumber;
            }
            else
            {
                StaticVariables.characterNumber = currentlySelectedCharacter;
            }
        }
    }
}
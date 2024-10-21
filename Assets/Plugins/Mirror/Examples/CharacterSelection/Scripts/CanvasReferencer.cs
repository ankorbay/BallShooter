using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
        private GameObject _currentInstantiatedCharacter;
        private CharacterSelection characterSelection;
        public SceneReferencer sceneReferencer;

        
        public void Configure()
        {
            // this is a placeholder for the future
        }
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
                NetworkClient.localPlayer.GetComponent<CharacterSelection>().CmdSetupCharacter(StaticVariables.characterColour);
                sceneReferencer.CloseCharacterSelection();
            }
            else
            {
                SceneManager.LoadScene("LobbyGame");
            }
        }

        private void SetupCharacters()
        {
            if (_currentInstantiatedCharacter)
            {
                Destroy(_currentInstantiatedCharacter);
            }
            _currentInstantiatedCharacter = Instantiate(characterData.characterPrefabs[currentlySelectedCharacter]);
            _currentInstantiatedCharacter.transform.position = podiumPosition.position;
            _currentInstantiatedCharacter.transform.rotation = podiumPosition.rotation;
            characterSelection = _currentInstantiatedCharacter.GetComponent<CharacterSelection>();
            _currentInstantiatedCharacter.transform.SetParent(this.transform.root);

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
    }
}
using _BallShooter._Scripts.Infrastructure;
using _BallShooter._Scripts.Player;
using Infrastructure;
using Infrastructure.Factory;
using Mirror;
using Mirror.Examples.CharacterSelection;
using UnityEngine;
using UnityEngine.UI;

namespace _BallShooter._Scripts.UI
{
    public class ColorSelectionController : MonoBehaviour
    {
        public ColorButtonsController colorSelector;
        public Button buttonExit, buttonGo;
        public Transform podiumPosition;

        private int currentlySelectedCharacter = 1;
        private CharacterData characterData;
        private GameObject _currentInstantiatedCharacter;
        private ColorSelection _colorSelection;
        public SceneReferencer sceneReferencer;
        private SceneLoader _sceneLoader;
        private IGameFactory _gameFactory;

        public void Configure(SceneLoader sceneLoader, IGameFactory gameFactory)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
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
            LoadData();
            SetupCharacters();
        }

        private void ColorSelectorOnOnColorSelected(Color color)
        {
            StaticVariables.characterColour = color;
            SetupCharacterColours();
        }

        private void ButtonExit()
        {
            Application.Quit();
        }

        private void ButtonGo()
        {
            if (sceneReferencer && NetworkClient.active)
            {
                NetworkClient.localPlayer.GetComponent<CharacterSelection>()
                    .CmdSetupCharacter(StaticVariables.characterColour);
                sceneReferencer.CloseCharacterSelection();
            }
            else
            {
                _sceneLoader.Load(SceneNames.LobbyGame);
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
            _colorSelection = _currentInstantiatedCharacter.GetComponent<ColorSelection>();
            _currentInstantiatedCharacter.transform.SetParent(this.transform.root);

            SetupCharacterColours();
        }

        private void SetupCharacterColours()
        {
            if (StaticVariables.characterColour != new Color(0, 0, 0, 0))
            {
                _colorSelection.characterColour = StaticVariables.characterColour;
                //_colorSelection.AssignColours();
            }
        }

        private void LoadData()
        {
            if (StaticVariables.characterNumber > 0 &&
                StaticVariables.characterNumber < characterData.characterPrefabs.Length)
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
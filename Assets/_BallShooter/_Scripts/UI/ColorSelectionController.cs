using System;
using _BallShooter._Scripts.Infrastructure.Services;
using _BallShooter._Scripts.Player;
using Infrastructure;
using Infrastructure.Factory;
using Mirror.Examples.CharacterSelection;
using UnityEngine;
using UnityEngine.UI;

namespace _BallShooter._Scripts.UI
{
    public class ColorSelectionController : MonoBehaviour
    {
        public event Action OnColorConfirmed;
        
        public ColorButtonsController colorSelector;
        public Button buttonExit, buttonGo;
        public Transform podiumPosition;
        
        private GameObject _currentInstantiatedCharacter;
        private ColorSelection _colorSelection;
        private SceneLoader _sceneLoader;
        private IGameFactory _gameFactory;
        private IStaticDataService _staticDataService;

        public void Configure(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        private void OnEnable()
        {
            buttonExit.onClick.AddListener(ButtonExit);
            buttonGo.onClick.AddListener(ButtonGo);
            colorSelector.OnColorSelected += ColorSelectorOnOnColorSelected;
        }

        private void Start()
        {
            SetupCharacters();
        }

        private void OnDisable()
        {
            buttonExit.onClick.RemoveListener(ButtonExit);
            buttonGo.onClick.RemoveListener(ButtonGo);
            colorSelector.OnColorSelected -= ColorSelectorOnOnColorSelected;
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
            OnColorConfirmed?.Invoke();
        }

        private void SetupCharacters()
        {
            if (_currentInstantiatedCharacter)
            {
                Destroy(_currentInstantiatedCharacter);
            }

            _currentInstantiatedCharacter = Instantiate(_staticDataService.GameSettings.playerSettings.playerPrefab, transform.root, true);
            _currentInstantiatedCharacter.transform.position = podiumPosition.position;
            _currentInstantiatedCharacter.transform.rotation = podiumPosition.rotation;
            _colorSelection = _currentInstantiatedCharacter.GetComponent<ColorSelection>();

            SetupCharacterColours();
        }

        private void SetupCharacterColours()
        {
            if (StaticVariables.characterColour != new Color(0, 0, 0, 0))
            {
                _colorSelection.characterColour = StaticVariables.characterColour;
                _colorSelection.AssignColours();
            }
        }
    }
}
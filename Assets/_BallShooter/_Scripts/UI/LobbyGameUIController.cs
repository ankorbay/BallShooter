using UnityEngine;
using UnityEngine.UI;

namespace _BallShooter._Scripts.UI
{
    public class LobbyGameUIController : MonoBehaviour
    {
        public Button buttonCharacterSelection;

        private GameObject _characterSelectionObject;
        
        private GameObject _environment;
    
        public void Configure(GameObject characterSelectionObject, GameObject environment)
        {
            _characterSelectionObject = characterSelectionObject;
            _environment = environment;
        }
    
        private void OnEnable()
        {
            buttonCharacterSelection.onClick.AddListener(OpenColorSelection);
        }

        private void OnDisable()
        {
            buttonCharacterSelection.onClick.RemoveListener(OpenColorSelection);
        }

        private void OpenColorSelection()
        {
            _characterSelectionObject.SetActive(true);
            _environment.SetActive(false);
            this.GetComponent<Canvas>().enabled = false;
        }
        
        public void HideColorSelectorUI()
        {
            buttonCharacterSelection.gameObject.SetActive(false);
        }

        public void OpenLobbyUI()
        {
            _characterSelectionObject.SetActive(false);
            _environment.SetActive(false);
            this.GetComponent<Canvas>().enabled = true;
        }
    }

}

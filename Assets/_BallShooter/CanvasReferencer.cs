using Mirror;
using Mirror.Examples.CharacterSelection;
using UnityEngine;
using UnityEngine.UI;

public class CanvasReferencer : MonoBehaviour
{
    // Make sure to attach these Buttons in the Inspector
    public Button buttonExit, buttonGo;
    public PlayerColorSelector playerColorSelection;
    
    public Transform podiumPosition;
    private int currentlySelectedCharacter;
    private CharacterData characterData;
    private GameObject currentInstantiatedCharacter;
    private CharacterSelection characterSelection;
    public SceneReferencer sceneReferencer;
    public GameObject lobbyUIobject;

    private void Start()
    {
        characterData = CharacterData.characterDataSingleton;
        if (characterData == null)
        {
            Debug.Log("Add CharacterData prefab singleton into the scene.");
            return;
        }

        playerColorSelection.OnButtonSelected += (int index) =>
        {
            currentlySelectedCharacter = index;
            Debug.Log("currentlySelectedCharacter - " + index);
        };
        
        buttonExit.onClick.AddListener(ButtonExit);
        buttonGo.onClick.AddListener(ButtonGo);

        LoadData();
        SetupCharacters();
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    public void ButtonGo()
    {
        NetworkManager.singleton.playerPrefab = characterData.characterPrefabs[currentlySelectedCharacter];
        lobbyUIobject.SetActive(true);
    }

    public void ButtonNextCharacter()
    {
        //Debug.Log("ButtonNextCharacter");

        currentlySelectedCharacter += 1;
        if (currentlySelectedCharacter >= characterData.characterPrefabs.Length)
        {
            currentlySelectedCharacter = 0;
        }
        SetupCharacters();

        StaticVariables.characterNumber = currentlySelectedCharacter;
    }

    private void SetupCharacters()
    {
        // if (currentInstantiatedCharacter)
        // {
        //     Destroy(currentInstantiatedCharacter);
        // }
        // currentInstantiatedCharacter = Instantiate(characterData.characterPrefabs[currentlySelectedCharacter]);
        // currentInstantiatedCharacter.transform.position = podiumPosition.position;
        // currentInstantiatedCharacter.transform.rotation = podiumPosition.rotation;
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

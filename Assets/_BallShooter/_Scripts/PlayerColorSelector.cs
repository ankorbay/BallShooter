using System;
using UnityEngine;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.CharacterSelection;
using UnityEngine.UI;

public class PlayerColorSelector : MonoBehaviour
{
    public Action<int> onButtonSelected;
    
    [SerializeField]
    private List<PlayerColorButton> colorButtons;  // List of all buttons with colors

    [SerializeField] private Button goButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject lobbyUIGameobject;
    
    private PlayerColorButton _selectedColorButton;  // Currently selected color button
    private int _selectedColorIndex;

    private void OnEnable()
    {
        goButton.onClick.AddListener(OnGoClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        foreach (var colorButton in colorButtons)
        {
            // Add listener to each button to handle selection when clicked
            colorButton.button.onClick.AddListener(() => OnColorButtonClick(colorButton));
        }
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }
    private void OnGoClicked()
    {
        NetworkManager.singleton.playerPrefab =
            PlayerPrefabs.playerPrefabsSingleton.characterPrefabs[_selectedColorIndex];
       
        lobbyUIGameobject.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnColorButtonClick(PlayerColorButton colorButton)
    {
        // Set the selected color
        _selectedColorButton = colorButton;
        Debug.Log("selectedColorButton - " + _selectedColorButton);
        // Highlight the selected button and unhighlight others
        HighlightSelectedButton();
        CacheSelectedPlayerColor();
        onButtonSelected?.Invoke(colorButtons.IndexOf(colorButton));
    }
    
    void HighlightSelectedButton()
    {
        foreach (PlayerColorButton button in colorButtons)
        {
            button.outline.enabled = false;
        }

        _selectedColorButton.outline.enabled = true;
    }

    private void CacheSelectedPlayerColor()
    {
        _selectedColorIndex = colorButtons.IndexOf(_selectedColorButton);
        StaticVariables.characterNumber = _selectedColorIndex;
    }
}
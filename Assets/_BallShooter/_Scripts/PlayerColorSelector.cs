using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerColorSelector : MonoBehaviour
{
    public Action<int> OnButtonSelected;
    
    public List<PlayerColorButton> colorButtons;  // List of all buttons with colors

    private Material cachedMaterial;
    
    private PlayerColorButton selectedColorButton;  // Currently selected color button

    void Start()
    {
        foreach (var colorButton in colorButtons)
        {
            // Add listener to each button to handle selection when clicked
            colorButton.button.onClick.AddListener(() => OnColorButtonClick(colorButton));
        }
    }

    // Called when a color button is clicked
    void OnColorButtonClick(PlayerColorButton colorButton)
    {
        // Set the selected color
        selectedColorButton = colorButton;
        Debug.Log("selectedColorButton - " + selectedColorButton);
        // Highlight the selected button and unhighlight others
        HighlightSelectedButton();
        OnButtonSelected?.Invoke(colorButtons.IndexOf(colorButton));
    }

    // Highlights the selected button and removes highlight from others
    void HighlightSelectedButton()
    {
        foreach (var colorButton in colorButtons)
        {
            // If this is the selected button, enable the outline
            if (colorButton == selectedColorButton)
            {
                colorButton.outline.enabled = true;
            }
            else
            {
                // Disable the outline for all other buttons
                colorButton.outline.enabled = false;
            }
        }
    }
}
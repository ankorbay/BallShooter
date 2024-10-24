﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.CharacterSelection
{
    public class ColorButtonsController : MonoBehaviour
    {
        public event Action<Color> OnColorSelected;
    
        [SerializeField] 
        private Button[] buttons;

        private void OnValidate()
        {
            if (buttons.Length == 0)
                buttons = GetComponentsInChildren<Button>();
        }

        private void Start()
        {
            if(buttons.Length != 0)
                OnColorSelected?.Invoke(buttons[0].image.color);
        }

        private void OnEnable()
        {
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() =>
                {
                    Debug.Log("Color selected: " + button.image.color);
                    OnColorSelected?.Invoke(button.image.color);
                });
            }
        }

        private void OnDisable()
        {
            foreach (Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    // Ensure following buttons are always highlighted upon game start
    public Button mainMenuFirstButton, playFirstButton, optionsFirstButton, howToFirstButton, controlsFirstButton, creditsFirstButton;
    
    private void Start()
    {
        SetMainFirstButton();
    }

    public void SetMainFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null); // Reset before selecting button
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton.gameObject);
        mainMenuFirstButton.Select();
    }

    public void SetPlayFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playFirstButton.gameObject);
        playFirstButton.Select();
    }

    public void SetOptionsFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton.gameObject);
        optionsFirstButton.Select();
    }

    public void SetHowToPlayFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(howToFirstButton.gameObject);
        howToFirstButton.Select();
    }

    public void SetControlsFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton.gameObject);
        controlsFirstButton.Select();
    }

    public void SetCreditsFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton.gameObject);
        creditsFirstButton.Select();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}

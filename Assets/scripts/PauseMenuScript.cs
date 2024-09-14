using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    // Ensure following buttons are always highlighted upon game start
    public Button pauseMenuFirstButton, controlsFirstButton, howToFirstButton, optionsFirstButton;
    [SerializeField] private Animator transitionAnim;

    private void Start()
    {
        SetPauseFirstButton();
    }

    public void SetPauseFirstButton()
    {
        pauseMenuFirstButton.Select();
    }

    public void SetControlsFirstButton()
    {
        controlsFirstButton.Select();
    }

    public void SetHowToFirstButton()
    {
        howToFirstButton.Select();
    }

    public void SetOptionsFirstButton()
    {
        optionsFirstButton.Select();
    }

    public void MainMenuButton()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator LoadMainMenu()
    {
        Time.timeScale = 1;
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(0);
    }
}

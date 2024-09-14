using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseMenuScript : MonoBehaviour
{
    public Button restartFirstButton; // Set first button for controller/keyboard support
    [SerializeField] private Animator transitionAnim;

    private void Start()
    {
        SetRestartFirstButton();
    }

    public void RestartButton()
    {
        StartCoroutine(LoadFirstLevel());
    }

    public void MainMenuButton()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void SetRestartFirstButton()
    {
        restartFirstButton.Select();
    }

    IEnumerator LoadFirstLevel()
    {
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
    }

    IEnumerator LoadMainMenu()
    {
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(0);
    }
}

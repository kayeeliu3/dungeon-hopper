using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyScript : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;

    // Store difficulty in player prefs
    // 1 = Easy, 2 = Medium, 3 = Hard
    public void SetDifficulty(int diff)
    {
        PlayerPrefs.SetInt("Difficulty", diff);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

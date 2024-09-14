using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Animator transitionAnim; 
    [SerializeField] private Button skipButton;

    private void Start()
    {
        skipButton.Select();
    }

    public void ClearText()
    {
        dialogueText.text = "";
    }

    public void FirstTextDialogue()
    {
        dialogueText.text = "Hey you!";
    }
    
    public void SecondTextDialogue()
    {
        dialogueText.text = "Many mines inside!!";
    }

    public void ThirdTextDialogue()
    {
        dialogueText.text = "Get rid of them!!!";
    }

    public void FourthTextDialogue()
    {
        dialogueText.text = "Great! Bye!";
    }

    public void PlayerTextDialogue()
    {
        dialogueText.text = ". . .";
    }

    public void PlayerTextDialogueTwo()
    {
        dialogueText.text = "Not much choice now.";
    }

    public void SkipButton()
    {
        StartCoroutine(CutsceneEnd());
    }

    public IEnumerator CutsceneEnd()
    {
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

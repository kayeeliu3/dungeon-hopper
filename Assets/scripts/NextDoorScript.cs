using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class NextDoorScript : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject textAnimPrefab;
    private NextLevelSceneSwitch nextLevelScript;
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private UnityEvent stopPlayerInputTrigger;

    private void Start()
    {
        nextLevelScript = GameObject.Find("Walls").GetComponent<NextLevelSceneSwitch>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && col != null)
        {
            if (!nextLevelScript.isOpen)
            {
                GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
                boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Incomplete board...");
            }
            else
            {
                stopPlayerInputTrigger.Invoke();
                //Debug.Log("Changing scene...");
                StartCoroutine(LoadNextLevel());
            }
        }
    }

    IEnumerator LoadNextLevel()
    {
        transitionAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

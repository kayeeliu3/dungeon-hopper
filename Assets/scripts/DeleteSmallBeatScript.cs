using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events; 

public class DeleteSmallBeatScript : MonoBehaviour
{
    public BeatTrackerScript beatTracker;
    public Transform playerTransform;
    public GameObject textAnimPrefab;
    public int missedBeatCount; // Three consecutive missed beats spawns enemies

    [SerializeField] private UnityEvent tooManyMissedBeatsTrigger;

    private void Start()
    {
        missedBeatCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D  col)
    {
        if (col.gameObject.tag == "SmallBeat" && col != null)
        {
            missedBeatCount++;
            beatTracker.isSmallerBeatColliding = false;
            Destroy(col.gameObject);
            if (missedBeatCount == 3 && (PlayerPrefs.GetInt("Difficulty") != 1)) // Only spawn enemies on med/hard difficulty
            {
                SpawnEnemy();
                missedBeatCount = 0; // Reset
            }
            else
            {
                GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
                boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Missed beat...");
            }
        }
    }

    // Three consecutive missed beats spawns enemies
    private void SpawnEnemy()
    {
        GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
        boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Watch out!");
        tooManyMissedBeatsTrigger.Invoke();
    }
}

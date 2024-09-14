using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FalseDoorScript : MonoBehaviour
{   
    public Transform playerTransform;
    public GameObject textAnimPrefab;
    public AudioSource incompleteExitSound;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && col != null)
        {
            GameObject boardCompleteText = Instantiate(textAnimPrefab, playerTransform);
            boardCompleteText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("No turning back now...");
            incompleteExitSound.Play();
        }
    }
}

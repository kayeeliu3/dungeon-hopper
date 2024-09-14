using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBeatScript : MonoBehaviour
{
    private AudioManagerScript audioScript;
    private Transform mainBeatTracker;

    private void Start()
    {
        mainBeatTracker = GameObject.Find("BeatTracker").transform;
        audioScript = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        // Align small beat above main beat tracker
        transform.position = new Vector3(mainBeatTracker.position.x, mainBeatTracker.position.y + 150f, mainBeatTracker.position.z);
    }

    private void Update()
    {
        // Update position towards the main beat tracker based on the song BPM so they collide on the beat
        transform.position -= new Vector3(0f, audioScript.bpm / .5f * Time.deltaTime, 0f);
    }
}

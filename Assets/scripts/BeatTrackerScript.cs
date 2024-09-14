using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTrackerScript : MonoBehaviour
{
    public int beatSpeed;
    public bool isSmallerBeatColliding;
    public Collider2D currentSmallBeat;
    private Vector3 startSize;

    private void Start()
    {
        isSmallerBeatColliding = false;
    }

    // Set bool to true when small circle collides with main beat tracker
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "SmallBeat" && col != null)
        {
            isSmallerBeatColliding = true;
            currentSmallBeat = col;
        }
    }

    // Upon successful move, destroy small beat object
    public void DeleteSmallBeat()
    {
        if (currentSmallBeat != null)
        {
            Destroy(currentSmallBeat.gameObject);
            isSmallerBeatColliding = false;
        }
    }
}

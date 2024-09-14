using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float pulseSize;
    public float returnSpeed = 5f;
    private Vector3 startSize;
    
    private void Start()
    {
        startSize = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, returnSpeed * Time.deltaTime);
    }

    public void PulseObject()
    {
        transform.localScale = startSize * pulseSize;
    }
}

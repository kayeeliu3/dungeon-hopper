using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player; // Track player
    [SerializeField] private float speed;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        Vector3 trackedPos = player.position + offset;
        // Camera tracking, lerp allows smoother following
        transform.position = Vector3.Lerp(transform.position, trackedPos, speed * Time.deltaTime);
    }
}

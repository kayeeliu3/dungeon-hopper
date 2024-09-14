using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimEffect : MonoBehaviour
{
    // Destroy parent holding text animation
    public void DestroyAnim()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}

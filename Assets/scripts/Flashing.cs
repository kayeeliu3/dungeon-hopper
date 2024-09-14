using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour
{
    [SerializeField] private Material flashMaterial; // material to switch when flashing
    [SerializeField] private float duration;
    
    private SpriteRenderer sRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = sRenderer.material; // reference to revert back to original material
    }

    public void FlashEffect()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine); // ensure multiple routines are not running
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        sRenderer.material = originalMaterial;
        flashRoutine = null;
    }
}

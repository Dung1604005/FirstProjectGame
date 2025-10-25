using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;

    private IEnumerator FlashRoutine(SpriteRenderer renderer, Material defaultMaterial)
    {

        Material clonedMaterial = new Material(defaultMaterial);

        // Đặt material của renderer thành flash material
        renderer.material = flashMaterial;
    
        yield return new WaitForSeconds(flashDuration);
        if (renderer != null)
        {

            Debug.Log("End flash");
            renderer.material = clonedMaterial;
        }


    }

    public void Flash(SpriteRenderer spriteRenderer, ref Coroutine flashRoutine)
    {
        Material defaultMaterial ;
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        defaultMaterial = spriteRenderer.material;
        flashRoutine = StartCoroutine(FlashRoutine(spriteRenderer, defaultMaterial));

    }

}

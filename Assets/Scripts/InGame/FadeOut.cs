using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void OnEnable()
    {
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
        StartCoroutine(ChangeAlpha());
    }

    IEnumerator ChangeAlpha()
    {
        while (spriteRenderer.color.a > 0.01f)
        {
            Color color = spriteRenderer.color;
            color.a -= Time.deltaTime;
            spriteRenderer.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
}

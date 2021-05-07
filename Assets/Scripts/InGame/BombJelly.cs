using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombJelly : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(ChangeColor());
    }

    void OnDisable()
    {
        StopCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            
            yield return null;
        }
    }
}

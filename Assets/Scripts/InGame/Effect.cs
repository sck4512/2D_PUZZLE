using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("Disable", 0.5f);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}

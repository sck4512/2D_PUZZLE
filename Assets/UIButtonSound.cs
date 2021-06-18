using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void PlayUIButtonSound()
    {
        audioSource.Play();
    }
}

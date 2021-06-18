using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGType
{
    Title, Challenge, Stage
}

public class SoundManager : GenericSingleton<SoundManager>
{
    AudioSource audioSources;
    [SerializeField] AudioClip[] bgms;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        audioSources = GetComponent<AudioSource>();
        PlayBG(BGType.Title);
    }

    public void PlayBG(BGType _Type)
    {
        audioSources.clip = bgms[(int)_Type];
        StopCoroutine("PlayBGRoutine");
        StartCoroutine("PlayBGRoutine");
    }

    IEnumerator PlayBGRoutine()
    {
        while (true)
        {
            if (!audioSources.isPlaying)
                audioSources.Play();
            yield return new WaitForEndOfFrame();
        }
    }

}

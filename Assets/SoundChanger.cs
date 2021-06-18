using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundChanger : MonoBehaviour
{
    [SerializeField] BGType type;
    [SerializeField] Slider slider;
    [SerializeField] Button bgmButton;
    [SerializeField] AudioSource bgmSource;

    void Start()
    {
        SoundManager.Instance.PlayBG(type);
        slider.value = bgmSource.volume;
        bgmButton.interactable = bgmSource.volume > 0;
    }

    public void ChangeBGMVolume()
    {
        bgmSource.volume = slider.value;
        bgmButton.interactable = bgmSource.volume > 0;
    }


    public void ChangeBGMVolume(float _Volume)
    {
        slider.value = _Volume;
        bgmSource.volume = slider.value;
        bgmButton.interactable = bgmSource.volume > 0;
    }
}

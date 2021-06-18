using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChanger : MonoBehaviour
{
    [SerializeField] BGType type;

    void Start()
    {
        SoundManager.Instance.PlayBG(type);    
    }
}

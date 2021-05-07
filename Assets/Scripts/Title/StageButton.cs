using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image[] starImages;
    [SerializeField] GameObject textAndStar;
    bool isCleared = false;
    int starCount = 0;

    public void UpdateStageLevelStarRank(int _Rank)
    {
        textAndStar.SetActive(true);
        if (_Rank == 0)
            return;
        for(int i = 0; i < _Rank; i++)
        {
            starImages[i].gameObject.SetActive(true);
        }
    }

    public void UpdateButtonImageAndClickeble(Sprite _UnLockImage)
    {
        image.sprite = _UnLockImage;
        image.raycastTarget = true;
    }
}

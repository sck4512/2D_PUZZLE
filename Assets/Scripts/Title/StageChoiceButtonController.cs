using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoiceButtonController : MonoBehaviour
{
    [SerializeField] StageButton[] stageButtons;


    void Awake()
    {
        int buttonCount = transform.GetChild(1).childCount; 
        stageButtons = new StageButton[buttonCount];
        for(int i = 0; i < buttonCount; i++)
        {
            stageButtons[i] = transform.GetChild(1).GetChild(i).GetComponent<StageButton>();
        }

        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void UpdateStageChoiceButtonRankStar(int _OpendStage , int[] _Rank, Sprite _UnlockImage)
    {
        for(int i = 0; i < _OpendStage; i++)
        {
            stageButtons[i].UpdateStageLevelStarRank(_Rank[i]);
            stageButtons[i].UpdateButtonImageAndClickeble(_UnlockImage);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleUIManager : MonoBehaviour
{
    [SerializeField] StageChoiceButtonController stageChoiceButtonController;

    [SerializeField] Sprite unlockSprite;
    [SerializeField] GameObject stageChoicePanel;
    [SerializeField] GameObject settingObject;
    [SerializeField] GameObject noDataMessagePanel;
    [SerializeField] GameObject noRankDataMessagePanel;
    [SerializeField] GameObject noDeleteFileMessagePanel;

    [SerializeField] Button stageButton;
    [SerializeField] Button challengeButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button rankingButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button deleteContinueGameFileButton;
    [SerializeField] Button helpButton;


    [SerializeField] SceneFadeEffect sceneFadeEffect;

    void SetAllMainButtoninteractable(bool _Interactable)
    {
        stageButton.interactable = _Interactable;
        challengeButton.interactable = _Interactable;
        continueButton.interactable = _Interactable;
        exitButton.interactable = _Interactable;
        rankingButton.interactable = _Interactable;
        settingButton.interactable = _Interactable;
        deleteContinueGameFileButton.interactable = _Interactable;
        helpButton.interactable = _Interactable;
    }


    public void OnButtonEvent(GameObject _gameObject)
    {
        _gameObject.SetActive(true);
        SetAllMainButtoninteractable(false);
    }

    public void OffButtonEvent(GameObject _gameObject)
    {
        _gameObject.SetActive(false);
        SetAllMainButtoninteractable(true);
    }


    public void ChallengeButtonEvent()
    {
        SceneManager.LoadScene("Challenge");
    }

    public void ContinueButtonEvent()
    {
        bool IsExit = DataManager.Instance.jsonManager.LoadPlayData();
        if (!IsExit)
        {
            noDataMessagePanel.SetActive(true);
            SetAllMainButtoninteractable(false);
            return;
        }

        
        sceneFadeEffect.LoadScene(1);
        //SceneManager.LoadScene("Challenge");
    }

    public void ExitButtonEvent()
    {
        Application.Quit();
    }

    public void RankingButtonEvent()
    {
        if(DataManager.Instance.jsonManager.JsonRankSavedData == null)
        {
            SetAllMainButtoninteractable(false);
            noRankDataMessagePanel.SetActive(true);
            return;
        }


        SceneManager.LoadScene("ChallengeRanking");
    }


    public void DeleteFileButtonEvent()
    {
        if(!System.IO.File.Exists(DataManager.Instance.jsonManager.ContinueFilePath))
        {
            noDeleteFileMessagePanel.SetActive(true);
            SetAllMainButtoninteractable(false);
            return;
        }
        DataManager.Instance.jsonManager.DeleteFile();
    }

  

    public void StageChoiceButtonEvent(int _StageNumber)
    {
        SceneManager.LoadScene("Stage_" + _StageNumber.ToString());
    }


    //해당 Title신이 불러올때마다 호출되어서 업데이트 해줌
    void Start()
    {
        DataManager.Instance.opendStage = 10;
        stageChoiceButtonController.UpdateStageChoiceButtonRankStar(DataManager.Instance.opendStage, DataManager.Instance.stageRank, unlockSprite);
    }
}



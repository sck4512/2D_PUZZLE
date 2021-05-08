using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayUIManager : GenericSingleton<PlayUIManager>
{
    [Header("Stage")]
    [SerializeField] Image timer;

    [Header("Common")]
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Image[] scoreNumbers;
    [SerializeField] Image[] comboNumbers;
    [SerializeField] Sprite playButtonSprite;
    [SerializeField] Sprite stopButtonSprite;
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject exitPanel;
    [SerializeField] TextMeshProUGUI scoreRenderText;
    [SerializeField] Transform comboRenderTransform;
    [SerializeField] GameObject gameOverMessageBox;

    [Header("Challenge")]
    [SerializeField] TextMeshProUGUI playTimeText;
    [SerializeField] Image lifeGaugeImage;
    [SerializeField] GameObject gaugeUI;
    [SerializeField] GameObject savedMessageBox;

    //Stage
    GameObject[] stars;
    Transform[] gameOverDatas;
    Button nextStageButton;
    //Challenge
    Button gaugeButton;
    Image gaugeImage;

    public void RenderScore(int _Score)
    {
        scoreRenderText.text = _Score.ToString();
    }

    public void RenderCombo(int _Combo)
    {
        //0이면 아무것도 출력 안 함
        if (_Combo == 0)
            return;

        var splittedNum = SplitNumber(_Combo);
        int length = splittedNum.Count;
        float startPosX = (length / 2) * -1 + comboRenderTransform.position.x;
        if (length % 2 != 0)
            startPosX -= 0.5f;

        for(int i = 0; i < length; i++)
        {
            int temp = splittedNum.Pop();
            var number = GameManager.Instance.Pool.GetComboNumber(temp);
            number.SetActive(true);
            number.transform.position = new Vector3(startPosX + i, comboRenderTransform.position.y, 0);
        }
    }

    Stack<int> SplitNumber(int _Number)
    {
        Stack<int> splittedNumber = new Stack<int>();
        int length = _Number.ToString().Length;
        int number = _Number;

        for(int i = 0; i < length - 1; i++)
        {
            int Temp = number % 10;
            splittedNumber.Push(Temp);
            number /= 10;
        }
        splittedNumber.Push(number);
        return splittedNumber;
    }

    //버튼 이벤트들
    public void PauseButtonEvent()
    {
        pauseButton.image.sprite = playButtonSprite;
        pauseButton.interactable = false;
        //Time.timeScale = 0;
        GameManager.Instance.SetIsWaiting(true);
        pausePanel.SetActive(true);
    }

    public void ResumeButtonEvent()
    {
        pauseButton.image.sprite = stopButtonSprite;
        pauseButton.interactable = true;
        //Time.timeScale = 1;
        GameManager.Instance.SetIsWaiting(false);
        pausePanel.SetActive(false);
    }

    public void SaveButtonEvent()
    {
        DataManager.Instance.jsonManager.SavePlayData();
        savedMessageBox.SetActive(true);
    }

    public void SavedMessageBoxOkayButtonEvent()
    {
        savedMessageBox.SetActive(false);
    }

    //Challenge용
    public void RestartButtonEvent()
    {
        //Time.timeScale = 1;
        GameManager.Instance.SetIsWaiting(false);
        SceneManager.LoadScene("Challenge");
        //UI로 인한 Timescale 되돌리기
    }

    public void RestartButtonEvent(int _StageIndex)
    {
        //Time.timeScale = 1;
        GameManager.Instance.SetIsWaiting(false);
        SceneManager.LoadScene("Stage_" + _StageIndex.ToString());
    }

    public void HomeButtonEvent()
    {
        //UI로 인한 Timescale 되돌리기
        //Time.timeScale = 1;
        GameManager.Instance.SetIsWaiting(false);
        SceneManager.LoadScene("Title");
    }


    public void ExitPanelExitButtonEvent()
    {
        Application.Quit();
    }


    public void OnButtonEvent(GameObject _gameObject)
    {
        _gameObject.SetActive(true);
    }

    public void OffButtonEvent(GameObject _gameObject)
    {
        _gameObject.SetActive(false);
    }

    public void UpdateSamePangGauge(float _Gauge)
    {
        //최대 게이지 기준은 100f
        float Gauge = _Gauge / 100f;
        //여기서 초기화
        if(Gauge == 0f)
        {
            gaugeImage.fillAmount = 0;
            return;
        }


        Gauge += gaugeImage.fillAmount;
        if (Gauge > 1)
        {
            Gauge = 1;
            gaugeButton.interactable = true;
        }
        
        gaugeImage.fillAmount = Gauge;
    }

    public void GaugeButtonEvent()
    {
        var jelly = (Type)Random.Range(0, 6);
        GameManager.Instance.DoPangSameJelly(jelly);

        GameManager.Instance.GaugeUpdateFunctions(0f);
        gaugeButton.interactable = false;
    }

    //자신 라이프 그대로 넣음
    public void UpdateLifeGauge(float _Life, float _MaxLife)
    {
        lifeGaugeImage.fillAmount = _Life / _MaxLife;
    }


    //새롭게 시작할 때 세팅
    public void SetContinueGameGauge(float _Gauge)
    {
        GameManager.Instance.GaugeUpdateFunctions(_Gauge);
        PauseButtonEvent();
    }

    public void GameOverMessageBoxOkayButtonEvent()
    {
        gameOverMessageBox.SetActive(false);
        PauseButtonEvent();
    }

    //게임끝났을때 Time.timeScale = 0으로
    public void GameOverMessageBoxEnableEvent()
    {
        //Time.timeScale = 0;
        GameManager.Instance.SetIsWaiting(true);
        pauseButton.interactable = false;
        gameOverMessageBox.SetActive(true);

        StartCoroutine(GameOverMessageBoxEnableEventRoutine());
    }

    IEnumerator GameOverMessageBoxEnableEventRoutine()
    {
        Vector3[] targetPos = new Vector3[gameOverDatas.Length];
        for(int i = 0; i < gameOverDatas.Length; i++)
        {
            targetPos[i] = gameOverDatas[i].transform.position;
            gameOverDatas[i].transform.position -= Vector3.right * 800f;
        }

        for(int i = 0; i < gameOverDatas.Length; i++)
        {
            while (Vector3.Distance(targetPos[i], gameOverDatas[i].transform.position) > 0.2f)
            {
                gameOverDatas[i].transform.position += Vector3.right * 0.02f * 800f;
                yield return new WaitForSecondsRealtime(0.001f);
            }
            gameOverDatas[i].transform.position = targetPos[i];

            if(i == 0)
            {
                //도착하고 난 뒤
                float elapsedTime = 0;
                while (elapsedTime < 3f)
                {
                    //숫자 셔플
                    for(int index = 0; index < scoreNumbers.Length; index++)
                    {
                        int randomNumber = Random.Range((int)0, (int)10);
                        scoreNumbers[index].sprite = numberSprites[randomNumber];
                    }
                    elapsedTime += 0.02f;
                    yield return null;
                }

                //끝나고 난 뒤
                var SplittedNumber = SplitNumber(GameManager.Instance.Score);
                int length = SplittedNumber.Count;

                if(length > scoreNumbers.Length)
                {
                    for(int Index = 0; Index < scoreNumbers.Length; Index++)
                    {
                        scoreNumbers[Index].sprite = numberSprites[9];
                    }
                    continue;
                }

                for(int Index = 0; Index < scoreNumbers.Length; Index++)
                {
                    scoreNumbers[Index].sprite = numberSprites[0];
                }


                for (int Index = scoreNumbers.Length - length; Index < scoreNumbers.Length; Index++)
                {
                    int num = SplittedNumber.Pop();
                    scoreNumbers[Index].sprite = numberSprites[num];
                }
            }
            else if(i == 1)
            {
                //도착하고 난 뒤
                float elapsedTime = 0;
                while (elapsedTime < 3f)
                {
                    //숫자 셔플
                    for (int index = 0; index < comboNumbers.Length; index++)
                    {
                        int randomNumber = Random.Range((int)0, (int)10);
                        comboNumbers[index].sprite = numberSprites[randomNumber];
                    }
                    elapsedTime += 0.02f;
                    yield return null;
                }

                var SplittedNumber = SplitNumber(GameManager.Instance.MaxCombo);
                int length = SplittedNumber.Count;

                if(length > comboNumbers.Length)
                {
                    for(int Index = 0; Index < comboNumbers.Length; Index++)
                    {
                        comboNumbers[Index].sprite = numberSprites[9];
                    }
                    continue;
                }

                for (int Index = 0; Index < comboNumbers.Length; Index++)
                {
                    comboNumbers[Index].sprite = numberSprites[0];
                }
                for (int Index = comboNumbers.Length - length; Index < comboNumbers.Length; Index++)
                {
                    int num = SplittedNumber.Pop();
                    comboNumbers[Index].sprite = numberSprites[num];
                }
            }
            else if (i == 2) //스코어
            {
                //Challange일때는 시간
                if (GameManager.Instance.CurStage == 0)
                {
                    //시간 계산
                    int minute = GameManager.Instance.PlayTime / 60;
                    int second = GameManager.Instance.PlayTime % 60;

                    string secondString = second.ToString();
                    if (second < 10)
                        secondString = '0' + secondString;

                    playTimeText.text = minute.ToString() + " : " + secondString;
                }
                else
                {
                    //랭크를 받아와서 저장(랭크 = 별 갯수)
                    int Rank = GameManager.Instance.Rank;
                    if (Rank > DataManager.Instance.stageRank[GameManager.Instance.CurStage - 1])
                        DataManager.Instance.stageRank[GameManager.Instance.CurStage - 1] = Rank;
                    
                    //별 갯수 업데이트
                    for (int Index = 0; Index < Rank; Index++)
                    {
                        gameOverDatas[i].transform.GetChild(0).GetChild(Index).gameObject.SetActive(true);
                        yield return new WaitForSecondsRealtime(0.1f);
                    }

                    //Rank가 0이상이어야만 저장가능
                    if(Rank > 0)
                    {
                        //데이터에 저장해줌
                        if (GameManager.Instance.CurStage > DataManager.Instance.opendStage)
                            DataManager.Instance.opendStage = GameManager.Instance.CurStage;
                    }
                }
            }
            else if (i == 3) //버튼의 경우
            {
                if (GameManager.Instance.Rank == 0 && GameManager.Instance.CurStage != 10)
                    nextStageButton.interactable = false;
            }
        }

        yield return null;
    }
       

    public void UpdateTimer(int _curTime, int _MaxTime)
    {
        //                        형변환
        timer.fillAmount = ((float)_curTime) / ((float)_MaxTime);
    }
    

    void Awake()
    {
        if (GameManager.Instance.CurStage == 0)
        {
            gaugeButton = gaugeUI.GetComponent<Button>();
            gaugeImage = gaugeUI.GetComponent<Image>();
        }
        gameOverDatas = new Transform[4];
        //게임 끝나는 데이터 부분 일일이 캐싱 안 하고 넣어줌
        for(int i = 0; i < gameOverDatas.Length; i++)
        {
            gameOverDatas[i] = gameOverMessageBox.transform.GetChild(i);
        }

        //마지막 스테이지는 안함
        if (GameManager.Instance.CurStage == 10 || GameManager.instance.CurStage == 0)
            return;
        //맨 마지막에 있는 버튼임
        nextStageButton = gameOverDatas[gameOverDatas.Length - 1].GetChild(2).GetComponent<Button>();
    }

}

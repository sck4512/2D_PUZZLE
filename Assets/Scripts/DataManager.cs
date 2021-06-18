using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DataManager : GenericSingleton<DataManager>
{
    public JsonManager jsonManager { get; private set; }
    public int MaxCombo { get; private set; }
    public int MaxScore { get; private set; }
    public int MaxPlayTime { get; private set; }
    public Dictionary<Type, int> PangedJellyCount;
    public int opendStage = 4;  // 0이면 전부 해금 못한 상태
    public int[] stageRank; //0이면 별 0개
    bool? isLoaded;

    public void SetComboMax(int _ComboMax)
    {
        MaxCombo = _ComboMax;
    }

    public void SetScoreMax(int _ScoreMax)
    {
        MaxScore = _ScoreMax;
    }

    public void SetPlayTimeMax(int _PlayTime)
    {
        MaxPlayTime = _PlayTime;
    }

    public void AddPangedJellyCount(Type _Type, int _Count = 1)
    {
        //폭탄 종류는 저장 안 함
        if (_Type == Type.Bomb || _Type == Type.LineBomb)
            return;

        PangedJellyCount[_Type] += _Count;
    }

    void SetRankData()
    {
        if (isLoaded == false || isLoaded == null)
            return;

        var jsonRankData = jsonManager.JsonRankSavedData;
        MaxCombo = jsonRankData.MaxCombo;
        MaxScore = jsonRankData.MaxScore;
        MaxPlayTime = jsonRankData.MaxPlayTime;

        PangedJellyCount[Type.Yellow] = jsonRankData.PangedYellowCount;
        PangedJellyCount[Type.Red] = jsonRankData.PangedRedCount;
        PangedJellyCount[Type.Blue] = jsonRankData.PangedBlueCount;
        PangedJellyCount[Type.Gray] = jsonRankData.PangedGrayCount;
        PangedJellyCount[Type.Green] = jsonRankData.PangedGreenCount;
        PangedJellyCount[Type.Purple] = jsonRankData.PangedPurpleCount;
    }

    void SetStageData()
    {
        var jsonData = jsonManager?.JsonStageSavedData;
        if (jsonData == null)
            return;
        opendStage = jsonData.OpendStage;
        for(int i = 0; i < stageRank.Length; i++)
        {
            stageRank[i] = jsonData.StageRank[i];
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        jsonManager = GetComponent<JsonManager>();

        PangedJellyCount = new Dictionary<Type, int>();
        PangedJellyCount.Add(Type.Yellow, 0);
        PangedJellyCount.Add(Type.Blue, 0);
        PangedJellyCount.Add(Type.Red, 0);
        PangedJellyCount.Add(Type.Gray, 0);
        PangedJellyCount.Add(Type.Purple, 0);
        PangedJellyCount.Add(Type.Green, 0);

        stageRank = new int[10];


        //시작하자마자 Stage 관련 데이터 불러옴
        jsonManager?.LoadStageData();
        //시작하자마자 랭크 관련 데이터 불러옴
        isLoaded = jsonManager?.LoadRankData();

        //데이터 적용
        SetStageData();
        SetRankData();
    }
}

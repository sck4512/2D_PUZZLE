using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class JsonPlayData
{
    public PlayData playData;
    public MapData[] mapData;
}


//«√∑π¿Ã===============================
[Serializable]
public class PlayData
{
    public int CurCombo;
    public int MaxCombo;
    public float Gauge;
    public int Score;
    public float Life;
}

[Serializable]
public class MapData
{
    public int JellyType;
}

//==================================


[SerializeField]
public class JsonRankData
{
    public int MaxCombo;
    public int MaxScore;
    public int MaxPlayTime;

    public int PangedYellowCount;
    public int PangedRedCount;
    public int PangedBlueCount;
    public int PangedGreenCount;
    public int PangedGrayCount;
    public int PangedPurpleCount;
}

//Stage
[Serializable]
public class JsonStageData
{
    public int OpendStage;
    public int[] StageRank;
}

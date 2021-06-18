using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class JsonManager : MonoBehaviour
{
    public string ContinueFilePath { get; private set; }
    string rankFilePath;
    string stageFilePath;
    public JsonPlayData JsonPlaySavedData { get; private set; } = null;
    public JsonRankData JsonRankSavedData { get; private set; } = null;
    public JsonStageData JsonStageSavedData { get; private set; } = null;

    void Awake()
    {
        ContinueFilePath = Path.Combine(Application.persistentDataPath, "JsonPlayData.json");
        rankFilePath = Path.Combine(Application.persistentDataPath, "JsonRankData.json");
        stageFilePath = Path.Combine(Application.persistentDataPath, "JsonStageData.json");
    }

    public void SavePlayData()
    {
        JsonPlayData jsonData = new JsonPlayData();
        jsonData.playData = new PlayData();
        //Challenge Save Data
        jsonData.playData.CurCombo = GameManager.Instance.Combo;
        jsonData.playData.MaxCombo = GameManager.Instance.MaxCombo;
        jsonData.playData.Gauge = GameManager.Instance.Gauge;
        jsonData.playData.Life = GameManager.Instance.Life;
        jsonData.playData.Score = GameManager.Instance.Score;


        int lengthY = GameManager.Instance.lengthY;
        int lengthX = GameManager.Instance.lengthX;
        jsonData.mapData = new MapData[lengthY * lengthX];

        for(int y = 0; y < lengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                jsonData.mapData[lengthX * y + x] = new MapData();
                jsonData.mapData[lengthX * y + x].JellyType = (int)GameManager.Instance.GameMap[y, x].type;
            }
        }

        string data = JsonUtility.ToJson(jsonData);
        FileStream fileStream = new FileStream(ContinueFilePath, FileMode.OpenOrCreate);
        byte[] arr = Encoding.UTF8.GetBytes(data);
        fileStream.Write(arr, 0, arr.Length);
        fileStream.Close();
    }

    public bool LoadPlayData()
    {
        if (!File.Exists(ContinueFilePath))
            return false;
        //string jsonStringData = File.ReadAllText(continueFilePath);
        string jsonStringData;
        FileStream fileStream = new FileStream(ContinueFilePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);
        jsonStringData = streamReader.ReadLine();

        JsonPlaySavedData = JsonUtility.FromJson<JsonPlayData>(jsonStringData);
        return true;
    }

    //현재 JsonData에 담긴 파일을 제거
    public void RemoveSavedData()
    {
        JsonPlaySavedData = null;
    }

    public void DeleteFile()
    {
        if (!File.Exists(ContinueFilePath))
            return;
        File.Delete(ContinueFilePath);
    }



    public void SaveRankData()
    {
        JsonRankData jsonRankData = new JsonRankData();
        jsonRankData.MaxCombo = DataManager.Instance.MaxCombo;
        jsonRankData.MaxScore = DataManager.Instance.MaxScore;
        jsonRankData.MaxPlayTime = DataManager.Instance.MaxPlayTime;

        jsonRankData.PangedYellowCount = DataManager.Instance.PangedJellyCount[Type.Yellow];
        jsonRankData.PangedRedCount = DataManager.Instance.PangedJellyCount[Type.Red];
        jsonRankData.PangedGreenCount = DataManager.Instance.PangedJellyCount[Type.Green];
        jsonRankData.PangedGrayCount = DataManager.Instance.PangedJellyCount[Type.Gray];
        jsonRankData.PangedBlueCount = DataManager.Instance.PangedJellyCount[Type.Blue];
        jsonRankData.PangedPurpleCount = DataManager.Instance.PangedJellyCount[Type.Purple];

        string saveData = JsonUtility.ToJson(jsonRankData);
        FileStream fileStream = new FileStream(rankFilePath, FileMode.OpenOrCreate, FileAccess.Write);
        byte[] arr = Encoding.UTF8.GetBytes(saveData);
        fileStream.Write(arr, 0, arr.Length);
        fileStream.Close();
    }

    public bool LoadRankData()
    {
        if (!File.Exists(rankFilePath))
            return false;

        FileStream fileStream = new FileStream(rankFilePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);
        string loadRankData = streamReader.ReadLine();
        JsonRankSavedData = JsonUtility.FromJson<JsonRankData>(loadRankData);
        return true;
    }


    public void SaveStageData()
    {
        JsonStageData jsonStageData = new JsonStageData();
        jsonStageData.OpendStage = DataManager.Instance.opendStage;
        jsonStageData.StageRank = new int[DataManager.Instance.stageRank.Length];
        for(int i = 0; i < jsonStageData.StageRank.Length; i++)
        {
            jsonStageData.StageRank[i] = DataManager.Instance.stageRank[i];
        }

        string jsonStageStringData = JsonUtility.ToJson(jsonStageData);
        FileStream fileStream = new FileStream(stageFilePath, FileMode.OpenOrCreate, FileAccess.Write);
        byte[] arr = Encoding.UTF8.GetBytes(jsonStageStringData);
        fileStream.Write(arr, 0, arr.Length);
        fileStream.Close();
    }

    public void LoadStageData()
    {
        if (!File.Exists(stageFilePath))
            return;
        FileStream fileStream = new FileStream(stageFilePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);
        string data = streamReader.ReadLine();
        JsonStageSavedData =  JsonUtility.FromJson<JsonStageData>(data);
    }
}

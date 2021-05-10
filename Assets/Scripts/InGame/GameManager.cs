using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

//������
using Debug = UnityEngine.Debug;

public struct JellyMapData
{
    public int yMin;

    public JellyMapData(int _yMin)
    {
        yMin = _yMin;
    }
}

public class GameManager : GenericSingleton<GameManager>
{

    [SerializeField] int curStage; //0�� Challenge
    public int CurStage { get => curStage; }
    int[] targetMiss;
    List<int[]> rankCondition;
    public int MissCount = 0;
    int curTime = 40;
    int TotalScore { get => Score + Combo * 5 - MissCount * 10; }
    public int Rank
    {
        get
        {
            int totalScore = TotalScore;
            if (totalScore < rankCondition[curStage - 1][0])
                return 0;
            else if (totalScore < rankCondition[curStage - 1][1])
                return 1;
            else if (totalScore < rankCondition[curStage - 1][2])
                return 2;
            else
                return 3;
        }
    }
    //============================================================


    [SerializeField] Transform map;
    public bool IsWaiting { get; private set; }
    public PoolManager Pool { get; private set; }
    Transform poolTransform;


    public int lengthX = 20;
    //���� ���� ���� 15�̰� 15�� ����
    public int lengthY = 30;
    [HideInInspector] public int HalfLengthY;
    public Jelly[,] GameMap { get; private set; }
    public bool IsPangSuccess { get; private set; } //��� �� �����ߴ��� üũ �ƴϸ� �޺� ���� �� ü�� ����
    int maxBombCount = 6;
    int bombCount = 0;
    bool isGameOver = false;
    //������Ʈ �����Ű�� ��������Ʈ
    Action updates;
    

    Stopwatch stopWatch;
    [HideInInspector] public int MaxCombo;
    [HideInInspector] public int Combo;
    [HideInInspector] public int Score;
    public int PlayTime { get; private set; } = 0;
    [HideInInspector] public float Gauge;
    [HideInInspector] public float Life = 70f;
    [HideInInspector] public float MaxLife = 70f;

    Action<int> ScoreUpdateFunctions;
    public Action<float> GaugeUpdateFunctions;

    //������ �� ���������� üũ�뵵
    int moveDownJellyCount;
    public int MoveDownJellyCount
    {
        get
        {
            return moveDownJellyCount;
        }
        set
        {
            moveDownJellyCount = value;
            if (moveDownJellyCount == 0)
                SetIsWaiting(false);
        }
    }

    public void SetIsWaiting(bool _IsWaiting)
    {
        IsWaiting = _IsWaiting;
    }

    void UpdateCombo(int _ComboCount)
    {
        //5�� ������ �޺� �ʱ�ȭ ��Ŵ
        if (stopWatch.ElapsedMilliseconds > 5000f)
            Combo = 0;

        Combo += _ComboCount;
        if (Combo > MaxCombo)
            MaxCombo = Combo;

        //Challenge�϶���
        if (MaxCombo > DataManager.Instance.MaxCombo && curStage == 0)
            DataManager.Instance.SetComboMax(MaxCombo);
        stopWatch.Restart();

        //�޺� ���ӻ󿡼� ǥ��
        PlayUIManager.Instance.RenderCombo(Combo);
    }

    void UpdateScore(int _Score)
    {
        Score = _Score;
    }

    void UpdateGauge(float _Gauge)
    {
        if(_Gauge == 0)
        {
            Gauge = 0f;
            return;
        }
        Gauge += _Gauge;
    }

    public void UpdateLife(float _Life = 1)
    {
        if (Life <= 0 && !isGameOver)
        {
            GameOverEvent();
            isGameOver = true;
        }
        Life -= _Life;
        PlayUIManager.Instance.UpdateLifeGauge(Life, MaxLife);
    }

    public void UpdateLife()
    {
        if (Life <= 0 && !isGameOver)
        {
            GameOverEvent();
            isGameOver = true;
        }
        Life--;
        PlayUIManager.Instance.UpdateLifeGauge(Life, MaxLife);
    }

    //Stage��
    void UpdateTime()
    {
        if (curTime == 0 && !isGameOver)
        {
            GameOverEvent();
            isGameOver = true;
        }
        --curTime;
        //Stage ���̵� ������� �ϴ� �ִ� �ð� 60���� ����
        PlayUIManager.Instance.UpdateTimer(curTime, 60);
    }


    void GameOverEvent()
    {
        //Challenge�϶���
        if (curStage == 0)
        {
            //�� ������ �� �־���
            if (Score > DataManager.Instance.MaxScore)
                DataManager.Instance.SetScoreMax(Score);

            if (PlayTime > DataManager.Instance.MaxPlayTime)
                DataManager.Instance.SetPlayTimeMax(PlayTime);

            //Challenge���� Ư�� ���� �޼��ϸ� Stage1 ����
            if (Score > 1000 && PlayTime > 50 && MaxCombo > 300)
            {
                if (DataManager.Instance.opendStage == 0)
                {
                    DataManager.Instance.opendStage = 1;
                    DataManager.Instance.jsonManager.SaveStageData();

                    //�������� ���ȴٴ� UI�ڽ� 
                }
            }
        }
        else //Stage�� ���
        {
            //Stage�̸鼭 ���� ������������ ���� ��ũ�� ����� ��ũ���� ���� ��
            if(DataManager.Instance.stageRank[curStage - 1] < Rank)
            {
                DataManager.Instance.stageRank[curStage - 1] = Rank;
                //10�� ������ ������ ���������� �ƴ� ���
                if (curStage != 10)
                    DataManager.Instance.opendStage++;
                DataManager.Instance.jsonManager.SaveStageData();
            }
        }

        //UI�ڽ� �����Ŵ
        PlayUIManager.Instance.GameOverMessageBoxEnableEvent();

        //�� ������ �� ����
        DataManager.Instance.jsonManager.SaveRankData();       
    }



    void StartPlayTimeUpdate()
    {
        if (isGameOver)
            return;
        PlayTime++;
    }


    void ListingDelegate()
    {
        ScoreUpdateFunctions += UpdateScore;
        ScoreUpdateFunctions += PlayUIManager.Instance.RenderScore;

        if (curStage != 0)
            return;
        GaugeUpdateFunctions += UpdateGauge;
        GaugeUpdateFunctions += PlayUIManager.Instance.UpdateSamePangGauge;
    }

    Type GetJellyTypeToCreateJelly()
    {
        int range = 8;
        if (maxBombCount == bombCount)
            range = 6;
        Type type = (Type)Random.Range(0, range);

        if (type == Type.Bomb || type == Type.LineBomb)
            ++bombCount;
        
        return type;
    }

    void SetMap()
    {
        for(int y = 0; y < lengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                Type type = GetJellyTypeToCreateJelly();
                var jelly = Pool.GetJelly(type);
                jelly.gameObject.SetActive(true);
                jelly.SetPos(x, y);
                jelly.transform.SetParent(map);
                jelly.transform.localPosition = new Vector3(x, y, 0);

                GameMap[y, x] = jelly;
            }
        }
    }


    public void DoPang(Jelly _jelly)
    {
        IsPangSuccess = false;
        StartCoroutine(DoPangRoutine(_jelly));
    }

    IEnumerator DoPangRoutine(Jelly _jelly)
    {
        var jellys = GetAllJellySameType(_jelly);
    
        if (jellys.Count >= 3)
        {
            SetIsWaiting(true);
            Dictionary<int, JellyMapData> xData = new Dictionary<int, JellyMapData>();

            //���� ����
            RemoveJellys(jellys, ref xData);

            //���� ���� �� ���� �����鼭 ����
            MoveDownJellyAndCreateJelly(xData);

            IsPangSuccess = true;
        }

        yield return null;
    }

    void RemoveJellys(List<Jelly> _jellys, ref Dictionary<int, JellyMapData> _xData)
    {
        int comboCount = 0;

        foreach (var jelly in _jellys)
        {
            if (_xData.ContainsKey(jelly.CurPos.x))
            {
                var data = _xData[jelly.CurPos.x];
                if (data.yMin > jelly.CurPos.y)
                    data.yMin = jelly.CurPos.y;
                _xData[jelly.CurPos.x] = data;
            }
            else
                _xData.Add(jelly.CurPos.x, new JellyMapData(jelly.CurPos.y));

            //���� ���� null
            Type jellyType = jelly.type;
            GameMap[jelly.CurPos.y, jelly.CurPos.x] = null;
            jelly.transform.SetParent(poolTransform);
            jelly.gameObject.SetActive(false);


            var Spark = Pool.GetSpark();
            Spark.SetActive(true);
            Spark.transform.position = new Vector3(jelly.CurPos.x, jelly.CurPos.y, -0.5f);

            ++comboCount;

            DataManager.Instance.AddPangedJellyCount(jellyType);
        }
        UpdateCombo(comboCount);
        ScoreUpdateFunctions(Score + comboCount * Random.Range(8, 12));

        //Challenge�ƴϸ� �Ѿ
        if (curStage != 0)
            return;
        GaugeUpdateFunctions(comboCount * Random.Range(0.2f, 0.5f));
        DataManager.Instance.jsonManager.SaveRankData();
    }

    void MoveDownJellyAndCreateJelly(Dictionary<int, JellyMapData> _xData, float _speed = 20f)
    {
        StartCoroutine(MoveDownJellyAndCreateJellyRoutine(_xData, _speed));
    }

    IEnumerator MoveDownJellyAndCreateJellyRoutine(Dictionary<int, JellyMapData> _xData, float _speed = 20f)
    {
        foreach (var data in _xData)
        {
            for (int y = data.Value.yMin; y < lengthY; y++)
            {
                if (GameMap[y, data.Key] == null)
                {
                    //���̱��ϱ�
                    int length = 1;
                    while (GameMap[y + length, data.Key] == null)
                    {
                        ++length;
                        if (y + length == lengthY)
                            break;
                    }
                    if (y + length == lengthY)
                        break;

                    int indexAdd = length;


                    while (GameMap[y + indexAdd, data.Key] != null)
                    {
                        GameMap[y + indexAdd - length, data.Key] = GameMap[y + indexAdd, data.Key];
                        GameMap[y + indexAdd, data.Key] = null;
                        GameMap[y + indexAdd - length, data.Key].SetPos(data.Key, y + indexAdd - length);



                        //gameMap[y + indexAdd, data.Key].transform.position = new Vector3(data.Key, y + indexAdd - length, 0);
                        GameMap[y + indexAdd - length, data.Key].MoveDown(length, _speed);
                        //MoveDown �ϴ� �ֵ� ����
                        ++moveDownJellyCount;


                        ++indexAdd;
                        if (y + indexAdd == lengthY)
                            break;
                    }

                    //���� ���� �κ� ����
                    for (int i = 0; i < length; i++)
                    {
                        var jelly = Pool.GetJelly(GetJellyTypeToCreateJelly());
                        jelly.gameObject.SetActive(true);
      
                        jelly.transform.SetParent(map);
                        jelly.transform.localPosition = new Vector3(data.Key, lengthY - i - 1, 0);
                        jelly.SetPos(new Vector2Int(data.Key, lengthY - i - 1));
                        GameMap[lengthY - i - 1, data.Key] = jelly;
                    }

                }
            }
        }

        if (!IsTherePang())
        {
            //0.7�� ��ٷȴٰ� ����
            yield return new WaitForSecondsRealtime(0.7f);
            SetIsWaiting(true);
            PangAllJelly();
        }
        yield return null;
    }

    public void DoHorizontalLinePang(Jelly _jelly)
    {
        StartCoroutine(DoHorizontalLinePangRoutine(_jelly));
    }

    IEnumerator DoHorizontalLinePangRoutine(Jelly _jelly)
    {
        Dictionary<int, JellyMapData> xData = new Dictionary<int, JellyMapData>();
        int yIndex = _jelly.CurPos.y;
        int comboCount = 0;
        for (int x = 0; x < lengthX; x++)
        {
            if (_jelly.CurPos.x != x && (GameMap[yIndex, x].type == Type.Bomb || GameMap[yIndex, x].type == Type.LineBomb))
                continue;
            SetIsWaiting(true);
            Type jellyType = GameMap[yIndex, x].type;
            GameMap[yIndex, x].gameObject.transform.SetParent(poolTransform);
            GameMap[yIndex, x].gameObject.SetActive(false);
            GameMap[yIndex, x] = null;

            var Spark = Pool.GetSpark();
            Spark.SetActive(true);
            Spark.transform.position = new Vector3(x, yIndex, -0.5f);

            xData.Add(x, new JellyMapData(yIndex));

            ++comboCount;
            DataManager.Instance.AddPangedJellyCount(jellyType);
            yield return new WaitForSeconds(0.1f);
        }
        MoveDownJellyAndCreateJelly(xData);
        UpdateCombo(comboCount);
        ScoreUpdateFunctions(Score + Mathf.RoundToInt(comboCount * Random.Range(0.5f, 1f)));
        if (curStage == 0)
        {
            GaugeUpdateFunctions(comboCount * Random.Range(0.5f, 1f));
            DataManager.Instance.jsonManager.SaveRankData();
        }
        --bombCount;
        yield return null;
    }

    public void DoVerticalLinePang(Jelly _jelly)
    {
        StartCoroutine(DoVerticalLinePangRoutine(_jelly));
    }

    IEnumerator DoVerticalLinePangRoutine(Jelly _jelly)
    {
        Dictionary<int, JellyMapData> xData = new Dictionary<int, JellyMapData>();
        int xIndex = _jelly.CurPos.x;
        int comboCount = 0;
        for (int y = 0; y < HalfLengthY; y++)
        {
            if (GameMap[y, xIndex] != _jelly&& (GameMap[y, xIndex].type == Type.Bomb || GameMap[y, xIndex].type == Type.LineBomb))
                continue;

            SetIsWaiting(true);
            Type jellyType = GameMap[y, xIndex].type;
            GameMap[y, xIndex].gameObject.transform.SetParent(poolTransform);
            GameMap[y, xIndex].gameObject.SetActive(false);
            GameMap[y, xIndex] = null;

            var Spark = Pool.GetSpark();
            Spark.SetActive(true);
            Spark.transform.position = new Vector3(xIndex, y, -0.5f);

            ++comboCount;
            if (!xData.ContainsKey(xIndex))
                xData.Add(xIndex, new JellyMapData(y));

            DataManager.Instance.AddPangedJellyCount(jellyType);
            yield return new WaitForSeconds(0.1f);
        }
      
        MoveDownJellyAndCreateJelly(xData);
        UpdateCombo(comboCount);
        ScoreUpdateFunctions(Score + comboCount * Random.Range(8, 12));
        if (curStage == 0)
        {
            GaugeUpdateFunctions(comboCount * Random.Range(0.5f, 1f));
            DataManager.Instance.jsonManager.SaveRankData();
        }
        --bombCount;
        yield return null;
    }


    public void DoPangSameJelly(Type _jelly)
    {
        Dictionary<int, JellyMapData> xData = new Dictionary<int, JellyMapData>();

        int comboCount = 0;
        for(int y = 0; y < HalfLengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                if (GameMap[y, x].type != _jelly)
                    continue;
                SetIsWaiting(true);
                GameMap[y, x].gameObject.SetActive(false);
                GameMap[y, x].transform.SetParent(poolTransform);
                GameMap[y, x] = null;

                var Spark = Pool.GetSpark();
                Spark.SetActive(true);
                Spark.transform.position = new Vector3(x, y, -0.5f);

                //�޺� ���
                ++comboCount;

                if (!xData.ContainsKey(x))
                    xData.Add(x, new JellyMapData(y));
            }
        }
        DataManager.Instance.AddPangedJellyCount(_jelly, comboCount);

        UpdateCombo(comboCount);
        ScoreUpdateFunctions(Score + comboCount * Random.Range(10, 15));
        MoveDownJellyAndCreateJelly(xData);
        DataManager.Instance.jsonManager.SaveRankData();
    }

    

    public void DoBomb(Jelly _jelly)
    {
        var neighbours = GetNeighbours(_jelly, true);
        //���� �ڽŵ� �߰�
        neighbours.Add(_jelly);


        Dictionary<int, JellyMapData> xData = new Dictionary<int, JellyMapData>();

        int comboCount = 0;
        foreach (var jelly in neighbours)
        {
            if ((jelly != _jelly && jelly.type == Type.Bomb) || jelly.type == Type.LineBomb)
                continue;
            IsWaiting = true;
            GameMap[jelly.CurPos.y, jelly.CurPos.x].transform.SetParent(poolTransform);
            GameMap[jelly.CurPos.y, jelly.CurPos.x].gameObject.SetActive(false);
            GameMap[jelly.CurPos.y, jelly.CurPos.x] = null;

            var Spark = Pool.GetSpark();
            Spark.SetActive(true);
            Spark.transform.position = new Vector3(jelly.CurPos.x, jelly.CurPos.y, -0.5f);

            //�޺�
            ++comboCount;

            if (!xData.ContainsKey(jelly.CurPos.x))
                xData.Add(jelly.CurPos.x, new JellyMapData(jelly.CurPos.y));
            else
            {
                JellyMapData jellyMapData = new JellyMapData(xData[jelly.CurPos.x].yMin);
                if (xData[jelly.CurPos.x].yMin > jelly.CurPos.y)
                    jellyMapData.yMin = jelly.CurPos.y;
                xData[jelly.CurPos.x] = jellyMapData;
            }

            DataManager.Instance.AddPangedJellyCount(jelly.type);
        }

        UpdateCombo(comboCount);
        MoveDownJellyAndCreateJelly(xData);
        ScoreUpdateFunctions(Score + comboCount * Random.Range(8, 12));
        if (curStage == 0)
        {
            GaugeUpdateFunctions(comboCount * Random.Range(0.2f, 0.5f));
            DataManager.Instance.jsonManager.SaveRankData();
        }
        --bombCount;
    }

    List<Jelly> GetAllJellySameType(Jelly _Jelly)
    {
        List<Jelly> jellys = new List<Jelly>();
        jellys.Add(_Jelly);

        var neighbours = GetNeighbours(_Jelly, false);
        for(int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].type != _Jelly.type)
                continue;

            if (neighbours[i].CurPos.y >= HalfLengthY)
                continue;

            jellys.Add(neighbours[i]);
        }

        for(int i = 1; i < jellys.Count; i++)
        {
            var neighbour = GetNeighbours(jellys[i], false);
            for(int j = 0; j < neighbour.Count; j++)
            {
                if (jellys.Contains(neighbour[j]))
                    continue;

                if (neighbour[j].type != _Jelly.type)
                    continue;

                if (neighbour[j].CurPos.y >= HalfLengthY)
                    continue;

                jellys.Add(neighbour[j]);
            }
        }

        return jellys;
    }


    List<Jelly> GetNeighbours(Jelly _Jelly, bool _IsDiagonalOkay)
    {
        List<Jelly> neighbours = new List<Jelly>();
        for(int y = -1;y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                if (!_IsDiagonalOkay && Mathf.Abs(x) + Mathf.Abs(y) == 2)
                    continue;

                if (x == 0 && y == 0)
                    continue;

                int posX = _Jelly.CurPos.x + x;
                int posY = _Jelly.CurPos.y + y;

                if (0 <= posX && posX < lengthX && 0 <= posY && posY < lengthY)
                    neighbours.Add(GameMap[posY, posX]);
            }
        }
        return neighbours;
    }

    bool IsTherePang()
    {
        for(int y = 0; y < HalfLengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                var type = GameMap[y, x].type;
                if (type == Type.Bomb || type == Type.LineBomb)
                    return true;
                var sameNeighboursCount = GetAllJellySameType(GameMap[y, x]).Count;
                if (sameNeighboursCount >= 3)
                    return true;
            }
        }

        return false;
    }

    void PangAllJelly()
    {
        List<Jelly> allJelly = new List<Jelly>();
        for(int y = 0; y < HalfLengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                allJelly.Add(GameMap[y, x]);
            }
        }

        Dictionary<int, JellyMapData> xData = new Dictionary<int, JellyMapData>();
        RemoveJellys(allJelly, ref xData);
        MoveDownJellyAndCreateJelly(xData);
    }


    void MoveDownJellyBeforeStart()
    {
        StartCoroutine(MoveDownJellyBeforeStartRoutine());
    }

    IEnumerator MoveDownJellyBeforeStartRoutine()
    {
        float speed = 10f;
        if (curStage == 0)
            speed = 30f;

        map.transform.position += Vector3.up * (HalfLengthY + 1);
        Vector3 target = map.transform.position + Vector3.down * (HalfLengthY + 1);
        float length = 0;
        IsWaiting = true;

        //FadeIn������ �ణ ��ٷȴٰ� ����
        yield return new WaitForSeconds(0.01f);

        while (length < HalfLengthY + 1)
        {
            length += Time.deltaTime * speed;
            map.transform.position += Vector3.down * Time.deltaTime * speed;
            yield return null;
        }
        map.transform.position = target;
        IsWaiting = false;
    }


    //Challenge�ʸ�
    void SetContinueMap(JsonPlayData _JsonData)
    {
        for(int y = 0; y < lengthY; y++)
        {
            for(int x = 0; x < lengthX; x++)
            {
                var jelly = Pool.GetJelly((Type)_JsonData.mapData[lengthX * y + x].JellyType);
                jelly.gameObject.SetActive(true);
                jelly.transform.SetParent(map);
                jelly.transform.localPosition = new Vector3(x, y, 0);
                jelly.SetPos(x, y);
                GameMap[y, x] = jelly;
            }
        }
        //�� �κ� ���� 
        MaxCombo = _JsonData.playData.MaxCombo;
        int combo = _JsonData.playData.CurCombo;
        float gauge = _JsonData.playData.Gauge;
        int score = _JsonData.playData.Score;
        float life = _JsonData.playData.Life;

        UpdateCombo(combo);
        ScoreUpdateFunctions(score);
        Life = life;
        UpdateLife(Life - life);
        //�ٽ� ������ �� Resume��ư�� �ֵ��� ����
        PlayUIManager.Instance.SetContinueGameGauge(gauge);

        //����� JsonData null��
        DataManager.Instance.jsonManager.RemoveSavedData();
    }

    void Awake()
    {
        Pool = GetComponent<PoolManager>();
        GameMap = new Jelly[lengthY, lengthX];
        poolTransform = Pool.gameObject.transform;
        stopWatch = new Stopwatch();

        ListingDelegate();
    }

    void Start()
    {
        HalfLengthY = lengthY / 2;

        if(curStage == 0)
        {
            if (DataManager.Instance.jsonManager.JsonPlaySavedData == null)
            {
                SetMap();
                MoveDownJellyBeforeStart();
            }
            else
                SetContinueMap(DataManager.Instance.jsonManager.JsonPlaySavedData);

            
            updates += UpdateLife;
            updates += StartPlayTimeUpdate;
        }
        else
        {
            SetMap();
            MoveDownJellyBeforeStart();

            targetMiss = new int[10];  //10������������ �����Ƿ� 10
            rankCondition = new List<int[]>();


            for(int i = 0; i < 10; i++)
            {
                targetMiss[i] = 10 - i;
                rankCondition.Add(new int[3]);
            }
            //���� : Score + Combo * 2 - Miss * 15
            rankCondition[0][0] = 1000;
            rankCondition[0][1] = 1200;
            rankCondition[0][2] = 1400;

            rankCondition[1][0] = 1100;
            rankCondition[1][1] = 1250;
            rankCondition[1][2] = 1450;

            rankCondition[2][0] = 1120;
            rankCondition[2][1] = 1290;
            rankCondition[2][2] = 1500;

            rankCondition[3][0] = 1200;
            rankCondition[3][1] = 1350;
            rankCondition[3][2] = 1550;

            rankCondition[4][0] = 1230;
            rankCondition[4][1] = 1390;
            rankCondition[4][2] = 1570;

            rankCondition[5][0] = 1280;
            rankCondition[5][1] = 1420;
            rankCondition[5][2] = 1600;

            rankCondition[6][0] = 1330;
            rankCondition[6][1] = 1440;
            rankCondition[6][2] = 1640;

            rankCondition[7][0] = 1400;
            rankCondition[7][1] = 1500;
            rankCondition[7][2] = 1700;

            rankCondition[8][0] = 1560;
            rankCondition[8][1] = 1620;
            rankCondition[8][2] = 1760;

            rankCondition[9][0] = 1640;
            rankCondition[9][1] = 1890;
            rankCondition[9][2] = 2000;



            updates += UpdateTime;
        }
    }


    float time = 0;
    void Update()
    {
        if (!IsWaiting)
            time += Time.deltaTime;
        if (time >= 1)
        {
            updates.Invoke();
            time = 0f;
        }
    }
}

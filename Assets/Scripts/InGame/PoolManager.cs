using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject missWordPrefab;
    [SerializeField] GameObject[] comboNumbersPrefab;
    [SerializeField] GameObject sparkPrefab;
    //Á©¸®µé
    [SerializeField] GameObject yellowPrefab;
    [SerializeField] GameObject greenPrefab;
    [SerializeField] GameObject bluePrefab;
    [SerializeField] GameObject redPrefab;
    [SerializeField] GameObject purplePrefab;
    [SerializeField] GameObject grayPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject lineBombPrefab;

    List<GameObject> missWordContainer;
    List<List<GameObject>> comboNumbersContainer;
    List<GameObject> sparkContainer;
    List<Jelly> yellowContainer;
    List<Jelly> greenContainer;
    List<Jelly> blueContainer;
    List<Jelly> redContainer;
    List<Jelly> purpleContainer;
    List<Jelly> grayContainer;
    List<Jelly> bombContainer;
    List<Jelly> lineBombContainer;

    void Awake()
    {
        missWordContainer = new List<GameObject>();
        comboNumbersContainer = new List<List<GameObject>>();
        sparkContainer = new List<GameObject>();
        yellowContainer = new List<Jelly>();
        greenContainer = new List<Jelly>();
        blueContainer = new List<Jelly>();
        redContainer = new List<Jelly>();
        purpleContainer = new List<Jelly>();
        grayContainer = new List<Jelly>();
        bombContainer = new List<Jelly>();
        lineBombContainer = new List<Jelly>();
    }

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            //Combo
            List<GameObject> numbers = new List<GameObject>();
            var number = Instantiate(comboNumbersPrefab[i]);
            number.transform.SetParent(transform);
            number.SetActive(false);
            numbers.Add(number);
            comboNumbersContainer.Add(numbers);
        }
    }

    public GameObject GetMissWord()
    {
        foreach (var missWord in missWordContainer)
        {
            if (!missWord.activeSelf)
                return missWord;
        }

        var missWordObj = Instantiate(missWordPrefab);
        missWordObj.SetActive(false);
        missWordContainer.Add(missWordObj);
        missWordObj.transform.SetParent(transform);
        return missWordObj;
    }

    public GameObject GetComboNumber(int _Number)
    {
        if (_Number < 0 || _Number >= 10)
            return null;

        foreach (GameObject number in comboNumbersContainer[_Number])
        {
            if (!number.activeSelf)
                return number;
        }

        var num = Instantiate(comboNumbersPrefab[_Number]);
        num.transform.SetParent(transform);
        comboNumbersContainer[_Number].Add(num);
        num.SetActive(false);
        return num;
    }

    public GameObject GetSpark()
    {
        foreach (var spark in sparkContainer)
        {
            if (!spark.activeSelf)
                return spark;
        }

        var sparkOb = Instantiate(sparkPrefab);
        sparkOb.transform.SetParent(transform);
        sparkContainer.Add(sparkOb);
        sparkOb.SetActive(false);

        return sparkOb;
    }

    public Jelly GetJelly(Type _type)
    {
        switch (_type)
        {
            case Type.Yellow:
                foreach (var yellow in yellowContainer)
                {
                    GameObject yellowObj = yellow.gameObject;
                    if (!yellowObj.activeSelf)
                        return yellow;
                }
                GameObject yellowOb = Instantiate(yellowPrefab);
                yellowOb.SetActive(false);
                var yel = yellowOb.GetComponent<Jelly>();
                yel.SetType(Type.Yellow);
                yellowContainer.Add(yel);
                return yellowContainer[yellowContainer.Count - 1];
            case Type.Green:
                foreach (var green in greenContainer)
                {
                    GameObject greenObj = green.gameObject;
                    if (!greenObj.activeSelf)
                        return green;
                }
                GameObject greenOb = Instantiate(greenPrefab);
                greenOb.SetActive(false);
                var gre = greenOb.GetComponent<Jelly>();
                gre.SetType(Type.Green);
                greenContainer.Add(gre);
                return greenContainer[greenContainer.Count - 1];

            case Type.Blue:
                foreach (var blue in blueContainer)
                {
                    GameObject blueObj = blue.gameObject;
                    if (!blueObj.activeSelf)
                        return blue;
                }
                GameObject blueOb = Instantiate(bluePrefab);
                blueOb.SetActive(false);
                var blu = blueOb.GetComponent<Jelly>();
                blu.SetType(Type.Blue);
                blueContainer.Add(blu);
                return blueContainer[blueContainer.Count - 1];

            case Type.Gray:
                foreach (var gray in grayContainer)
                {
                    GameObject grayObj = gray.gameObject;
                    if (!grayObj.activeSelf)
                        return gray;
                }
                GameObject grayOb = Instantiate(grayPrefab);
                grayOb.SetActive(false);
                var gra = grayOb.GetComponent<Jelly>();
                gra.SetType(Type.Gray);
                grayContainer.Add(gra);
                return grayContainer[grayContainer.Count - 1];

            case Type.Red:
                foreach (var red in redContainer)
                {
                    GameObject redObj = red.gameObject;
                    if (!redObj.activeSelf)
                        return red;
                }
                GameObject redOb = Instantiate(redPrefab);
                redOb.SetActive(false);
                var re = redOb.GetComponent<Jelly>();
                re.SetType(Type.Red);
                redContainer.Add(re);
                return redContainer[redContainer.Count - 1];

            case Type.Purple:
                foreach (var purple in purpleContainer)
                {
                    GameObject purpleObj = purple.gameObject;
                    if (!purpleObj.activeSelf)
                        return purple;
                }
                GameObject purpleOb = Instantiate(purplePrefab);
                purpleOb.SetActive(false);
                var pur = purpleOb.GetComponent<Jelly>();
                pur.SetType(Type.Purple);
                purpleContainer.Add(pur);
                return purpleContainer[purpleContainer.Count - 1];
            case Type.Bomb:
                foreach (var bomb in bombContainer)
                {
                    GameObject bombObj = bomb.gameObject;
                    if (!bombObj.activeSelf)
                        return bomb;
                }
                GameObject bombOb = Instantiate(bombPrefab);
                bombOb.SetActive(false);
                var bom = bombOb.GetComponent<Jelly>();
                bom.SetType(Type.Bomb);
                bombContainer.Add(bom);
                return bombContainer[bombContainer.Count - 1];

            default:
                foreach (var lineBomb in lineBombContainer)
                {
                    GameObject bombObj = lineBomb.gameObject;
                    if (!bombObj.activeSelf)
                        return lineBomb;
                }
                GameObject lineBombOb = Instantiate(lineBombPrefab);
                lineBombOb.SetActive(false);
                var lineBom = lineBombOb.GetComponent<Jelly>();
                lineBom.SetType(Type.LineBomb);
                lineBombContainer.Add(lineBom);
                return lineBombContainer[lineBombContainer.Count - 1];
        }
    }
}

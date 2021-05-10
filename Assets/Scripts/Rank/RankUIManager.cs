using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RankUIManager : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Transform[] maxTransforms;
    [SerializeField] Transform pangedJellyTransform;
    [SerializeField] Image[] scoreNumbers;
    [SerializeField] Image[] comboNumbers;
    [SerializeField] TextMeshProUGUI playTimeText;
    [SerializeField] TextMeshProUGUI yellowText;
    [SerializeField] TextMeshProUGUI redText;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI grayText;
    [SerializeField] TextMeshProUGUI greenText;
    [SerializeField] TextMeshProUGUI purpleText;

    void StartRankUIEvent()
    {
        StartCoroutine(StartRankUIEventRoutine());
    }

    IEnumerator StartRankUIEventRoutine()
    {
        //¸Æ½º ¾Öµé ¿Å±è
        Vector3[] targetPos = new Vector3[maxTransforms.Length];
        for(int i = 0; i < maxTransforms.Length; i++)
        {
            targetPos[i] = maxTransforms[i].position;
            maxTransforms[i].position -= Vector3.right * 800f;
        }

        //ÆÎ ¿Å±è
        Vector3 pangTargetPos = pangedJellyTransform.position;
        pangedJellyTransform.position -= Vector3.right * 800f;

        for (int i = 0; i < maxTransforms.Length; i++)
        {
            while (Vector3.Distance(targetPos[i], maxTransforms[i].position) > 0.2f)
            {
                maxTransforms[i].transform.position += Vector3.right * Time.fixedDeltaTime * 800f;
                yield return new WaitForFixedUpdate();
            }


            if (i == 0)
            {
                //µµÂøÇÏ°í ³­ µÚ
                float elapsedTime = 0;
                while (elapsedTime < 3f)
                {
                    //¼ýÀÚ ¼ÅÇÃ
                    for (int index = 0; index < scoreNumbers.Length; index++)
                    {
                        int randomNumber = Random.Range((int)0, (int)10);
                        scoreNumbers[index].sprite = numberSprites[randomNumber];
                    }
                    elapsedTime += 0.02f;
                    yield return null;
                }

                //³¡³ª°í ³­ µÚ
                var SplittedNumber = SplitNumber(DataManager.Instance.MaxScore);
                int length = SplittedNumber.Count;

                if (length > scoreNumbers.Length)
                {
                    for (int Index = 0; Index < scoreNumbers.Length; Index++)
                    {
                        scoreNumbers[Index].sprite = numberSprites[9];
                    }
                    continue;
                }

                for (int Index = 0; Index < scoreNumbers.Length; Index++)
                {
                    scoreNumbers[Index].sprite = numberSprites[0];
                }


                for (int Index = scoreNumbers.Length - length; Index < scoreNumbers.Length; Index++)
                {
                    int num = SplittedNumber.Pop();
                    scoreNumbers[Index].sprite = numberSprites[num];
                }
            }
            else if (i == 1)
            {
                //µµÂøÇÏ°í ³­ µÚ
                float elapsedTime = 0;
                while (elapsedTime < 3f)
                {
                    //¼ýÀÚ ¼ÅÇÃ
                    for (int index = 0; index < comboNumbers.Length; index++)
                    {
                        int randomNumber = Random.Range((int)0, (int)10);
                        comboNumbers[index].sprite = numberSprites[randomNumber];
                    }
                    elapsedTime += 0.02f;
                    yield return null;
                }

                var SplittedNumber = SplitNumber(DataManager.Instance.MaxCombo);
                int length = SplittedNumber.Count;

                if (length > comboNumbers.Length)
                {
                    for (int Index = 0; Index < comboNumbers.Length; Index++)
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
            else
            {
                //Áö³­ ½Ã°£
                int minute = DataManager.Instance.MaxPlayTime / 60;
                int second = DataManager.Instance.MaxPlayTime % 60;

                string secondString = second.ToString();
                if (second < 10)
                    secondString = '0' + secondString;

                playTimeText.text = minute.ToString() + " : " + secondString;
            }
        }

        //ÆÎ
        while (Vector3.Distance(pangTargetPos, pangedJellyTransform.position) > 0.1f)
        {
            pangedJellyTransform.position += Vector3.right * Time.fixedDeltaTime * 1000f;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        yellowText.text = DataManager.Instance.PangedJellyCount[Type.Yellow].ToString();
        redText.text = DataManager.Instance.PangedJellyCount[Type.Red].ToString();
        blueText.text = DataManager.Instance.PangedJellyCount[Type.Blue].ToString();
        grayText.text = DataManager.Instance.PangedJellyCount[Type.Gray].ToString();
        greenText.text = DataManager.Instance.PangedJellyCount[Type.Green].ToString();
        purpleText.text = DataManager.Instance.PangedJellyCount[Type.Purple].ToString();

        yield return null;
    }

    Stack<int> SplitNumber(int _Number)
    {
        Stack<int> splittedNumber = new Stack<int>();
        int length = _Number.ToString().Length;
        int number = _Number;

        for (int i = 0; i < length - 1; i++)
        {
            int Temp = number % 10;
            splittedNumber.Push(Temp);
            number /= 10;
        }
        splittedNumber.Push(number);
        return splittedNumber;
    }


    public void HomeButtonEvent()
    {
        SceneManager.LoadScene("Title");
    }

    public void Start()
    {
        StartRankUIEvent();
    }
}

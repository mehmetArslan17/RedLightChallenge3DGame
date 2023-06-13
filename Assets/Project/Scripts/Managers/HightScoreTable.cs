using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HightScoreTable : MonoBehaviour
{

    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTamplate;
    [SerializeField] List<GameObject> textObject;
    [SerializeField] DataScripts data;
    private List<Transform> hightScoreEntryTransformList;
    int newScore;
    int index;
    private void Awake()
    {
        entryTamplate.gameObject.SetActive(false);

        //for (int i = 0; i < 999; i++)
        //{
        //    data.hightScoreList.Add(new DataScripts.HightScoreEntry { score = Random.Range(0, 750000), name = "New Player " + i });
        //}
      //  newScore = data.oldhighscore > data.score ? data.score : data.oldhighscore;//
        newScore = data.oldhighscore;
        DataScripts.HightScoreEntry deneme = new DataScripts.HightScoreEntry { score = newScore, name = "New Player " };
        data.hightScoreList.Add(deneme);
        for (int i = 0; i < data.hightScoreList.Count; i++)
        {   
            for (int j = 0; j < data.hightScoreList.Count; j++)
            {
                if (data.hightScoreList[j].score < data.hightScoreList[i].score)
                {
                    DataScripts.HightScoreEntry tmp = data.hightScoreList[i];
                    data.hightScoreList[i] = data.hightScoreList[j];
                    data.hightScoreList[j] = tmp;
                }
            }
        }
          index = data.hightScoreList.IndexOf(deneme);

        hightScoreEntryTransformList = new List<Transform>();
        int maxIndex = data.hightScoreList.Count;
        int minIndex = 0;
        if (data.hightScoreList.Count > index + 3)
        {
            maxIndex = index + 3;
        }
        else
        {
            minIndex = data.hightScoreList.Count - 5;
            Debug.Log(data.hightScoreList.Count);
        }
        if (index > 3 && data.hightScoreList.Count > index + 3)
        {
            minIndex = index - 2;
            Debug.Log("sdda");
        }
        if (index < 3)
        {
            maxIndex = 6;
        }

        for (int i = minIndex; i < maxIndex; i++)
        {
            if (i == index)
                CreatHightScoreEntryTransform(data.hightScoreList[i], entryContainer, hightScoreEntryTransformList, true, i);
            else
                CreatHightScoreEntryTransform(data.hightScoreList[i], entryContainer, hightScoreEntryTransformList, false, i);
        }
      //  StartCoroutine(ScoreUpAnim());

    }


    private void CreatHightScoreEntryTransform(DataScripts.HightScoreEntry hightScoreEntry, Transform container, List<Transform> transformsList, bool isPlayer, int rank)
    {
        float templateHeigt = 100f;
        Transform entryTransform = Instantiate(entryTamplate, container);
        textObject.Add(entryTransform.gameObject);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeigt * transformsList.Count);
        entryTransform.gameObject.SetActive(true);
        string rankString;
        switch (rank)
        {
            default: rankString = (rank + 1).ToString(); break;
            case 0: rankString = "1ST"; break;
            case 1: rankString = "2ND"; break;
            case 2: rankString = "3ND"; break;


        }
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        int score = hightScoreEntry.score;
        string name = hightScoreEntry.name;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
        entryTransform.Find("nameText").GetComponent<Text>().text = name;
        if (isPlayer)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.blue;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.blue;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.blue;
        }


        transformsList.Add(entryTransform);
    }
    IEnumerator ScoreUpAnim()
    {
        yield return new WaitForSeconds(1f);
        DataScripts.HightScoreEntry deneme = new DataScripts.HightScoreEntry { score = data.score, name = "New Player " };
        data.hightScoreList.Add(deneme);
        for (int i = 0; i < data.hightScoreList.Count; i++)
        {
            for (int j = 0; j < data.hightScoreList.Count; j++)
            {
                if (data.hightScoreList[j].score < data.hightScoreList[i].score)
                {
                    DataScripts.HightScoreEntry tmp = data.hightScoreList[i];
                    data.hightScoreList[i] = data.hightScoreList[j];
                    data.hightScoreList[j] = tmp;
                }
            }
        }
        int newIndex = data.hightScoreList.IndexOf(deneme);
        Debug.Log("index" + newIndex + "oldindex" + index);
        if(index > newIndex)
        {
            Debug.Log("4444");
            for (int i = index; i < newIndex; i++)
            {
                RectTransform rect = textObject[0].transform.GetComponent<RectTransform>();
                for (int j = 0; j < 5; j++)
                {
                    if (i != 4)
                        textObject[0].transform.DOMoveY(textObject[1].transform.position.y, 0.1f);
                    GameObject go = textObject[0];
                    textObject.Remove(go);
                    textObject.Add(go);
                }
                yield return new WaitForSeconds(0.05f);
                textObject[4].transform.GetComponent<RectTransform>().position = rect.position;

            }
        }
        else if(newIndex < index)
        {
            Debug.Log("555");
            for (int i = newIndex; i < index; i++)
            {
                RectTransform rect = textObject[4].transform.GetComponent<RectTransform>();
                for (int j = 5; j < 0; j++)
                {
                    if (i != 0)
                        textObject[4].transform.DOMoveY(textObject[3].transform.position.y, 0.1f);
                    GameObject go = textObject[0];
                    textObject.Remove(go);
                    textObject.Add(go);
                }
                yield return new WaitForSeconds(0.5f);
                textObject[0].transform.GetComponent<RectTransform>().position = rect.position;

            }
        }
      
    }

}



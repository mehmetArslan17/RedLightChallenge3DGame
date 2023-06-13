using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewHightScoreScript : MonoBehaviour
{
    [SerializeField] List<GameObject> panelList;
    [SerializeField] DataScripts data;
    [SerializeField] List<Vector3> textTransForm;
    [SerializeField] List<Vector3> textControlTransForm;

    [SerializeField] GameObject panelParent;
    [SerializeField] GameObject rewardedButton;
    //   [SerializeField] DataScripts.HightScoreEntry playerScore;
    [SerializeField] int oldScore;
    [SerializeField] int newScore;
    GameObject playerPanel;
    bool isWaitAnim = false;
    int score;
    int index;
    int newIndex;
    int a = 0;
    bool isPlayerAddList;
    [SerializeField] List<GameObject> controlList;
    
    //[SerializeField] Color playerColor;
    //[SerializeField] Color textColor;
    private void Awake()
    {
        //for (int i = 0; i < 999; i++)
        //{
        //    data.hightScoreList.Add(new DataScripts.HightScoreEntry { score = Random.Range(0, 750000), name = "New Player " + i });
        //}
        if (data.score == 0)
        {
            rewardedButton.GetComponent<Button>().interactable = false;
            rewardedButton.transform.GetChild(1).gameObject.SetActive(false);
        }
      //  SaveManager.LoadData(data);
        oldScore = data.oldhighscore;
        newScore = data.score;
        for (int i = 0; i < 7; i++)
        {
            textControlTransForm.Add(panelList[i].transform.localPosition);

            if (i == 3) continue;
            textTransForm.Add(panelList[i].transform.localPosition);
        }
        data.playerHightScore.name = "User";
        // score = data.oldhighscore;

        // playerScore.score = score;
        ////data.hightScoreList.Add(playerScore);
        data.playerHightScore.score = oldScore;
        //if (!data.hightScoreList.Contains(data.playerHightScore))
        //{
        //    Debug.Log("player Bulunamadi");
        //       data.hightScoreList.Add(data.playerHightScore);
        //}
        for (int i = 0; i < data.hightScoreList.Count; i++)
        {
            if (data.hightScoreList[i].name == "User")
            {
                data.hightScoreList[i] = data.playerHightScore;
                isPlayerAddList = true;
                for (int j = 0; j < data.hightScoreList.Count; j++)
                {
                    if (i == j) continue;
                    data.hightScoreList[j].name = "NewPlayer" + Random.Range(1, 199).ToString();
                }
                break;
            }
            
            
        }
        if (!isPlayerAddList)
        {
            Debug.Log("ScoreEklendi");
            data.hightScoreList.Add(data.playerHightScore);
            for (int i = 0; i < data.hightScoreList.Count; i++)
            {
                if (data.hightScoreList[i].name == "User")
                {
                    data.hightScoreList[i] = data.playerHightScore;
                    isPlayerAddList = true;
                }
            }
        }
        SaveManager.SaveData(data);
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
        SaveManager.SaveData(data);

        index = data.hightScoreList.IndexOf(data.playerHightScore);
        if (index + 2 < data.hightScoreList.Count+20)
        {
            playerPanel = panelList[3];
            for (int i = index - 3; i < index + 4; i++)
            {
                //if (i == index)
                //{
                //    panelList[3].transform.GetComponent<Image>().color = textColor;

                //    for (int j = 0; j < 3; j++)
                //    {
                //        panelList[3].transform.GetChild(j).GetComponent<Text>().color = playerColor;
                //    }
                //}
                if(i> data.hightScoreList.Count-1)
                {
                    panelList[a].SetActive(false);
                    if (a+1 < panelList.Count) panelList[a + 1].SetActive(false);
                    continue;
                }
                panelList[a].transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
                panelList[a].transform.GetChild(1).GetComponent<Text>().text = data.hightScoreList[i].score.ToString();
                panelList[a].transform.GetChild(2).GetComponent<Text>().text = data.hightScoreList[i].name.ToString();
                a++;
            }
            a = 0;
        }
        else
        {
            Debug.Log("11");
            playerPanel = panelList[4];

            for (int i = index - 4; i < index + 1; i++)
            {
                //if (i == index)
                //{
                //    panelList[3].transform.GetComponent<Image>().color = textColor;

                //    for (int j = 0; j < 3; j++)
                //    {
                //        panelList[3].transform.GetChild(j).GetComponent<Text>().color = playerColor;
                //    }
                //}
                panelList[a].transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
                panelList[a].transform.GetChild(1).GetComponent<Text>().text = data.hightScoreList[i].score.ToString();
                panelList[a].transform.GetChild(2).GetComponent<Text>().text = data.hightScoreList[i].name.ToString();
                a++;
            }
            a = 0;

        }
        StartCoroutine(NewScoreAnim());
        data.oldhighscore = data.score;
        SaveManager.SaveData(data);
    }

    IEnumerator NewScoreAnim()
    {
        yield return new WaitForSeconds(1.5f);
        //   index = data.hightScoreList.IndexOf(playerScore);
        data.hightScoreList[index].score = newScore;
        data.playerHightScore = data.hightScoreList[index];
        SaveManager.SaveData(data);

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
        newIndex = data.hightScoreList.IndexOf(data.playerHightScore);
        if (newIndex >= index)
        {
           

            //playerPanel = panelList[4];
            playerPanel.transform.GetChild(1).GetComponent<Text>().text = (newScore).ToString();
            playerPanel.transform.GetChild(0).GetComponent<Text>().text = (newIndex + 1).ToString();
            playerPanel.transform.GetChild(2).GetComponent<Text>().text = data.playerHightScore.name.ToString();
            panelList.Remove(playerPanel);
            for (int i = index; i < newIndex; i++)
            {

                GameObject go = panelList[0];
                if (i + 3 < data.hightScoreList.Count)
                {
                    if (i + 3 == newIndex) continue;

                    panelList[5].transform.GetChild(0).GetComponent<Text>().text = (i + 4).ToString();
                    panelList[5].transform.GetChild(1).GetComponent<Text>().text = data.hightScoreList[i + 3].score.ToString();
                    panelList[5].transform.GetChild(2).GetComponent<Text>().text = data.hightScoreList[i + 3].name.ToString();


                }
                if (i + 1 > data.hightScoreList.Count - 1)
                {
                    GameObject pO = panelList[0];
                    pO.SetActive(false);

                }
                if (i + 2 > data.hightScoreList.Count - 1)
                {
                    GameObject pO = panelList[0];
                    pO.SetActive(false);

                }
                if (i + 3 > data.hightScoreList.Count - 1)
                {
                    GameObject pO = panelList[0];
                    pO.SetActive(false);
                }

                if (i > data.hightScoreList.Count - 2)
                {
                    panelList[3].SetActive(false);
                    continue;
                }

                for (int j = panelList.Count - 1; j >= 0; j--)
                {
                    if (j == 0) continue;

                    panelList[j].transform.DOLocalMoveY(textTransForm[j - 1].y, 0.07f);

                }
                yield return new WaitForSeconds(0.05f);

                panelList[0].transform.localPosition = new Vector3(panelList[0].transform.localPosition.x, textTransForm[5].y);
                panelList.Remove(go); panelList.Add(go);


                a = 0;
            }
        }
        else if (index > newIndex)
        {
            for (int i = 0; i < 7; i++)
            {
                panelList[i].SetActive(true);
            }
            playerPanel.transform.GetChild(1).GetComponent<Text>().text = (newScore).ToString();
            playerPanel.transform.GetChild(0).GetComponent<Text>().text = (newIndex + 1).ToString();
            playerPanel.transform.GetChild(2).GetComponent<Text>().text = data.playerHightScore.name.ToString();
            panelList.Remove(playerPanel);
            for (int i = index; i > newIndex; i--)
            {

                GameObject go = panelList[5];

                if (i <= 3)
                {
                    StartCoroutine(WinControl());
                    continue;
                }
                if (i - 3 < 1)
                {
                    GameObject pO = panelList[5];
                    pO.SetActive(false);

                }
                if (i + 4 < 1)
                {
                    GameObject pO = panelList[5];
                    pO.SetActive(false);


                }
                if (i + 5 < 1)
                {
                    GameObject pO = panelList[0];
                    pO.SetActive(false);
                }
                if (i - 3 < data.hightScoreList.Count)
                {
                    if (i - 3 == newIndex) continue;
                    panelList[0].transform.GetChild(0).GetComponent<Text>().text = (i - 2).ToString();
                    panelList[0].transform.GetChild(1).GetComponent<Text>().text = data.hightScoreList[i - 3].score.ToString();
                    panelList[0].transform.GetChild(2).GetComponent<Text>().text = data.hightScoreList[i - 3].name.ToString();
                }



                for (int j = 0; j < panelList.Count; j++)
                {
                    if (j == 5) continue;

                    panelList[j].transform.DOLocalMoveY(textTransForm[j + 1].y, 0.01f);

                }
                panelList[5].transform.localPosition = new Vector3(panelList[5].transform.localPosition.x, textTransForm[0].y);


                yield return new WaitForSeconds(0.05f);
                controlList.Clear();
                panelList.Remove(go);
                controlList.Add(go);
                for (int k = 0; k < panelList.Count; k++)
                {
                    controlList.Add(panelList[k]);
                }

                panelList.Clear();
                for (int j = 0; j < controlList.Count; j++)
                {
                    panelList.Add(controlList[j]);
                }
                controlList.Clear();
                a = 0;


            }
        }

    }
    IEnumerator WinControl()
    {
        yield return new WaitUntil(() => !isWaitAnim);
        isWaitAnim = true;
        yield return new WaitForSeconds(2f);
        if (!panelList.Contains(playerPanel))
        {

            controlList.Clear();

            for (int k = 0; k < panelList.Count; k++)
            {

                controlList.Add(panelList[k]);
                if (k == 0)
                    controlList.Add(playerPanel);
            }

            panelList.Clear();
            for (int j = 0; j < controlList.Count; j++)
            {
                panelList.Add(controlList[j]);
            }
            controlList.Clear();
            a = 0;


        }
        for (int j = 0; j < panelList.Count; j++)
        {
            if (j == 0)
            {
                panelList[0].transform.localPosition = textControlTransForm[6];
                continue;
            }
            panelList[j].transform.DOLocalMoveY(textControlTransForm[j].y, 0.01f);
        }
        isWaitAnim = false;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ComboScript : MonoBehaviour
{
    [SerializeField] List<GameObject> comboObjects;
    [SerializeField] List<GameObject> comboListControl;
    [SerializeField] RectTransform scoreText;
    Sequence seq;
    private void OnEnable()
    {
        Eventmanager.comboTextControl += ComboControl;
        Eventmanager.revivePanelOpen += RevivePanelOpen;
        Eventmanager.comboFinish += RevivePanelOpen;
    }
    private void OnDisable()
    {
        Eventmanager.comboTextControl -= ComboControl;
        Eventmanager.revivePanelOpen -= RevivePanelOpen;
        Eventmanager.comboFinish -= RevivePanelOpen;

    }

    void ComboControl(int combo)
    {
        seq = DOTween.Sequence();
        if (combo == 0)
        {
            foreach (var item in comboObjects)
            {
                item.transform.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f);
            }

        }

        if (combo == 1)
        {
            comboObjects[0].SetActive(true);
            comboListControl.Add(comboObjects[0]);
            comboObjects[0].transform.GetComponent<RectTransform>().DOAnchorPosY(0, 0.1f);
            //comboObjects[0].transform.GetComponent<RectTransform>().DOPunchRotation(new Vector3(60, 0), 1f).OnComplete(()=>
            // comboObjects[0].transform.GetComponent<RectTransform>().DORotate(new Vector3(0, 0), 0.4f));
        }
        else if (combo == 2)
        {
            comboObjects[0].transform.GetComponent<RectTransform>().DOAnchorPosY( - 150, 0.5f).OnComplete(() =>
             {
                 comboObjects[1].SetActive(true);
                 comboObjects[1].transform.GetComponent<RectTransform>().DOAnchorPosY(0, 0.1f);
             });
        }
        else if (combo == 3)
        {
            seq.Append(comboObjects[0].transform.GetComponent<RectTransform>().DOAnchorPosY(- 300, 0.5f)).
                    Join(comboObjects[1].transform.GetComponent<RectTransform>().DOAnchorPosY( - 150, 0.5f))
                     .AppendCallback(() =>
                {
                    comboObjects[2].SetActive(true);
                    comboObjects[2].transform.GetComponent<RectTransform>().DOAnchorPosY(0, 0.1f);
                });
        }
        else if (combo == 4)
        {
            seq.Append(comboObjects[0].transform.GetComponent<RectTransform>().DOAnchorPosY( - 450, 0.5f)).
                    Join(comboObjects[1].transform.GetComponent<RectTransform>().DOAnchorPosY( - 300, 0.5f)).
                    Join(comboObjects[2].transform.GetComponent<RectTransform>().DOAnchorPosY( - 150, 0.5f))
                     .AppendCallback(() =>
                     {
                         comboObjects[3].SetActive(true);
                         comboObjects[3].transform.GetComponent<RectTransform>().DOAnchorPosY(0, 0.1f);
                         //                  comboObjects[3].transform.GetComponent<RectTransform>().DOPunchRotation(new Vector3(60, 0), 1f).OnComplete(() =>
                         //comboObjects[3].transform.GetComponent<RectTransform>().DORotate(new Vector3(0, 0), 0.4f));
                     });
        }
        else if (combo == 5)
        {
            seq.Append(comboObjects[0].transform.GetComponent<RectTransform>().DOAnchorPosY( - 600, 0.5f)).
                    Join(comboObjects[1].transform.GetComponent<RectTransform>().DOAnchorPosY(- 450, 0.5f)).
                    Join(comboObjects[2].transform.GetComponent<RectTransform>().DOAnchorPosY(- 300, 0.5f)).
                    Join(comboObjects[3].transform.GetComponent<RectTransform>().DOAnchorPosY(- 150, 0.5f))

                     .AppendCallback(() =>
                     {
                         comboObjects[4].SetActive(true);
                         comboObjects[4].transform.GetComponent<RectTransform>().DOAnchorPosY(0, 0.1f);
                         //                  comboObjects[4].transform.GetComponent<RectTransform>().DOPunchRotation(new Vector3(60, 0), 1f).OnComplete(() =>
                         //comboObjects[4].transform.GetComponent<RectTransform>().DORotate(new Vector3(0, 0), 0.4f));
                     });
        }
        foreach (var item in comboObjects)
        {
            item.transform.GetComponent<RectTransform>().DOPunchRotation(new Vector3(60, 0), 1f).OnComplete(() =>
       item.transform.GetComponent<RectTransform>().DORotate(new Vector3(0, 0), 0.4f));
        }

    }
    private void RevivePanelOpen()
    {
        foreach (var item in comboObjects)
        {
            item.SetActive(false);
        }
    }
}

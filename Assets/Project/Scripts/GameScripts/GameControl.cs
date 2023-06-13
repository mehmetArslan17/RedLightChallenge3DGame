using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;  
public class GameControl : MonoBehaviour
{
    [SerializeField] public List<GameObject> allCubes;
    [SerializeField] public List<GameObject> cubes;
    [SerializeField] Material changeMatarial;
    [SerializeField] Material cubeMatarial;
    [SerializeField] Material redMatarial;
    [SerializeField] Material yellowMatarial;
    [SerializeField] DataScripts data;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject tapPanel;
    [SerializeField] AudioSource firstFinish;
    private int comboCount = 0;


    GameObject changeObject;
    GameObject lastObject;
    float timer;
    bool gameActive = false;
    bool isSelect = false;
    bool isRevivePanel = false;
    [SerializeField] List<GameObject> redPixel;
    [SerializeField] List<GameObject> yellowPixel;
    int i = 0;
    int j = 0;
    Sequence seq;

    private void OnEnable()
    {
        Eventmanager.addCube += AddCube;
        Eventmanager.revivePanelButton += RevivePanelButton;
        Eventmanager.tutorialStart += TutorialStart;
    }
    private void OnDisable()
    {
        Eventmanager.addCube -= AddCube;
        Eventmanager.revivePanelButton -= RevivePanelButton;
        Eventmanager.tutorialStart -= TutorialStart;

    }
    #region Tutorial
    private void TutorialStart()
    {
        if(data.tutorialCount == 0)
        {
            allCubes[1].transform.DOLocalMoveX(-1, 0.5f) ;
            allCubes[2].SetActive(false);
            allCubes[3].SetActive(false);
            redPixel.Add(allCubes[1]);
            allCubes[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = redMatarial;
        }
        if (data.tutorialCount == 1)
        {
            allCubes[1].transform.DOLocalMove( new Vector3(-0.8f,0.45f), 0.45f);
            allCubes[2].transform.DOLocalMove( new Vector3(-0.8f,-0.45f),0.45f);
            
            allCubes[3].SetActive(false);
            allCubes[2].transform.GetChild(0).GetComponent<MeshRenderer>().material = redMatarial;
        }

        StartCoroutine(TutorialControl());
    }
    IEnumerator TutorialControl()
    {
        yield return new WaitForSeconds(0.5f);
        if (data.tutorialCount == 0)
        {
            allCubes[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
            yield return new WaitForSeconds(1.2f);
            allCubes[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
            allCubes[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
            yield return new WaitForSeconds(0.5f);
            tapPanel.SetActive(true);
        }
        else if (data.tutorialCount == 1)
        {
            allCubes[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
            yield return new WaitForSeconds(1.2f);
            allCubes[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
            allCubes[0].transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
            yield return new WaitForSeconds(0.5f);
            allCubes[2].transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
            allCubes[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
            yield return new WaitForSeconds(0.5f);
            tapPanel.SetActive(true);
        }
    }
    public void TapControl()
    {

        if(data.tutorialCount == 0)
        {
            allCubes[1].transform.DOLocalMoveX(0, 0.5f).OnComplete(() => {
                Eventmanager.playGame?.Invoke();

            });
            allCubes[2].SetActive(true);
            allCubes[3].SetActive(true);
            redPixel.Clear();
            data.tutorialCount = 1;

        }
        else if (data.tutorialCount == 1)
        {
            allCubes[1].transform.DOLocalMove(Vector3.zero, 0.5f);
            allCubes[2].transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => {
              
                Eventmanager.playGame?.Invoke();

            });
            allCubes[3].SetActive(true);
            redPixel.Clear();
            data.tutorialCount = 2;

        }
        tapPanel.SetActive(false);
        foreach (var item in allCubes)
        {
            item.transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
        }
    }

    #endregion

    private void AddCube(GameObject addObject = default, bool addNewObject = false)
    {

        if (addNewObject && data.level < 60) {
            allCubes.Add(addObject);
            cubes.Add(addObject);
                }

        float unitPain = (float)360 / allCubes.Count;
        allCubes[0].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.7f, RotateMode.FastBeyond360).OnComplete(() => gameActive = true);
        float childPos = (float)0.5f / Mathf.Sin(Mathf.Deg2Rad * unitPain / 2);
        virtualCamera.transform.position = new Vector3(0, virtualCamera.transform.position.y, -(2 * childPos + 1) / 0.24f);

        for (int i = 0; i < allCubes.Count; i++)
        {
            allCubes[i].transform.GetChild(0).localPosition = new Vector3(childPos, 0, 0);
            if (i == 0) continue;
            allCubes[i].transform.DOLocalRotate(new Vector3(0, 0, unitPain * i), 0.7f, RotateMode.FastBeyond360);
        }
        lastObject = allCubes[allCubes.Count-1];
        RedZone();
    }

    void Update()
    {
        if (gameActive)
        {
            ChangePixel();
            if(isSelect)
            SelectPixel();
        }

    }
    private void SelectPixel()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameActive = false;
            isSelect = false;

            foreach (var item in allCubes)
            {
                item.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
                item.transform.DOShakeScale(0.1f, 0.2f, 2, 100).OnComplete(() => item.transform.localScale = Vector3.one);
            }
            comboCount++;
            comboCount = Mathf.Clamp(comboCount, 0, 5);
            Eventmanager.comboTextControl?.Invoke(comboCount);
            i = 0;
            j=cubes.Count;
            Invoke("CreatNextLevel", 1f);
            redPixel.Clear();
            yellowPixel.Clear();
        }


#endif
        if (Input.GetMouseButtonDown(0))
        {
            if(data.vibrationOn)  Handheld.Vibrate();
            gameActive = false;
            isSelect = false;
            if (redPixel.Contains(changeObject))
            {
                foreach (var item in allCubes)
                {
                    item.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
                    item.transform.DOShakeScale(0.1f, 0.2f, 2, 100).OnComplete(() => item.transform.localScale = Vector3.one);
                }
                comboCount++;
              comboCount = Mathf.Clamp(comboCount, 0, 5);
                Eventmanager.comboTextControl?.Invoke(comboCount);
                i = 0;
                j = cubes.Count-1;

                Invoke("CreatNextLevel", 1f);

            }
            else if (yellowPixel.Contains(changeObject))
            {
              
                foreach (var item in allCubes)
                {
                    item.transform.GetChild(0).GetComponent<MeshRenderer>().material = yellowMatarial;
                    item.transform.DOShakeScale(0.1f, 0.2f, 2, 100).OnComplete(() => item.transform.localScale = Vector3.one);
                }
                comboCount = 0;
                Eventmanager.comboTextControl?.Invoke(comboCount);
                i = 0;
                j = cubes.Count - 1;

                Invoke("CreatNextLevel", 1f);
            }
            else
            {
                foreach (var item in allCubes)
                {
                    item.transform.GetChild(0).GetComponent<MeshRenderer>().material = redMatarial;
                   
                }
                Eventmanager.cameraShake?.Invoke(1f, 0.4f);
                i = 0;
                comboCount = 0;
                Invoke("FailLevel",1f);
               // Invoke("RestartLevel", 1f);
            }
            redPixel.Clear();
            yellowPixel.Clear();
        }
    }
    private void ChangePixel()
    {
        timer += Time.deltaTime;
        if (timer >= data.speed)
        {
            if (changeObject != null && !redPixel.Contains(changeObject))
            {
                changeObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
            }
            if (redPixel.Contains(changeObject))
            {
                changeObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = redMatarial;
            }
            if (yellowPixel.Contains(changeObject))
            {
                changeObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = yellowMatarial;
            }
           

            changeObject = allCubes[0];
            allCubes.Remove(changeObject);
            allCubes.Add(changeObject);
            changeObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMatarial;
            changeObject.transform.DOShakeScale(0.02f, 0.05f, 1, 100);
            timer = 0;
            if (lastObject == changeObject)
            {
                comboCount = 0;
                Eventmanager.comboFinishControl?.Invoke();
                Eventmanager.comboFinish?.Invoke();
            }
            isSelect = true;
        }
    }
    private void RedZone()
    {
        int redCount = (int)data.level / 10 + 1;
        redCount = Mathf.Clamp(redCount, 1, 5);
        for (int i = 0; i < redCount+1; i++)
        {
            if (data.redChance < Random.Range(1, 100)) redCount -= 1;
            redCount = Mathf.Clamp(redCount, 1, 5);
        }
       // if (data.redChance < Random.Range(0, 100)) redCount = 1;
        int count = 0;
        Debug.LogWarning("RedCount : " + redCount);
        while (count <redCount)
        {
            count++;
            SelecRedZone();
        }
        foreach (var item in redPixel)
        {
            item.transform.GetChild(0).GetComponent<MeshRenderer>().material = redMatarial;
        }
        foreach (var item in yellowPixel)
        {
            item.transform.GetChild(0).GetComponent<MeshRenderer>().material = yellowMatarial;
        }
    }
    private void SelecRedZone()
    {
        int redCount = Random.Range(2, allCubes.Count - 2);
        GameObject go = allCubes[redCount];
        if (redPixel.Contains(go) || yellowPixel.Contains(go))
        {
            SelecRedZone();
            return;
        }
        redPixel.Add(go);
        if (data.yellowChance > Random.Range(0, 100))
        {
            GameObject yellowRightObject = allCubes[redCount + 1];
            if (!redPixel.Contains(yellowRightObject) || !yellowPixel.Contains(yellowRightObject))
                yellowPixel.Add(yellowRightObject);
            GameObject yellowLeftObject = allCubes[redCount - 1];
            if (!redPixel.Contains(yellowLeftObject) || !yellowPixel.Contains(yellowLeftObject))
                yellowPixel.Add(yellowLeftObject);
        }
       
    }
    private void CreatNextLevel()
    {
        seq = DOTween.Sequence();
        gameActive = false;
     StartCoroutine(SoundOn());
        if (j > 0)
        {
            seq.Append(cubes[j].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.7f, RotateMode.FastBeyond360));

            j--;
            CreatNextLevel();
        }
        else
        {
            seq.Append(cubes[j].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.7f, RotateMode.FastBeyond360))
                .AppendCallback(() =>
                {
                    foreach (var item in allCubes)
                    {
                        item.transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;

                    }
                    transform.position += new Vector3(0, 0, 0.1f);
                    data.level++;
                    Eventmanager.levelTextControl?.Invoke();
                    Eventmanager.creatCube?.Invoke();
                });
            i = 0;
        }

    }
    IEnumerator SoundOn()
    {
        float time = 0.7f / cubes.Count;
        for (int i = 0; i < cubes.Count-1; i++)
        {
            yield return new WaitForSeconds(time);
            allCubes[i].GetComponent<AudioSource>().Play();
        }
    }
    private void RestartLevel()
    {
        seq = DOTween.Sequence();
        gameActive = false;
        if (i != allCubes.Count - 1)
        {

            seq.Append(allCubes[i].transform.DORotateQuaternion(changeObject.transform.rotation, 0.5f));
        

            i++;
            RestartLevel();
        }
        else
        {
            seq.Append(allCubes[i].transform.DORotateQuaternion(changeObject.transform.rotation, 0.5f))
                .AppendCallback(() =>
                {
                    foreach (var item in allCubes)
                    {
                        item.transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
                    }

                    AddCube(this.gameObject, false);
                });
            i = 0;
        }

    }
    private void FailLevel()
    {
        seq = DOTween.Sequence();
        gameActive = false;
        if (i != allCubes.Count - 1)
        {

            seq.Append(allCubes[i].transform.DORotate(new Vector3(0,0,90), 0.5f));
            i++;
            FailLevel();
        }
        else
        {
            seq.Append(allCubes[i].transform.DORotate(new Vector3(0, 0, 90), 0.5f))
                .AppendCallback(() =>
                {
                    foreach (var item in allCubes)
                    {
                        item.transform.GetChild(0).GetComponent<MeshRenderer>().material = cubeMatarial;
                    }
                    if (!isRevivePanel)
                        Eventmanager.revivePanelOpen?.Invoke();
                    else if(isRevivePanel)
                    Eventmanager.finishControl?.Invoke();
                    

                });
            i = 0;
        }
    }
    private void RevivePanelButton()
    {
        isRevivePanel = true;
    }
}

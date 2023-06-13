using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] DataScripts data;
    GameObject allCube;
    [SerializeField] GameObject cubeParent;

    private void Awake()
    {
        
        SaveManager.LoadData(data);
        data.level = 1;
    }
    
    private void OnEnable()
    {
        Eventmanager.creatCube += CreatCube;
        Eventmanager.startGame += StartGame;
    }
    private void OnDisable()
    {
        Eventmanager.creatCube -= CreatCube;
        Eventmanager.startGame -= StartGame;

    }
    public void StartGame()
    {
        CreatCube();
        data.speed = data.baseSpeed;
    }
    private void CreatCube()
    {
        GameObject go = cubeParent.transform.GetChild(0).gameObject;
        if (data.level %2 ==0 && data.level < 60)
        {
            GameObject creatObject = Instantiate(go, cubeParent.transform);

            Eventmanager.addCube?.Invoke(creatObject, true);
        }
        else
        {
            data.speed -= data.speedRise;
            Eventmanager.addCube?.Invoke(go, false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelScripts : MonoBehaviour
{

    public void EggAnimControl()
    {
        Debug.Log("Deneme");

        Eventmanager.eggAnimFinish?.Invoke();
    }
}

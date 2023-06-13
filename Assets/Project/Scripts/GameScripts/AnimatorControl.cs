using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControl : MonoBehaviour
{
  [SerializeField]  GameObject revivePanel;
    public void RevivePanelClose()
    {

        revivePanel.SetActive(false);
        Eventmanager.finishControl?.Invoke();
    }
}

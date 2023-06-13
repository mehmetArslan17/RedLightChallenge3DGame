using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainScript : MonoBehaviour
{

  [SerializeField] CinemachineVirtualCamera currentCam;
    float shakeTimer;

    #region ShakeCamera
  
    private void OnEnable()
    {
        Eventmanager.cameraShake += ShakeCamera;
    }
    private void OnDisable()
    {
        Eventmanager.cameraShake -= ShakeCamera;

    }
    void ShakeCamera(float intensity, float time)
    {
         shakeTimer = time;
        CinemachineBasicMultiChannelPerlin cinemachineBasic = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasic.m_AmplitudeGain = intensity;

    }
    private void Update()
    {

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasic = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasic.m_AmplitudeGain = 0;
            }
        }
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer.loopPointReached += VideoFinish;
    }

    private void VideoFinish(VideoPlayer source)
    {
        SceneManager.LoadScene(1);
    }

}

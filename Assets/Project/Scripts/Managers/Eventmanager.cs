using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Eventmanager 
{
    public static Action tutorialStart;
    public static Action levelTextControl;
    public static Action<int> comboTextControl ;
    public static Action comboFinishControl;
    public static Action<float,float> cameraShake;
    public static Action startGame;
    public static Action playGame;
    public static Action finishControl;
    public static Action creatCube;
    public static Action <GameObject,bool> addCube;
    public static Action revivePanelOpen;
    public static Action comboFinish;
    public static Action revivePanelButton;
    public static Action showInterstialAd;
    public static Action<int> showCoinRewarded;
    public static Action showAgainRewarded;
    public static Action<int> showButtonRewarded;
    public static Action updateRedButton;
    public static Action updateYellowButton;
    public static Action updateNoahButton;
    public static Action closeBanner;
    public static Action bannerAddOn;
    public static Action eggAnimFinish;
}

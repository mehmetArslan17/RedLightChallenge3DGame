using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ADManager : MonoBehaviour
{
    [SerializeField] DataScripts data;
    [SerializeField] GameObject threeCoinButton;
    [SerializeField] Text adsFailedText;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd coinRewardedAd;
    private RewardedAd againRewardedAd;
    private RewardedAd buttonRewardedAd;
    int requestButtonNummer;
    int addCoin;

    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });

    }
    void Start()
    {
        if (!data.adsOn)
        {
          //  this.RequestBanner();
            this.RequestInterstitial();

        }
        else if (data.adsOn)
        {

        }
       this.coinRewardedAd = RequestCoinRewardedAd();
       this.againRewardedAd = RequestAgainRewardedAd();
       this.buttonRewardedAd =RequestButtonRewardedAd();
    }
    private void OnEnable()
    {
        Eventmanager.showInterstialAd += ShowInterstialAdd;
        Eventmanager.showCoinRewarded += ShowCoinRewardedAd;
        Eventmanager.showAgainRewarded += ShowAgainRewardedAd;
        Eventmanager.showButtonRewarded += ShowButtonRewardedAd;
        Eventmanager.closeBanner += CloseBanner;
        Eventmanager.bannerAddOn += RequestBanner;
    }
    private void OnDisable()
    {
        Eventmanager.showInterstialAd -= ShowInterstialAdd;
        Eventmanager.showCoinRewarded -= ShowCoinRewardedAd;
        Eventmanager.showAgainRewarded -= ShowAgainRewardedAd;
        Eventmanager.showButtonRewarded -= ShowButtonRewardedAd;
        Eventmanager.closeBanner -= CloseBanner;
        Eventmanager.bannerAddOn -= RequestBanner;


    }

    public RewardedAd RequestCoinRewardedAd()
    {
#if UNITY_ANDROID
        string reklamID = "	ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IPHONE
        string reklamID = "1";
#else
        string reklamID = "unexpected_platform";
#endif

        this.coinRewardedAd = new RewardedAd(reklamID);

        this.coinRewardedAd.OnUserEarnedReward += CoinRewardedSuccess;
      //  this.coinRewardedAd.OnAdFailedToShow += FailedText;
        AdRequest request = new AdRequest.Builder().Build();

        this.coinRewardedAd.LoadAd(request);
        return coinRewardedAd;
    }

    private void ShowCoinRewardedAd(int newAddCoin)
    {
        addCoin = newAddCoin;
      
        if (this.coinRewardedAd.IsLoaded())
        {
            this.coinRewardedAd.Show();
        }
        else
        {
            threeCoinButton.GetComponent<Button>().interactable = false;
        }
    }

    private void CoinRewardedSuccess(object sender, Reward e)
    {
       
        data.money += addCoin;
        SaveManager.SaveData(data);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    RewardedAd RequestButtonRewardedAd()
    {

#if UNITY_ANDROID
        string reklamID = "	ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IPHONE
        string reklamID = "1";
#else
        string reklamID = "unexpected_platform";
#endif

        this.buttonRewardedAd = new RewardedAd(reklamID);

        this.buttonRewardedAd.OnUserEarnedReward += ButtonRewardedSuccess;
      //  this.buttonRewardedAd.OnAdFailedToShow += FailedText;
        AdRequest request = new AdRequest.Builder().Build();

        this.buttonRewardedAd.LoadAd(request);
        return buttonRewardedAd;
    }
    private void ShowButtonRewardedAd(int buttonNummer)
    {

        requestButtonNummer = buttonNummer;
    
        if (this.buttonRewardedAd.IsLoaded())
        {
            this.buttonRewardedAd.Show();
        }
        else
        {
          //  RequestButtonRewardedAd();
        }
    }

    private void ButtonRewardedSuccess(object sender, Reward e)
    {
        switch (requestButtonNummer)
        {
            case 1:
                Eventmanager.updateRedButton?.Invoke();
                break;
            case 2:
                Eventmanager.updateYellowButton?.Invoke();
                break;
            case 3:
                Eventmanager.updateNoahButton?.Invoke();
                break;
        }

    }
    RewardedAd RequestAgainRewardedAd()
    {

#if UNITY_ANDROID
        string reklamID = "	ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IPHONE
        string reklamID = "1";
#else
        string reklamID = "unexpected_platform";
#endif

        this.againRewardedAd = new RewardedAd(reklamID);

        this.againRewardedAd.OnUserEarnedReward += AgainRewardedSuccess;
        this.againRewardedAd.OnAdFailedToShow += AgainRewardedFailed;
      //  this.againRewardedAd.OnAdFailedToShow += FailedText;
        AdRequest request = new AdRequest.Builder().Build();

        this.againRewardedAd.LoadAd(request);
        return againRewardedAd;
    }

    private void AgainRewardedFailed(object sender, AdErrorEventArgs e)
    {
        Eventmanager.revivePanelOpen?.Invoke();
    }

    private void ShowAgainRewardedAd()
    {
      
        if (this.againRewardedAd.IsLoaded())
        {
            this.againRewardedAd.Show();
        }
        else
        {
            Eventmanager.revivePanelOpen?.Invoke();
           // RequestAgainRewardedAd();
        }
    }

    private void AgainRewardedSuccess(object sender, Reward e)
    {

        Eventmanager.addCube?.Invoke(null, false); 
        Eventmanager.revivePanelButton?.Invoke();
    }

    void ShowInterstialAdd()
    {
        if (!data.adsOn)
        {
            if (this.interstitial.IsLoaded())
                this.interstitial.Show();
            else
            {
                SaveManager.SaveData(data);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            SaveManager.SaveData(data);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
  
    }
    void RequestInterstitial()
    {
#if UNITY_ANDROID
        string reklamID = "	ca-app-pub-3940256099942544/1033173712";

#elif UNITY_IPHONE
        string reklamID = "ca-app-pub-7956270127158724/6892115980";
#else
        string reklamID = "unexpected_platform";
#endif

        this.interstitial = new InterstitialAd(reklamID);
        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);
        this.interstitial.OnAdClosed += RequestSuccess;
    }

    private void RequestSuccess(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "	ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);

    }
    void CloseBanner()
    {
        bannerView.Destroy();
    }
}

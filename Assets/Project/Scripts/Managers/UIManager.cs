using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text levelText;
    [SerializeField] DataScripts data;
    [SerializeField] Text moneyText;
    [SerializeField] Text comboText;
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreUpText;
    [SerializeField] Text coinText;
    [SerializeField] Text threeXText;
    [SerializeField] Text hightLevelText;
    [SerializeField] Text hightLevelEggText;
    [SerializeField] Text hightMaxEggText;
    [SerializeField] Image hightlevelImage;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject finishPanel;
    [SerializeField] GameObject incomeButton;
    [SerializeField] GameObject adsIncomeButton;
    [SerializeField] GameObject redChanceButton;
    [SerializeField] GameObject adsRedChanceButton;
    [SerializeField] GameObject yellowChanceButton;
    [SerializeField] GameObject adsYellowChanceButton;
    [SerializeField] GameObject revivePanel;
    [SerializeField] GameObject vibrationButton;
    [SerializeField] GameObject EggButton;
    [SerializeField] List<GameObject> moneyEffect;
    [SerializeField] GameObject coinImage;
    Sequence seq;
    Animator eggButtonAnim;
    int gameFinishMoney;

    private void OnEnable()
    {
        Eventmanager.levelTextControl += LevelTextControl;
        Eventmanager.comboTextControl += ComboTextControl;
        Eventmanager.finishControl += FinishControl;
        Eventmanager.comboFinishControl += ComboFinish;
        Eventmanager.revivePanelOpen += RevivePanelOpen;
        Eventmanager.playGame += PlayButton;
        Eventmanager.updateRedButton += UpdateRedButtonAd;
        Eventmanager.updateYellowButton += UpdateYellowButtonAd;
        Eventmanager.updateNoahButton += UpdateIncomeButtonAd;
        Eventmanager.eggAnimFinish += EggAnimFinish;
    }
    private void OnDisable()
    {
        Eventmanager.levelTextControl -= LevelTextControl;
        Eventmanager.comboTextControl -= ComboTextControl;
        Eventmanager.finishControl -= FinishControl;
        Eventmanager.comboFinishControl -= ComboFinish;
        Eventmanager.revivePanelOpen -= RevivePanelOpen;
        Eventmanager.playGame -= PlayButton;
        Eventmanager.updateRedButton -= UpdateRedButtonAd;
        Eventmanager.updateYellowButton -= UpdateYellowButtonAd;
        Eventmanager.updateNoahButton -= UpdateIncomeButtonAd;
        Eventmanager.eggAnimFinish -= EggAnimFinish;

    }

    private void Awake()
    {
        SaveManager.LoadData(data);
        data.level = 1;

    }
    private void OnApplicationQuit()
    {
        data.oldhighscore = data.score;
        SaveManager.SaveData(data);
    }
    void Start()
    {
        eggButtonAnim = menuPanel.GetComponent<Animator>();
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        ButtonControl();
        LevelTextControl();
        //  data.oldhighscore = data.score;
        data.score = 0;
        scoreText.text = data.score.ToString();
        if (data.vibrationOn)
        {
            vibrationButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            vibrationButton.transform.GetChild(0).gameObject.SetActive(true);

        }
        HightLevelControl();
    }

    private void ComboTextControl(int combo)
    {
        seq = DOTween.Sequence();
        float scoreUp = 1 + (float)data.level / 10;
        if (combo > 1)
        {
            comboText.gameObject.SetActive(false);
            //   seq.Append(comboText.rectTransform.DOLocalMoveY(1201f, 1f))
            float scoreUpp = (float)combo * data.baseScore * scoreUp;
            data.score += (int)scoreUpp;
            comboText.text = "x" + combo.ToString();

            seq.Append(comboText.rectTransform.DOPunchScale(Vector3.one, 1f, 2, 100)).
                   Join(comboText.rectTransform.DOPunchRotation(Vector3.forward * 10, 0.5f, 5, 100)).
                   OnPlay(() =>
                   {
                       scoreUpText.text = "+" + ((int)scoreUpp).ToString();
                       scoreUpText.gameObject.SetActive(false);

                       scoreUpText.transform.DOShakeScale(1.2f, 0.1f, 5, 100).OnComplete(() =>
                       {
                           scoreUpText.rectTransform.localScale = Vector3.one;
                           //    scoreText.text = data.score.ToString();
                           scoreUpText.gameObject.SetActive(false);
                       });
                   })
                .AppendCallback(() =>
                {
                    comboText.rectTransform.localScale = Vector3.one;
                });
        }
        else
        {
            comboText.text = "Combo=0";
            scoreUpText.gameObject.SetActive(false);
            float scoreUpp = (float)data.baseScore * scoreUp;

            data.score += (int)scoreUpp;
            scoreUpText.text = "+" + ((int)scoreUpp).ToString();
            scoreUpText.rectTransform.DOShakeScale(1.2f, 0.1f, 5, 100).OnComplete(() =>
            {
                scoreUpText.rectTransform.localScale = Vector3.one;
                //  scoreText.text = data.score.ToString();
                scoreUpText.gameObject.SetActive(false);
            });
        }

        int gameMoney = (int)data.score;
        int angle = System.Convert.ToInt32(scoreText.text);
        DOTween.To(() => angle, x => angle = x, gameMoney, 1f)
            .OnUpdate(() =>
            {
                scoreText.text = angle.ToString();
            });
        SaveManager.SaveData(data);


    }
    private void RevivePanelOpen()
    {
        revivePanel.SetActive(true);
    }
    public void RevivePanelClose()
    {
        revivePanel.SetActive(false);
        Eventmanager.finishControl?.Invoke();
    }
    public void RevivePanelButton()
    {
        revivePanel.SetActive(false);
        Eventmanager.showAgainRewarded?.Invoke();
    }
    private void ComboFinish()
    {
        comboText.text = "Combo=0";
    }
    private void LevelTextControl()
    {
        levelText.text = "Level " + data.level.ToString();
        levelText.transform.DOShakeScale(1f, 0.1f, 5, 100);
    }
    private void HightLevelControl()
    {
        string fmt = "00";
        hightLevelText.text = data.hightLevel.ToString(fmt);
        hightLevelEggText.text = data.hightLevel.ToString();
        hightlevelImage.fillAmount = (float)data.hightLevel / 100;

        if (!data.isCheckPoint[0])
        {
            hightMaxEggText.text = "20";
            if (data.hightLevel >= 20)
            {
                EggButton.GetComponent<Button>().interactable = true;
                eggButtonAnim.SetBool("eggReadyAnim", true);
            }
            else
            {
                EggButton.GetComponent<Button>().interactable = false;
                eggButtonAnim.SetBool("eggReadyAnim", false);

            }
        }
        else if (!data.isCheckPoint[1])
        {
            hightMaxEggText.text = "40";
            if (data.hightLevel >= 40)
            {
                eggButtonAnim.SetBool("eggReadyAnim", true);
                EggButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                EggButton.GetComponent<Button>().interactable = false;
                eggButtonAnim.SetBool("eggReadyAnim", false);
            }

        }
        else if (!data.isCheckPoint[2])
        {
            hightMaxEggText.text = "60";
            if (data.hightLevel >= 60)
            {
                eggButtonAnim.SetBool("eggReadyAnim", true);
                EggButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                eggButtonAnim.SetBool("eggReadyAnim", false);
                EggButton.GetComponent<Button>().interactable = false;

            }
        }
        else if (!data.isCheckPoint[3])
        {
            hightMaxEggText.text = "80";
            if (data.hightLevel >= 80)
            {
                eggButtonAnim.SetBool("eggReadyAnim", true);
                EggButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                EggButton.GetComponent<Button>().interactable = false;
                eggButtonAnim.SetBool("eggReadyAnim", false);
            }
        }
        else if (!data.isCheckPoint[4])
        {
            hightMaxEggText.text = "100";
            if (data.hightLevel >= 100)
            {
                eggButtonAnim.SetBool("eggReadyAnim", true);
                EggButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                EggButton.GetComponent<Button>().interactable = false;
                eggButtonAnim.SetBool("eggReadyAnim", false);
            }
        }
        else if (data.isCheckPoint[4])
        {
            hightMaxEggText.text = "MAX";

        }
       
    }
    public void EggGiftButton()
    {
        eggButtonAnim.SetBool("eggBrokenAnim", true);
        if (!data.isCheckPoint[0])
        {
            data.isCheckPoint[0] = true;
            EggButton.GetComponent<Button>().interactable = false;
            data.money += 100;
        }
        else if (!data.isCheckPoint[1])
        {
            data.isCheckPoint[1] = true;
            EggButton.GetComponent<Button>().interactable = false;
            data.money += 200;

        }
        else if (!data.isCheckPoint[2])
        {
            data.isCheckPoint[2] = true;
            EggButton.GetComponent<Button>().interactable = false;
            data.money += 300;

        }
        else if (!data.isCheckPoint[3])
        {
            data.isCheckPoint[3] = true;
            EggButton.GetComponent<Button>().interactable = false;
            data.money += 400;

        }
        else if (!data.isCheckPoint[4])
        {
            data.isCheckPoint[4] = true;
            EggButton.GetComponent<Button>().interactable = false;
            data.money += 400;

        }
       
    }
    private void EggAnimFinish()
    {
        eggButtonAnim.SetBool("eggBrokenAnim", false);
        StartCoroutine(CoinEffect());
       

    }
    IEnumerator CoinEffect()
    {
        for (int i = 0; i < moneyEffect.Count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            moneyEffect[i].transform.position = EggButton.transform.position;
            moneyEffect[i].SetActive(true);
            moneyEffect[i].transform.DOMove(coinImage.transform.position, 0.5f).OnComplete(() =>
            {
                moneyEffect[i].SetActive(false);
           
                HightLevelControl();
                ButtonControl();
            });
        }
    }
    private void FinishControl()
    {
        //   if (data.score > data.highscore) data.highscore = data.score;

        int gameMoney = (int)data.score * data.incomeLevel / 100;
        if (data.level > data.hightLevel) data.hightLevel = data.level;
        data.money += gameMoney;
        gameFinishMoney = gameMoney;
        finishPanel.SetActive(true);
        int angle = 0;
        Eventmanager.closeBanner?.Invoke();
        DOTween.To(() => angle, x => angle = x, gameMoney, 1f)
            .OnUpdate(() =>
            {
                coinText.text = angle.ToString();
                threeXText.text = (3 * angle).ToString();
            });
        SaveManager.SaveData(data);

    }
    private void ButtonControl()
    {
        redChanceButton.transform.GetChild(0).GetComponent<Text>().text = data.redChanceMoney.ToString();
        yellowChanceButton.transform.GetChild(0).GetComponent<Text>().text = data.yellowChanceMoney.ToString();
        incomeButton.transform.GetChild(0).GetComponent<Text>().text = data.incomeMoney.ToString();
        redChanceButton.transform.GetChild(1).GetComponent<Text>().text = "%" + data.redChance.ToString();
        yellowChanceButton.transform.GetChild(1).GetComponent<Text>().text = "%" + data.yellowChance.ToString();
        incomeButton.transform.GetChild(1).GetComponent<Text>().text = data.incomeLevel.ToString() + "x";
        adsIncomeButton.transform.GetChild(0).GetComponent<Text>().text = data.incomeLevel.ToString() + "x";
        adsRedChanceButton.transform.GetChild(0).GetComponent<Text>().text = "%" + data.redChance.ToString();
        adsYellowChanceButton.transform.GetChild(0).GetComponent<Text>().text = "%" + data.yellowChance.ToString();
        string fmt = "000000";
        moneyText.text = data.money.ToString(fmt);
        if (data.money < data.incomeMoney)
        {
            incomeButton.SetActive(false);
            adsIncomeButton.SetActive(true);
        }
        else
        {
            incomeButton.SetActive(true);
            adsIncomeButton.SetActive(false);
        }
        if (data.money < data.redChanceMoney)
        {
            redChanceButton.SetActive(false);
            adsRedChanceButton.SetActive(true);
        }
        else
        {
            redChanceButton.SetActive(true);
            adsRedChanceButton.SetActive(false);
        }
        if (data.money < data.yellowChanceMoney)
        {
            yellowChanceButton.SetActive(false);
            adsYellowChanceButton.SetActive(true);
        }
        else
        {
            yellowChanceButton.SetActive(true);
            adsYellowChanceButton.SetActive(false);
        }
        SaveManager.SaveData(data);

    }

    public void PlayButton()
    {

        if (data.tutorialCount < 2)
        {
            menuPanel.SetActive(false);
            Eventmanager.tutorialStart?.Invoke();
        }
        else
        {
            if(!data.adsOn)
            Eventmanager.bannerAddOn?.Invoke();
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
            Eventmanager.startGame?.Invoke();
        }

    }
    public void UpdateRedButtonAd()
    {
        data.redChance += data.redChanceRise;
        redChanceButton.transform.DOShakeRotation(0.3f, 20, 5, 100, true, ShakeRandomnessMode.Harmonic).OnComplete(() => redChanceButton.transform.rotation = Quaternion.Euler(Vector3.zero));
        data.redChanceMoney = (int)(data.redChanceMoney * data.redChanceMoneyMultiple);
        ButtonControl();
    }
    public void UpdateYellowButtonAd()
    {
        data.yellowChance += data.yellowChanceRise;
        yellowChanceButton.transform.DOShakeRotation(0.3f, 20, 5, 100, true, ShakeRandomnessMode.Harmonic).OnComplete(() => yellowChanceButton.transform.rotation = Quaternion.Euler(Vector3.zero));
        data.yellowChanceMoney = (int)(data.yellowChanceMoney * data.yellowChanceMoneyMultiple);
        ButtonControl();
    }
    public void UpdateIncomeButtonAd()
    {
        data.incomeLevel += data.incomeRise;
        incomeButton.transform.DOShakeRotation(0.3f, 20, 5, 100, true, ShakeRandomnessMode.Harmonic).OnComplete(() => incomeButton.transform.rotation = Quaternion.Euler(Vector3.zero));
        data.incomeMoney = (int)(data.incomeMoney * data.incomeMoneyMultiple);
        ButtonControl();
    }
    public void RedChanceButton()
    {
        if (data.money < data.redChanceMoney) return;

        data.money -= data.redChanceMoney;
        data.redChance += data.redChanceRise;
        redChanceButton.transform.DOShakeRotation(0.3f, 20, 5, 100, true, ShakeRandomnessMode.Harmonic).OnComplete(() => redChanceButton.transform.rotation = Quaternion.Euler(Vector3.zero));
        data.redChanceMoney = (int)(data.redChanceMoney * data.redChanceMoneyMultiple);
        ButtonControl();

    }
    public void YellowChanceButton()
    {
        if (data.money < data.yellowChanceMoney) return;

        data.money -= data.yellowChanceMoney;
        data.yellowChance += data.yellowChanceRise;
        yellowChanceButton.transform.DOShakeRotation(0.3f, 20, 5, 100, true, ShakeRandomnessMode.Harmonic).OnComplete(() => yellowChanceButton.transform.rotation = Quaternion.Euler(Vector3.zero));
        data.yellowChanceMoney = (int)(data.yellowChanceMoney * data.yellowChanceMoneyMultiple);
        ButtonControl();

    }
    public void IncomeButton()
    {
        if (data.money < data.incomeMoney) return;

        data.money -= data.incomeMoney;
        data.incomeLevel += data.incomeRise;
        incomeButton.transform.DOShakeRotation(0.3f, 20, 5, 100, true, ShakeRandomnessMode.Harmonic).OnComplete(() => incomeButton.transform.rotation = Quaternion.Euler(Vector3.zero));
        data.incomeMoney = (int)(data.incomeMoney * data.incomeMoneyMultiple);
        ButtonControl();

    }
    public void AdRedButton()
    {
        Eventmanager.showButtonRewarded?.Invoke(1);
    }
    public void AdYellowButton()
    {
        Eventmanager.showButtonRewarded?.Invoke(2);
    }
    public void AdIncomeButton()
    {
        Eventmanager.showButtonRewarded?.Invoke(3);
    }
    public void NoThanksButton()
    {
        Eventmanager.showInterstialAd?.Invoke();
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void VibrationButtonOn()
    {
        data.vibrationOn = true;
        vibrationButton.transform.GetChild(0).gameObject.SetActive(false);

    }
    public void VibrationButtonOff()
    {
        data.vibrationOn = false;
        vibrationButton.transform.GetChild(0).gameObject.SetActive(true);

    }
    public void ThreeNoahButton()
    {
        Eventmanager.showCoinRewarded?.Invoke(gameFinishMoney * 3);
    }
}

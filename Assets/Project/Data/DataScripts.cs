using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/CreatData")]
public class DataScripts : ScriptableObject
{
    public bool adsOn;
    public bool vibrationOn;
    public int tutorialCount;
    public float baseSpeed;
    public float speed;
    public int playCount;
    public float speedRise;
    public int money;
   public  int level;
    public int score;
    public int baseScore;
    public int redChance;
    public int yellowChance;
    public int incomeLevel;
    public int highscore;
    public int oldhighscore;

    public int redChanceMoney;
    public int yellowChanceMoney;
    public int incomeMoney;

    public int redChanceRise;
    public int yellowChanceRise;
    public int incomeRise;

    public float redChanceMoneyMultiple;
    public float yellowChanceMoneyMultiple;
    public float incomeMoneyMultiple;

    public int redChanceBase;
    public int yellowChanceBase;
    public int incomeBase;
    public int hightLevel;
    public int redChanceBaseMoney;
    public int yellowChanceBaseMoney;
    public int incomeBaseMoney;
    [SerializeField] public HightScoreEntry playerHightScore = new HightScoreEntry { score = 1, name = "User" };
    public List<HightScoreEntry> hightScoreList;
    [System.Serializable]
    public class HightScoreEntry
    {
        public int score;
        public string name;
    }
    public List<bool> isCheckPoint;
}

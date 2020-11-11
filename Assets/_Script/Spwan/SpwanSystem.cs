using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpwanSystem : MonoBehaviour
{
    public SpwanData[] Levels;
    public int countDeadEnemies {  get;  private set; }
    public Material openGateMat;
    public Material closeGateMat;
    public GameObject endLevelPanel;
    public GameObject finalLevelPanel;
    public TextMeshProUGUI levelNum;
    public TextMeshProUGUI eneimesNum;
    public TextMeshProUGUI coinNum;
    public TextMeshProUGUI collectCoin;
    public GameObject leveltext;
    public int levelCount = 0;
    public int currentCoinCount { get; private set; }
    public int saveCoinCount { get; private set; }

    public int globalCoinCount;

    private bool isfinshround = false;
    #region Singleton 
    public static SpwanSystem instance;
    void Awake()
    {
        instance = this;
        levelCount = SelectPlayer.instance.GetInstantiatePoint();
    }
    #endregion

    private void Start()
    {
        isfinshround = false;
        leveltext.GetComponent<Animator>().SetTrigger("Anim");
        leveltext.GetComponent<TextMeshProUGUI>().text = "Level " + (levelCount + 1);
        countDeadEnemies = 0;
        Spwan(levelCount);
    }

    public void SetCountDeadEnemies(int deadnumber)
    {
        countDeadEnemies += deadnumber;
    }
    public void SetCoinCount(int amount)
    {
        currentCoinCount += amount;
        //print(currentCoinCount);
    }
    public int GetCoinCount()
    {
        return currentCoinCount;
    }
    public void SetGlobalCoins()
    {
        globalCoinCount = GetGlobalCoins() + GetCoinCount();
        PlayerPrefs.SetInt("Coins", globalCoinCount);
    }
    public int GetGlobalCoins()
    {
        return saveCoinCount = PlayerPrefs.GetInt("Coins");
    }

    private void Update()
    {
        if (countDeadEnemies == Levels[levelCount].enemiesCountToSpwan + 1  && !isfinshround)
        {
            SetGlobalCoins();
            WinLevel();

            //countDeadEnemies = 0;
            //if (Levels[levelCount].LevelGate != null)
            //{
            //    Levels[levelCount].LevelGate.GetComponent<MeshRenderer>().material = openGateMat;
            //    Levels[levelCount].LevelGate.GetComponent<MeshCollider>().isTrigger = true;
            //}
            //levelCount++;
            //leveltext.GetComponent<Animator>().SetTrigger("Anim");
            //leveltext.GetComponent<TextMeshProUGUI>().text = "Level " + (levelCount + 1);
            //Spwan(levelCount);  
        }
    }

    private void WinLevel()
    {
        isfinshround = true;
        if (levelCount == 9)
            finalLevelPanel.SetActive(true);
        else
            endLevelPanel.SetActive(true);
        eneimesNum.text = countDeadEnemies.ToString();
        coinNum.text = currentCoinCount.ToString();
        collectCoin.text = globalCoinCount.ToString();
        levelNum.text = (levelCount + 1).ToString();


        if (LevelSelector.instance.caveLevelReached <= levelCount+2) {
            if (SceneManager.GetActiveScene().name == "CaveScnce")
            {
                PlayerPrefs.SetInt("CavelevelReached", levelCount + 2);
            }
        }
        if (LevelSelector.instance.dungeonLevelReached <= levelCount)
        {
            if (SceneManager.GetActiveScene().name == "DungeonScnce")
            {
                PlayerPrefs.SetInt("DungeonlevelReached", levelCount + 2);
            }
        }
        if (LevelSelector.instance.forestDayLevelReached <= levelCount)
        {
            if (SceneManager.GetActiveScene().name == "ForestDayScnce")
            {
                PlayerPrefs.SetInt("ForestDaylevelReached", levelCount + 2);
            }
        }
        if (LevelSelector.instance.forestNightLevelReached <= levelCount)
        {
            if (SceneManager.GetActiveScene().name == "ForestNightScnce")
            {
                PlayerPrefs.SetInt("ForestNightlevelReached", levelCount + 2);
            }
        }
        if (LevelSelector.instance.desertLevelReached <= levelCount)
        {
            if (SceneManager.GetActiveScene().name == "DesertScnce")
            {
                PlayerPrefs.SetInt("DesertlevelReached", levelCount + 2);
            }
        }
    }

    public void OpenNextLevel()
    {
        isfinshround = false;
        countDeadEnemies = 0;
        levelCount++;
        if (Levels[levelCount - 1].LevelGate != null) 
        {
            Levels[levelCount - 1].LevelGate.GetComponent<MeshRenderer>().material = openGateMat;
            Levels[levelCount - 1].LevelGate.GetComponent<MeshCollider>().isTrigger = true;
        }
        leveltext.GetComponent<Animator>().SetTrigger("Anim");
        leveltext.GetComponent<TextMeshProUGUI>().text = "Level " + (levelCount + 1);
        Spwan(levelCount);
        EnemyController.instance.PowerUpEneimes();
        endLevelPanel.SetActive(false);
    }
    private void Spwan(int level)
    {
        for (int i = 0; i <= Levels[level].enemiesCountToSpwan - 1 ; i++) 
        {
            Instantiate(Levels[level].spwanEnemies[Random.Range(0, Levels[level].spwanEnemies.Count)], Levels[level].SpwanPoints[Random.Range(0, Levels[level].SpwanPoints.Count)].transform.position, Quaternion.identity);
            //leveltext.GetComponent<Animator>().SetTrigger("Anim");
            //leveltext.GetComponent<TextMeshProUGUI>().text = "Level " + (level + 1);
        }
        Instantiate(Levels[level].gateEnemy, Levels[level].spawnGateEnemyPoint.transform.position,Quaternion.identity);
    }
}
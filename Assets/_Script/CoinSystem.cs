using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;
using System.Dynamic;

public class CoinSystem : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI alertText;
    public Animator anim;

    public GameObject caveBuyButton;
    public GameObject caveOpenButton;

    public GameObject dungeonBuyButton;
    public GameObject dungeonOpenButton;

    public GameObject forestDayBuyButton;
    public GameObject forestDayOpenButton;

    public GameObject forsetNightBuyButton;
    public GameObject forsetNighOpenButton;

    public GameObject desertBuyButton;
    public GameObject desertOpenButton;

    public bool haveKeys;

    int isCaveMapAvilable;
    int isDungeonMapAvilable;
    int isForestDayMapAvilable;
    int isForestNightMapAvilable;
    int isDesertMapAvilable;

    string mapName;
    private int owndMoney;
    private int keys;
    public int savedKeys;
    private bool buyHappens = false;
    

    #region Singleton 
    public static CoinSystem instance;
    void Awake()
    {
        instance = this;
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("Coins", 5);
        //PlayerPrefs.SetInt("Keys", 5);
        owndMoney = PlayerPrefs.GetInt("Coins");
        savedKeys = PlayerPrefs.GetInt("Keys");
        buyHappens = false;
        coinText.text = owndMoney.ToString();
        keyText.text = savedKeys.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        savedKeys = PlayerPrefs.GetInt("Keys");
        isCaveMapAvilable = PlayerPrefs.GetInt("Cave");
        isDungeonMapAvilable = PlayerPrefs.GetInt("Dungeon");
        isForestDayMapAvilable = PlayerPrefs.GetInt("ForestDay");
        isForestNightMapAvilable = PlayerPrefs.GetInt("ForestNight");
        isDesertMapAvilable = PlayerPrefs.GetInt("Desert");
        CheckButton(isCaveMapAvilable, caveBuyButton, caveOpenButton);
        CheckButton(isDungeonMapAvilable, dungeonBuyButton, dungeonOpenButton);
        CheckButton(isForestDayMapAvilable, forestDayBuyButton, forestDayOpenButton);
        CheckButton(isForestNightMapAvilable, forsetNightBuyButton, forsetNighOpenButton);
        CheckButton(isDesertMapAvilable, desertBuyButton, desertOpenButton);
    }

    private void CheckButton(int isMapAvilabe, GameObject buyButton,GameObject openButton)
    {
        if (isMapAvilabe == 0)
        {
            buyButton.SetActive(true);
            openButton.SetActive(false);
        }
        else
        {
            openButton.SetActive(true);
            buyButton.SetActive(false);
        }
    }

    public void BuyKeys(int buyAmmount)
    {
        if (owndMoney >= buyAmmount)
        {
            owndMoney -= buyAmmount;
            PlayerPrefs.SetInt("Coins", owndMoney);
            owndMoney = PlayerPrefs.GetInt("Coins");
            coinText.text = owndMoney.ToString();
            buyHappens = true;
        }
        else
        {
            anim.SetTrigger("Anim");
            alertText.text = "Not Have enough Coins";
        }
    }
    public void AddKey(int amountKeysBuy)
    {
        if (buyHappens)
        {
            keys = savedKeys + amountKeysBuy;
            PlayerPrefs.SetInt("Keys", keys);
            buyHappens = false;
            keyText.text = keys.ToString();
        }
    }


    public void UseKeys(int amountUse)
    {     
        if(savedKeys >= amountUse)
        {
            savedKeys -= amountUse;
            PlayerPrefs.SetInt("Keys", savedKeys);
            PlayerPrefs.SetInt(mapName, 1);
            keyText.text = savedKeys.ToString();
            haveKeys =  true;
        }
        else
        {
            anim.SetTrigger("Anim");
            alertText.text = "Not Have enough Keys";
            haveKeys = false;
        }
    }
    public void BuyCoins(int buyAmmount)
    {
        int buyCoins = owndMoney + buyAmmount;
        PlayerPrefs.SetInt("Coins", buyCoins);
        coinText.text = buyCoins.ToString();
    }
    public void Massage(string massage)
    {
        anim.SetTrigger("Anim");
        alertText.text = massage;
    }
    public void GetMapName(string name)
    {
        mapName = name;
    }
}

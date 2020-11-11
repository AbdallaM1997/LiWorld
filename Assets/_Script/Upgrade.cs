using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    #region Health Attribute
    const int healthUpgradeAmount = 10;
    const int mainHealth = 100;
 
    public TextMeshProUGUI healthText;
    [HideInInspector]
    public int globalHealth;

    int tempUpgradeAmountHealth;
    #endregion

    #region Damage Attribute
    const int damageUpgradeAmount = 1;
    const int mainDamage = 10;
    [HideInInspector]
    public int globalDamage;
    int tempUpgradeAmountDamge;
    public TextMeshProUGUI damageText;
    #endregion

    #region Singleton 
    public static Upgrade instance;
    private void Awake()
    {
        instance = this;
        tempUpgradeAmountHealth = PlayerPrefs.GetInt("TempHealthUpgradeAmmount");
        SetGlobalHealth(mainHealth + tempUpgradeAmountHealth);
        healthText.text = GetGlobalHealth().ToString();
        tempUpgradeAmountDamge = PlayerPrefs.GetInt("TempDamageUpgradeAmmount");
        SetGlobalDamage(mainDamage + tempUpgradeAmountDamge);
        damageText.text = GetGlobalDamage().ToString();
    }
    #endregion

    #region Global Health Getter And Setter
    public void SetGlobalHealth(int amount)
    {
        PlayerPrefs.SetInt("HealthAmount", amount);
        globalHealth = amount;
        
    }
    public int GetGlobalHealth()
    {
        globalHealth =  PlayerPrefs.GetInt("HealthAmount");
        return globalHealth;
    }
    #endregion

    #region Global Damage Getter And Setter
    public void SetGlobalDamage(int amount)
    {
        PlayerPrefs.SetInt("DamageAmount", amount);
        globalDamage = amount;

    }
    public int GetGlobalDamage()
    {
        globalDamage = PlayerPrefs.GetInt("DamageAmount");
        return globalDamage;
    }
    #endregion

    public void UpgradeHealth(int amountUse)
    {
        if(CoinSystem.instance.savedKeys >= amountUse)
        {
            CoinSystem.instance.savedKeys -= amountUse;
            PlayerPrefs.SetInt("Keys", CoinSystem.instance.savedKeys);
            tempUpgradeAmountHealth += healthUpgradeAmount;
            PlayerPrefs.SetInt("TempHealthUpgradeAmmount", tempUpgradeAmountHealth);
            SetGlobalHealth(GetGlobalHealth() + healthUpgradeAmount);
            healthText.text = GetGlobalHealth().ToString();
            CoinSystem.instance.keyText.text = CoinSystem.instance.savedKeys.ToString();
        }
        else
        {
            CoinSystem.instance.anim.SetTrigger("Anim");
            CoinSystem.instance.alertText.text = "Not Have enough Keys";
        }
    }

    public void UpgradeDamage(int amountUse)
    {
        if (CoinSystem.instance.savedKeys >= amountUse)
        {
            CoinSystem.instance.savedKeys -= amountUse;
            PlayerPrefs.SetInt("Keys", CoinSystem.instance.savedKeys);
            tempUpgradeAmountDamge += damageUpgradeAmount;
            PlayerPrefs.SetInt("TempDamageUpgradeAmmount", tempUpgradeAmountDamge);
            SetGlobalDamage(GetGlobalDamage() + damageUpgradeAmount);
            damageText.text = GetGlobalDamage().ToString();
            CoinSystem.instance.keyText.text = CoinSystem.instance.savedKeys.ToString();
        }
        else
        {
            CoinSystem.instance.anim.SetTrigger("Anim");
            CoinSystem.instance.alertText.text = "Not Have enough Keys";
        }
    }
}

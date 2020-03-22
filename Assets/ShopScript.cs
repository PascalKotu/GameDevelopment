using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] ShopStatistics shopStats;
    [SerializeField] AudioClip buySFX, errorSFX;
    [SerializeField] AudioSource audioSource;

    [SerializeField] TMP_Text costHealthTxt, costMeleeTxt, costRangedTxt, costMaxMunitionsTxt, costMunitionsTxt;
 
    int baseCostHealth, baseCostMelee, baseCostRange, baseCostMaxMunitions, costMunitions;
    int costHealth, costMelee, costRange, CostMaxMunitions;

    public void Start()
    {
        baseCostHealth = 5;
        baseCostMelee = 5;
        baseCostRange = 10;
        baseCostMaxMunitions = 15;
        costMunitions = 1;

        CalcNewCosts();
        SetTextFields();
    }



    public void UpgradeHealth()
    {
        if(PayUpgrade(baseCostHealth, shopStats.maxHealthUpgradeCount))
        {
            playerStats.maxHealth += 2;
            shopStats.maxHealthUpgradeCount++;

            CalcNewCosts();
            SetTextFields();
        }
    }
    public void UpgradeMeleeDmg()
    {
        if (PayUpgrade(baseCostMelee, shopStats.meleeDamageUpgradeCount))
        {
            playerStats.meleeDamage ++;
            shopStats.meleeDamageUpgradeCount++;

            CalcNewCosts();
            SetTextFields();
        }
    }
    public void UpgradeRangeDmg()
    {
        if (PayUpgrade(baseCostRange, shopStats.rangedDamageUpgradeCount))
        {
            playerStats.rangedDamage ++;
            shopStats.rangedDamageUpgradeCount++;

            CalcNewCosts();
            SetTextFields();
        }
    }
    public void UpgradeMaxMunitions()
    {
        if (PayUpgrade(baseCostMaxMunitions, shopStats.maxMunitionUpgradeCount))
        {
            playerStats.maxMunition += 3;
            shopStats.maxMunitionUpgradeCount++;

            CalcNewCosts();
            SetTextFields();

        }
    }
    public void BuyMunitions()
    {
        if (PayUpgrade(costMunitions, 0))
        {
            if (playerStats.munition < playerStats.maxMunition)
                playerStats.munition += 2;
            else audioSource.PlayOneShot(errorSFX);
        }
            
       
    }

    bool PayUpgrade(int baseCost, int upgradeLevel)
    {
        int cost = UpgradeCost(baseCost, upgradeLevel);

        if(cost > playerStats.money)
        {
            audioSource.PlayOneShot(errorSFX);
            return false;
        }

        //TODO live update money UI
        playerStats.money -= cost;

        audioSource.PlayOneShot(buySFX);

        return true;
    }
    int UpgradeCost(int baseCost, int upgradeLevel)
    {
        return baseCost * (upgradeLevel * upgradeLevel + 1);
    }

    private void SetTextFields()
    {
        costHealthTxt.text = costHealth.ToString();
        costMeleeTxt.text = costMelee.ToString();
        costRangedTxt.text = costRange.ToString();
        costMaxMunitionsTxt.text = CostMaxMunitions.ToString();
        costMunitionsTxt.text = costMunitions.ToString();
    }
    private void CalcNewCosts()
    {
        costHealth = UpgradeCost(5, shopStats.maxHealthUpgradeCount);
        costMelee = UpgradeCost(5, shopStats.meleeDamageUpgradeCount);
        costRange = UpgradeCost(10, shopStats.rangedDamageUpgradeCount);
        CostMaxMunitions = UpgradeCost(15, shopStats.maxMunitionUpgradeCount);
    }


    public void ResetScriptables()
    {
        playerStats.maxHealth = 5;
        playerStats.money = 100;
        playerStats.rangedDamage = 1;
        playerStats.meleeDamage = 2;
        playerStats.maxMunition = 3;
        playerStats.munition = 0;

        shopStats.maxHealthUpgradeCount = 0;
        shopStats.meleeDamageUpgradeCount = 0;
        shopStats.rangedDamageUpgradeCount = 0;
        shopStats.maxMunitionUpgradeCount = 0;

        CalcNewCosts();
        SetTextFields();
    }

}

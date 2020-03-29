using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats = default;
    [SerializeField] ShopStatistics shopStats = default;
    [SerializeField] AudioClip buySFX = default, errorSFX = default;

    [SerializeField] TMP_Text costHealthTxt = default, costMeleeTxt = default, costRangedTxt = default, costMaxMunitionsTxt = default, costMunitionsTxt = default;
 
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
            playerStats.maxHealth += 3;
            shopStats.maxHealthUpgradeCount++;

            CalcNewCosts();
            SetTextFields();
        }
    }
    public void UpgradeMeleeDmg()
    {
        if (PayUpgrade(baseCostMelee, shopStats.meleeDamageUpgradeCount))
        {
            playerStats.meleeDamage +=2;
            shopStats.meleeDamageUpgradeCount++;

            CalcNewCosts();
            SetTextFields();
        }
    }
    public void UpgradeRangeDmg()
    {
        if (PayUpgrade(baseCostRange, shopStats.rangedDamageUpgradeCount))
        {
            playerStats.rangedDamage += 3;
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
            GameEvents.ChangeMunition.Invoke(0);
            CalcNewCosts();
            SetTextFields();

        }
    }
    public void BuyMunitions()
    {
        if (playerStats.munition < playerStats.maxMunition) {
            if (PayUpgrade(costMunitions, 0)) {
                GameEvents.ChangeMunition.Invoke(1);
            }
        } else {
            GameEvents.PlaySound.Invoke(new AudioEventData(errorSFX, 1f));
        }
        



    }

    bool PayUpgrade(int baseCost, int upgradeLevel)
    {
        int cost = UpgradeCost(baseCost, upgradeLevel);

        if(cost > playerStats.money)
        {
            GameEvents.PlaySound.Invoke(new AudioEventData(errorSFX, 1f));
            return false;
        }
        
        GameEvents.ChangeMoney.Invoke(-cost);

        GameEvents.PlaySound.Invoke(new AudioEventData(buySFX, 1f));

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
        playerStats.money = 0;
        playerStats.rangedDamage = 2;
        playerStats.meleeDamage = 1;
        playerStats.maxMunition = 3;
        playerStats.munition = 0;

        shopStats.maxHealthUpgradeCount = 0;
        shopStats.meleeDamageUpgradeCount = 0;
        shopStats.rangedDamageUpgradeCount = 0;
        shopStats.maxMunitionUpgradeCount = 0;

        GameEvents.ChangeMoney.Invoke(0);
        GameEvents.ChangeMunition.Invoke(0);
        CalcNewCosts();
        SetTextFields();
    }

}

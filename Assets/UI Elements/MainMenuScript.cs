﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    [SerializeField] PlayerStats playerStats = default;
    [SerializeField] ShopStatistics shopStatistics = default;
    [SerializeField] Button continueButton = default;
    [SerializeField] GameObject howToPlay = default;
    void Start() {
        if(PlayerPrefs.GetInt("GameStarted") == 1) {
            continueButton.interactable = true;
        } else {
            continueButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void ToggleHowToPlay() {
        howToPlay.SetActive(!howToPlay.activeSelf);
    }

    public void StartNewGame() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("GameStarted", 1);

        SceneManager.LoadScene(4);

    }

    public void ContinueGame() {
        playerStats.maxHealth = PlayerPrefs.GetInt("maxHealth");
        playerStats.maxMunition = PlayerPrefs.GetInt("maxMunition");
        playerStats.meleeDamage = PlayerPrefs.GetInt("meleeDamage");
        playerStats.money = PlayerPrefs.GetInt("money");
        playerStats.munition = PlayerPrefs.GetInt("munition");
        playerStats.rangedDamage = PlayerPrefs.GetInt("rangedDamage");

        shopStatistics.maxHealthUpgradeCount = PlayerPrefs.GetInt("maxHealthUpgradeCount");
        shopStatistics.maxMunitionUpgradeCount = PlayerPrefs.GetInt("maxMunitionUpgradeCount");
        shopStatistics.meleeDamageUpgradeCount = PlayerPrefs.GetInt("meleeDamageUpgradeCount");
        shopStatistics.munitionUpgradeCount = PlayerPrefs.GetInt("munitionUpgradeCount");
        shopStatistics.rangedDamageUpgradeCount = PlayerPrefs.GetInt("rangedDamageUpgradeCount");

        SceneManager.LoadScene("Base");
    }
}

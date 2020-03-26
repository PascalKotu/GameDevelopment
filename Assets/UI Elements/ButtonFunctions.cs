using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonFunctions : MonoBehaviour {
    [SerializeField] GameObject gameOverScreen = default;
    [SerializeField] AudioClip gameOvermusic = default;

    [SerializeField] PlayerStats playerStats = default;
    [SerializeField] ShopStatistics shopStatistics = default;
    [SerializeField] GameObject pauseScreen = default;
    [SerializeField] TextMeshProUGUI healthText = default;
    [SerializeField] TextMeshProUGUI meeleDmgText = default;
    [SerializeField] TextMeshProUGUI arrowDmgText = default;

    [SerializeField] TextMeshProUGUI coinText = default;
    int coins = 0;

    [SerializeField] TextMeshProUGUI arrowText = default;
    int maxArrows = 3;
    int currentArrow = 0;

    bool dead = false;
    float lastTimeScale = 0f;

    void Start() {
        GameEvents.PlayerDead.AddListener(OnPlayerDead);
        GameEvents.ChangeMoney.AddListener(ChangeCoinScore);
        GameEvents.ChangeMunition.AddListener(ChangeArrows);

        maxArrows = playerStats.maxMunition;
        currentArrow = playerStats.munition;
        arrowText.text = currentArrow.ToString() + "/" + maxArrows.ToString();

        coins = playerStats.money;
        coinText.text = coins.ToString();


        PlayerPrefs.SetInt("maxHealth", playerStats.maxHealth);
        PlayerPrefs.SetInt("maxMunition", playerStats.maxMunition);
        PlayerPrefs.SetInt("meleeDamage", playerStats.meleeDamage);
        PlayerPrefs.SetInt("money", playerStats.money);
        PlayerPrefs.SetInt("munition", playerStats.munition);
        PlayerPrefs.SetInt("rangedDamage", playerStats.rangedDamage);

        PlayerPrefs.SetInt("maxHealthUpgradeCount", shopStatistics.maxHealthUpgradeCount);
        PlayerPrefs.SetInt("maxMunitionUpgradeCount", shopStatistics.maxMunitionUpgradeCount);
        PlayerPrefs.SetInt("meleeDamageUpgradeCount", shopStatistics.meleeDamageUpgradeCount);
        PlayerPrefs.SetInt("munitionUpgradeCount", shopStatistics.munitionUpgradeCount);
        PlayerPrefs.SetInt("rangedDamageUpgradeCount", shopStatistics.rangedDamageUpgradeCount);

    }
    private void Update() {
        if (Input.GetButtonDown("Cancel") && !dead) {
            EnablePauseScreen();
        }
    }

    void OnPlayerDead() {
        dead = true;
        Invoke("EnableGameOver", 2f);
    }

    void ChangeCoinScore(int x) {
        playerStats.money += x;
        coins = playerStats.money;
        coinText.text = coins.ToString();
    }

    void ChangeArrows(int x) {
        maxArrows = playerStats.maxMunition;
        playerStats.munition += x;
        currentArrow = playerStats.munition;
        arrowText.text = currentArrow.ToString() + "/" + maxArrows.ToString();
    }



    void EnableGameOver() {
        GameEvents.PlayMusic.Invoke(new AudioEventData(gameOvermusic, 1f));
        GameEvents.ToggleMusicLoop.Invoke();
        gameOverScreen.SetActive(true);
    }

    public void RestartScene() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Base");
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void EnablePauseScreen() {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        float x = Time.timeScale;
        Time.timeScale = lastTimeScale;
        lastTimeScale = x;
        healthText.text = " = " + shopStatistics.maxHealthUpgradeCount.ToString();
        meeleDmgText.text = " = " + shopStatistics.meleeDamageUpgradeCount.ToString();
        arrowDmgText.text = " = " + shopStatistics.rangedDamageUpgradeCount.ToString();
    }
    
}

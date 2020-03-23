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

    [SerializeField] TextMeshProUGUI coinText = default;
    int coins = 0;

    [SerializeField] TextMeshProUGUI arrowText = default;
    int maxArrows = 3;
    int currentArrow = 0;

    void Start() {
        GameEvents.PlayerDead.AddListener(OnPlayerDead);
        GameEvents.ChangeMoney.AddListener(ChangeCoinScore);
        GameEvents.ChangeMunition.AddListener(ChangeArrows);

        maxArrows = playerStats.maxMunition;
        currentArrow = playerStats.munition;
        arrowText.text = currentArrow.ToString() + "/" + maxArrows.ToString();

        coins = playerStats.money;
        coinText.text = coins.ToString();
        
    }

    void OnPlayerDead() {
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
        SceneManager.LoadScene("Base");
    }

    
}

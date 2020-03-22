using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonFunctions : MonoBehaviour {
    [SerializeField] GameObject gameOverScreen = default;
    [SerializeField] AudioClip gameOvermusic = default;

    [SerializeField] TextMeshProUGUI coinText = default;
    int coins = 0;
    void Start() {
        GameEvents.PlayerDead.AddListener(OnPlayerDead);
        GameEvents.PickUpCoin.AddListener(IncreaseCoinScore);
        coinText.text = coins.ToString();
    }

    void OnPlayerDead() {
        Invoke("EnableGameOver", 2f);
    }

    void IncreaseCoinScore() {
        coins += 1;
        coinText.text = coins.ToString();
    }

    void EnableGameOver() {
        GameEvents.PlayMusic.Invoke(new AudioEventData(gameOvermusic, 1f));
        GameEvents.ToggleMusicLoop.Invoke();
        gameOverScreen.SetActive(true);
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

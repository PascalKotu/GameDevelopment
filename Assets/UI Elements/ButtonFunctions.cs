using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour {
    [SerializeField] GameObject gameOverScreen = default;
    [SerializeField] AudioClip gameOvermusic = default;
    void Start() {
        GameEvents.PlayerDead.AddListener(OnPlayerDead);
    }

    void OnPlayerDead() {
        Invoke("EnableGameOver", 2f);
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

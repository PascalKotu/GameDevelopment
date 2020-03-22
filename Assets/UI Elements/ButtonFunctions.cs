using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonFunctions : MonoBehaviour {
    [SerializeField] GameObject gameOverScreen = default;
    [SerializeField] AudioClip gameOvermusic = default;
    [SerializeField] AudioClip winMusic = default;

    [SerializeField] TextMeshProUGUI coinText = default;
    [SerializeField] GameObject boss = default;
    [SerializeField] Image bossHealthbar = default;
    [SerializeField] TextMeshProUGUI bossName = default;
    int coins = 0;
    int bossMaxLife = 0;
    int bossCurrentLife = 0;

    void Start() {
        GameEvents.PlayerDead.AddListener(OnPlayerDead);
        GameEvents.PickUpCoin.AddListener(IncreaseCoinScore);
        GameEvents.BossStart.AddListener(BossSpawn);
        GameEvents.enemyHit.AddListener(BossDMG);
        coinText.text = coins.ToString();
        if (boss) {
            bossMaxLife = boss.GetComponent<EnemyHealth>().GetMaxHP();
            bossCurrentLife = bossMaxLife;

        }
    }

    void OnPlayerDead() {
        Invoke("EnableGameOver", 2f);
    }

    void IncreaseCoinScore() {
        coins += 1;
        coinText.text = coins.ToString();
    }

    void BossSpawn () {
        bossName.transform.parent.gameObject.SetActive(true);
        bossName.text = boss.name;
        StartCoroutine("HealthBarDecay");
    }
    
    void BossDMG(HitData hitted) {
        if(hitted.hitted == boss) {
            bossCurrentLife -= hitted.dmg;
            if(bossCurrentLife <= 0) {
                GameEvents.PlayMusic.Invoke(new AudioEventData(winMusic, 1f));
                GameEvents.ToggleMusicLoop.Invoke();
            }
            StartCoroutine("HealthBarDecay");
        }
    }

    void EnableGameOver() {
        GameEvents.PlayMusic.Invoke(new AudioEventData(gameOvermusic, 1f));
        GameEvents.ToggleMusicLoop.Invoke();
        gameOverScreen.SetActive(true);
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator HealthBarDecay() {
        //will empty the healthbar in slow steps to create an "animation"
        while (Mathf.Abs( bossHealthbar.fillAmount - (float)bossCurrentLife / (float)bossMaxLife) > Mathf.Epsilon) {
            float newAmmount = (float)bossCurrentLife / (float)bossMaxLife;

            if (bossHealthbar.fillAmount < newAmmount) {
                bossHealthbar.fillAmount += Time.deltaTime;
                if(bossHealthbar.fillAmount > newAmmount) {
                    bossHealthbar.fillAmount = newAmmount;
                }
            } else {
                bossHealthbar.fillAmount -= Time.deltaTime;
                if (bossHealthbar.fillAmount < newAmmount) {
                    bossHealthbar.fillAmount = newAmmount;
                }
            }
            yield return null;
        }
        yield return null;
    }
}

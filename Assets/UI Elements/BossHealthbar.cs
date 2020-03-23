using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthbar : MonoBehaviour {
    [SerializeField] GameObject boss = default;
    [SerializeField] GameObject portal = default;
    [SerializeField] Image bossHealthbar = default;
    [SerializeField] TextMeshProUGUI bossName = default;

    [SerializeField] AudioClip winMusic = default;


    int bossMaxLife = 0;
    int bossCurrentLife = 0;
    // Start is called before the first frame update
    void Start() {
        GameEvents.BossStart.AddListener(BossSpawn);
        GameEvents.enemyHit.AddListener(BossDMG);
        if (boss) {
            bossMaxLife = boss.GetComponent<EnemyHealth>().GetMaxHP();
            bossCurrentLife = bossMaxLife;

        }
    }

    void BossSpawn() {
        bossName.transform.parent.gameObject.SetActive(true);
        bossName.text = boss.name;
        StartCoroutine("HealthBarDecay");
    }

    void BossDMG(HitData hitted) {
        if (hitted.hitted == boss) {
            bossCurrentLife -= hitted.dmg;
            if (bossCurrentLife <= 0) {
                portal.SetActive(true);
                GameEvents.PlayMusic.Invoke(new AudioEventData(winMusic, 1f));
                GameEvents.ToggleMusicLoop.Invoke();
            }
            StartCoroutine("HealthBarDecay");
        }
    }

    IEnumerator HealthBarDecay() {
        //will empty the healthbar in slow steps to create an "animation"
        while (Mathf.Abs(bossHealthbar.fillAmount - (float)bossCurrentLife / (float)bossMaxLife) > Mathf.Epsilon) {
            float newAmmount = (float)bossCurrentLife / (float)bossMaxLife;

            if (bossHealthbar.fillAmount < newAmmount) {
                bossHealthbar.fillAmount += Time.deltaTime;
                if (bossHealthbar.fillAmount > newAmmount) {
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

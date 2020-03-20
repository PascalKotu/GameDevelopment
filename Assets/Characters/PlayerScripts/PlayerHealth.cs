using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] Image healthbar = default;
    [SerializeField] int maxHp = 5;
    int currentHp = 0;

    void Start() {
        GameEvents.PlayerHit.AddListener(GetHit);
        currentHp = maxHp;
        healthbar.fillAmount = currentHp / maxHp;
    }

    void GetHit(HitData hitData) {
        if (hitData.hitted = gameObject) {
            currentHp -= hitData.dmg;
            StartCoroutine("HealthBarDecay");

            if (currentHp <= 0) {
                GameEvents.PlayerDead.Invoke();
            } else {
                //the lower the hp, the stronger the screenshake
                GameEvents.CameraShake.Invoke(1- (float)currentHp / (float)maxHp);
            }
        }
    }

    IEnumerator HealthBarDecay() {
        //will empty the healthbar in slow steps to create an "animation"
        while (healthbar.fillAmount > (float)currentHp / (float)maxHp) {
            healthbar.fillAmount -= Time.deltaTime;
            if (healthbar.fillAmount < (float)currentHp / (float)maxHp) {
                healthbar.fillAmount = (float)currentHp / (float)maxHp;
            }
            yield return null;
        }
        yield return null;
    }
}

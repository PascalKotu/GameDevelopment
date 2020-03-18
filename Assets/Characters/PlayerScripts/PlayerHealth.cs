using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] int maxHp = 5;
    int currentHp = 0;
    void Start() {
        GameEvents.PlayerHit.AddListener(GetHit);
        currentHp = maxHp;
    }

    void GetHit(HitData hitData) {
        if(hitData.hitted = gameObject) {
            currentHp -= hitData.dmg;
        }
    }
}

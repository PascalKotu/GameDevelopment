using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents {
    public static HitEvent enemyHit = new HitEvent();
    public static HitEvent PlayerHit = new HitEvent();
}   
   
public class HitEvent : UnityEvent<HitData> { }

public class HitData {
    public Transform hitPosition;
    public GameObject hitted;
    public int dmg;

    public HitData(Transform hitPosition, GameObject hitted, int dmg) {
        this.hitPosition = hitPosition;
        this.hitted = hitted;
        this.dmg = dmg;
    }
}

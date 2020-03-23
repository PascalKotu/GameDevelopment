using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents {
    public static HitEvent enemyHit = new HitEvent();
    public static HitEvent EnemyDead = new HitEvent();
    public static HitEvent PlayerHit = new HitEvent();
    public static CameraShakeEvent CameraShake = new CameraShakeEvent();
    public static UnityEvent PlayerDead = new UnityEvent();
    public static UnityEvent PlatformPass = new UnityEvent();
    public static AudioEvent PlaySound = new AudioEvent();
    public static AudioEvent PlayMusic = new AudioEvent();
    public static UnityEvent ToggleMusicLoop = new UnityEvent();
    public static IntEvent ChangeMoney = new IntEvent();
    public static IntEvent ChangeMunition = new IntEvent();
    public static UnityEvent BossStart = new UnityEvent();
}   
   
public class HitEvent : UnityEvent<HitData> { }

public class CameraShakeEvent : UnityEvent<float> { }

public class IntEvent : UnityEvent<int> { }

public class AudioEvent : UnityEvent<AudioEventData> { }

public class AudioEventData {
    public AudioClip clip;
    public float volume;

    public AudioEventData(AudioClip clip, float volume) {
        this.clip = clip;
        this.volume = volume;
    }
}

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

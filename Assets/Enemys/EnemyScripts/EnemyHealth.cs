using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] int maxHp = 3;
    int currentHp = 0;

    [SerializeField] GameObject despawnEffect = default;
    [SerializeField] List<GameObject> drops = new List<GameObject>();
    [SerializeField] List<AudioClip> hitSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> deathSounds = new List<AudioClip>();

    Animator animator = default;
    
    Vector2 hitPosition = default;

    void Start() {
        GameEvents.enemyHit.AddListener(GetHit);
        animator = GetComponent<Animator>();
        currentHp = maxHp;
    }
    
    void GetHit(HitData hitData) {
        //is called via enemyHit-Event
        if (hitData.hitted == gameObject ) {
            currentHp -= hitData.dmg;
            GameEvents.PlaySound.Invoke(new AudioEventData(hitSounds[Random.Range(0, hitSounds.Count)], 1f));
            if (currentHp <= 0) {
                //gets the position of the object that hitted this one
                hitPosition = hitData.hitPosition.position;
                animator.SetTrigger("Dead");
            }
        }
    }

    void Die() {
        //is called via animation-event

        //spawn one of the drops
        Instantiate(drops[Random.Range(0, drops.Count)], transform.position, Quaternion.identity);

        //spawn deathparticles
        Instantiate(despawnEffect, transform.position, Quaternion.FromToRotation(transform.up, (Vector2)transform.position- hitPosition));

        //play sound
        GameEvents.PlaySound.Invoke(new AudioEventData(deathSounds[Random.Range(0, deathSounds.Count)], 1f));

        Destroy(gameObject);
    }
}

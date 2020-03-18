using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] int maxHp = 3;
    int currentHp = 0;

    [SerializeField] GameObject despawnEffect = default;
    
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
            if (currentHp <= 0) {
                //gets the position of the object that hitted this one
                hitPosition = hitData.hitPosition.position;
                animator.SetTrigger("Dead");
            }
        }
    }

    void Die() {
        //is called via animation-event
        GameObject x = Instantiate(despawnEffect, transform.position, Quaternion.FromToRotation(transform.up, (Vector2)transform.position- hitPosition));
        Destroy(gameObject);
    }
}

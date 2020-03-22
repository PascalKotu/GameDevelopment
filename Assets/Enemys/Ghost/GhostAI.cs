using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour {
    Rigidbody2D rb = default;
    SpriteRenderer sprite = default;
    [SerializeField] Transform target = default;
    [SerializeField] float range = 10f;
    [SerializeField] float speed = 3f;
    [SerializeField] int dmg = 2;
    [SerializeField] float timeBetweenAttacks = 1f;
    bool inrange = false;
    [SerializeField] Collider2D dmgCollider = default;
    bool hit = false;
    [SerializeField] float knockBackSpeed = 5f;
    Vector2 knockBack = Vector2.zero;
    [SerializeField] Material hitMaterial = default;
    Material defaultMaterial = default;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        GameEvents.enemyHit.AddListener(HitEffect);
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        defaultMaterial = sprite.material;
    }

    // Update is called once per frame
    void Update() {
        if(target.position.x < transform.position.x- 0.1f) {
            sprite.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }

        if(Mathf.Abs(transform.position.x - target.position.x) <= range) {
            inrange = true;
        } else {
            inrange = false;
        }
    }

    private void FixedUpdate() {
        if (hit) {
            rb.velocity = knockBack * knockBackSpeed;
        } else {
            if (inrange) {
                rb.velocity = (target.position - transform.position).normalized * speed;
            } else {
                rb.velocity = Vector3.zero;
            }
            
        }
        
    }


    void HitEffect(HitData hitData) {
        if (hitData.hitted == gameObject) {
            hit = true;
            sprite.material = hitMaterial;

            //get the knockback direction
            knockBack = (transform.position - hitData.hitPosition.position).normalized;
            Invoke("ResetHitEffect", 0.2f);
        }
    }

    void ResetHitEffect() {
        hit = false;
        sprite.material = defaultMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //the collider for this is found on the texture
        if (collision.tag == "Player") {
            GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmg));
            dmgCollider.enabled = false;
            Invoke("EnableCollider", timeBetweenAttacks);
        }
    }

    void EnableCollider() {
        dmgCollider.enabled = true;
    }
}

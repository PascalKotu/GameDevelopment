using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour {
    Rigidbody2D rb = default;
    SpriteRenderer sprite = default;
    Animator animator = default;
    [SerializeField] Transform target = default;
    [SerializeField] float range = 10f;
    [SerializeField] float speed = 3f;
    [SerializeField] int dmg = 2;
    [SerializeField] float timeBetweenAttacks = 1f;
    bool inrange = false;
    [SerializeField] Collider2D dmgCollider = default;
    bool hit = true;
    [SerializeField] float knockBackRange = 500f;
    Vector2 knockBack = Vector2.zero;
    [SerializeField] Material hitMaterial = default;
    Material defaultMaterial = default;
    bool spawned = false;

    [SerializeField] List<AudioClip> spawnSounds = new List<AudioClip>();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameEvents.enemyHit.AddListener(HitEffect);
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        defaultMaterial = sprite.material;
    }

    // Update is called once per frame
    void Update() {
        if (target.position.x < transform.position.x - 0.1f) {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        } else {
            sprite.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Mathf.Abs(transform.position.x - target.position.x) <= range) {
            inrange = true;
            if (!spawned) {
                animator.SetTrigger("Spawn");
                GameEvents.PlaySound.Invoke(new AudioEventData(spawnSounds[Random.Range(0, spawnSounds.Count)], 0.5f));
                spawned = true;
            }
            
        } else {
            inrange = false;
        }
    }

    private void FixedUpdate() {
        if (hit) {
            if (knockBack != Vector2.zero) {
                rb.velocity = Vector3.zero;
                rb.AddForce(knockBack.normalized * knockBackRange);
                knockBack = Vector2.zero;
            }
        } else {
            if (inrange) {
                Vector2 targetDirection = (target.position - transform.position).normalized * speed;
                rb.velocity = new Vector2(targetDirection.x, rb.velocity.y);
                animator.SetFloat("Movement", Mathf.Abs(targetDirection.x));
            } else {
                rb.velocity = Vector3.zero;
                animator.SetFloat("Movement", 0);
            }

        }

    }


    void HitEffect(HitData hitData) {
        if (hitData.hitted == gameObject) {
            hit = true;
            sprite.material = hitMaterial;

            //get the knockback direction
            knockBack = transform.position - hitData.hitPosition.position;
            knockBack = new Vector2(knockBack.x, 0);
            Invoke("ResetHitEffect", 0.22f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithAI : MonoBehaviour {
    Rigidbody2D rb = default;
    SpriteRenderer sprite = default;
    Animator animator = default;
    [SerializeField] Transform target = default;
    [SerializeField] float range = 15f;
    [SerializeField] Material hitMaterial = default;
    Material defaultMaterial = default;
    Vector2 knockBack = Vector2.zero;
    bool hit = false;
    bool inrange = false;
    bool alive = true;
    [SerializeField] float speed = 4f;
    [SerializeField] float chargeSpeed = 8f;
    [SerializeField] float knockBackSpeed = 6f;
    [SerializeField] float timeBetweenAttacks = 2f;
    float timeSinceLastAttack = 0f;
    Vector2 direction = Vector2.zero;
    [SerializeField] GameObject greenFireBall = default;
    [SerializeField] GameObject redFireBall = default;
    [SerializeField] int dmgAttack1 = 7;
    [SerializeField] int dmgAttack2 = 5;
    [SerializeField] int dmgAttack3 = 6;


    [SerializeField] List<AudioClip> fireBall = new List<AudioClip>();

    [SerializeField] List<AudioClip> charge = new List<AudioClip>();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        GameEvents.enemyHit.AddListener(HitEffect);
        GameEvents.EnemyDead.AddListener(Death);
        defaultMaterial = sprite.material;
    }

    // Update is called once per frame
    void Update() {
        if (!hit) {
            timeSinceLastAttack += Time.deltaTime;
        }
        
        if (Mathf.Abs(target.position.x - transform.position.x) <= range && !inrange) {
            inrange = true;
            GameEvents.BossStart.Invoke();
        }

        if (inrange) {
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                float attack = Random.value;
                if((target.position - transform.position).magnitude < 5f) {
                    animator.SetTrigger("Attack3");
                    GameEvents.PlaySound.Invoke(new AudioEventData(charge[Random.Range(0, charge.Count)], 0.5f));
                } else if (Mathf.Abs(target.position.y - transform.position.y) < 0.7f) {
                    animator.SetTrigger("Attack1");
                } else {
                    animator.SetTrigger("Attack2");
                }
                
                timeSinceLastAttack = 0;
            }
        }

        

    }

    private void FixedUpdate() {
        if(inrange) {
            if (hit && alive && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Charge")) {
                rb.velocity = knockBack * knockBackSpeed;
            } else if(!alive) {
                rb.velocity = Vector3.zero;
            } else {
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                    rb.velocity = Vector3.zero;
                    
                } else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Charge")) {
                     
                    rb.velocity = direction * chargeSpeed;
                } else {
                    direction = (target.position - transform.position).normalized;
                    rb.velocity = direction * speed;
                }

            }

            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Charge")) {
                if (target.position.x >= transform.position.x) {
                    sprite.transform.localScale = new Vector3(-1, 1, 1);
                } else {
                    sprite.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            if(transform.position.y < 3f) {
                rb.MovePosition(new Vector2(transform.position.x, 3f));
            }
        }
    }

    void HitEffect(HitData hitData) {
        if (hitData.hitted == gameObject) {
            hit = true;
            sprite.material = hitMaterial;

            //get the knockback direction
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Charge")) {
                knockBack = (transform.position - hitData.hitPosition.position).normalized;
            }
            Invoke("ResetHitEffect", 0.51f);
        }
    }

    void Death(HitData hitData) {
        if (hitData.hitted == gameObject) {
            alive = false;
        }
    }

    void ResetHitEffect() {
        hit = false;
        sprite.material = defaultMaterial;
    }
    void SpawnGreenFireBall() {
        GameObject x = Instantiate(greenFireBall, transform.position, Quaternion.identity);
        x.transform.rotation = Quaternion.FromToRotation(x.transform.right, (Vector2)transform.position - new Vector2(target.position.x, transform.position.y));
        x.GetComponent<EnemyProjectile>().setDmg(dmgAttack1);

        GameEvents.PlaySound.Invoke(new AudioEventData(fireBall[Random.Range(0, fireBall.Count)], 1f));
    }

    void SpawnRedFireBall() {
        GameObject x = Instantiate(redFireBall, transform.position, Quaternion.identity);
        x.transform.rotation = Quaternion.FromToRotation(x.transform.right, (Vector2)transform.position - (Vector2)target.position); 
        x.GetComponent<EnemyProjectile>().setDmg(dmgAttack2);

        GameEvents.PlaySound.Invoke(new AudioEventData(fireBall[Random.Range(0, fireBall.Count)], 1f));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //the collider for this is found on the texture
        if (collision.tag == "Player") {
            GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmgAttack3));
        }
    }
}

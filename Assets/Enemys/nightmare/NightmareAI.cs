using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareAI : MonoBehaviour {
    Rigidbody2D rb = default;
    Animator animator = default;
    SpriteRenderer sprite = default;
    [SerializeField] Transform target = default;
    [SerializeField] float range = 15f;
    bool inRange = false;

    [SerializeField] Material hitMaterial = default;
    Material defaultMaterial = default;

    [SerializeField] int dmgMeeleP1 = 10;
    [SerializeField] int dmgMeeleP2 = 20;

    [SerializeField] int dmgRangeP1 = 8;
    [SerializeField] int dmgRangeP2 = 16;

    [SerializeField] float timeBetweenAttacks = 2f;
    float timeSinceLastAttack = 0f;

    [SerializeField] GameObject projectile = default;
    [SerializeField] float chargeStrength = 500f;
    bool phase1 = true;
    bool charges = false;

    [SerializeField] List<AudioClip> attackSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> fireSounds = new List<AudioClip>();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultMaterial = sprite.material;
        GameEvents.enemyHit.AddListener(OnHit);
        GameEvents.BossNextPhase.AddListener(GoIntoNextPhase);
    }

    void Update() {
        timeSinceLastAttack += Time.deltaTime;

        if (target.position.x < transform.position.x) {
            sprite.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }

        if (Mathf.Abs(target.position.x - transform.position.x) <= range && !inRange) {
            inRange = true;
            GameEvents.BossStart.Invoke();
        }
        

        if (inRange && (timeSinceLastAttack >= timeBetweenAttacks)) {
            Attack();
            timeSinceLastAttack = 0f;
        }

        if (transform.position.x <= 170.5f) {
            transform.position = new Vector2(170.5f, transform.position.y);
        }

    }

    private void FixedUpdate() {
        if (charges) {
            rb.AddForce((new Vector2(target.position.x, transform.position.y) - (Vector2)transform.position).normalized * chargeStrength);
            charges = false;
        } else if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            rb.velocity = Vector3.zero;
        }

    }
    void OnHit(HitData hitData) {
        if (hitData.hitted == gameObject) {
            sprite.material = hitMaterial;
            Invoke("ResetHit", 0.3f);
        }
    }

    void ResetHit() {
        sprite.material = defaultMaterial;
    }

    void GoIntoNextPhase() {
        phase1 = false;
    }

    void Attack() {
        if (phase1) {
            if (Mathf.Abs(target.position.x - transform.position.x) >= 2.5f) {
                animator.SetTrigger("Range");
            } else {
                animator.SetTrigger("Meele1");
                GameEvents.PlaySound.Invoke(new AudioEventData(attackSounds[Random.Range(0, attackSounds.Count)], 0.5f));
            }
        } else {
            if (Mathf.Abs(target.position.x - transform.position.x) >= 8f || Mathf.Abs(target.position.y - transform.position.y) >= 2f) {
                animator.SetTrigger("Range");
            } else if (Mathf.Abs(target.position.x - transform.position.x) >= 5f) {
                animator.SetTrigger("Jump");
            } else if (Mathf.Abs(target.position.x - transform.position.x) >= 2.5f) {
                animator.SetTrigger("Charge");
                GameEvents.PlaySound.Invoke(new AudioEventData(attackSounds[Random.Range(0, attackSounds.Count)], 0.5f));
                charges = true;
            } else {
                animator.SetTrigger("Meele2");
                GameEvents.PlaySound.Invoke(new AudioEventData(attackSounds[Random.Range(0, attackSounds.Count)], 0.5f));
            }
        }
    }


    void Shoot() {
        if (phase1) {
            GameObject x = Instantiate(projectile, transform.position + Vector3.down, Quaternion.identity);
            x.transform.rotation = Quaternion.FromToRotation(x.transform.right, (Vector2)transform.position - new Vector2(target.position.x, transform.position.y));
            x.GetComponent<EnemyProjectile>().setDmg(dmgRangeP1);
        } else {
            GameObject x = Instantiate(projectile, transform.position + Vector3.down, Quaternion.identity);
            x.transform.rotation = Quaternion.FromToRotation(x.transform.right, (Vector2)transform.position - (Vector2)target.position);
            x.GetComponent<EnemyProjectile>().setDmg(dmgRangeP2);
        }
        GameEvents.PlaySound.Invoke(new AudioEventData(fireSounds[Random.Range(0, fireSounds.Count)], 1f));

    }

    void Jump() {
        GameObject x = Instantiate(projectile, transform.position+Vector3.down, Quaternion.identity);
        x.transform.rotation = Quaternion.FromToRotation(x.transform.right, (Vector2)transform.position - new Vector2(target.position.x, transform.position.y));
        x.GetComponent<EnemyProjectile>().setDmg(dmgRangeP2);
        x = Instantiate(projectile, transform.position+Vector3.down, Quaternion.identity);
        x.transform.rotation = Quaternion.FromToRotation(x.transform.right, new Vector2(target.position.x, transform.position.y)-(Vector2)transform.position );
        x.GetComponent<EnemyProjectile>().setDmg(dmgRangeP2);
        GameEvents.PlaySound.Invoke(new AudioEventData(fireSounds[Random.Range(0, fireSounds.Count)], 1f));

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (phase1) {
                GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmgMeeleP1));
            } else {
                GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmgMeeleP2));
            }
        }
    }
}

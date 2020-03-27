using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAi : MonoBehaviour {
    Rigidbody2D rb = default;
    SpriteRenderer sprite = default;
    Animator animator = default;
    [SerializeField] Material hitMaterial = default;
    [SerializeField] Transform target = default;

    [SerializeField] GameObject projectile = default;
    [SerializeField] Transform projectileSpawn = default;

    Material defaultMaterial = default;


    [SerializeField] float knockBackRange = 500f;
    [SerializeField] float attackRange = 15f;
    [SerializeField] float timeBetweenAttacks = 2f;

    [SerializeField] int dmg = 3;

    [SerializeField] List<AudioClip> attack = new List<AudioClip>();
    float timeSinceLastAttack = 0f;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultMaterial = sprite.material;
        GameEvents.enemyHit.AddListener(GotHit);
        GameEvents.EnemyDead.AddListener(DeathKnockback);
    }

    // Update is called once per frame
    void Update() {
        timeSinceLastAttack += Time.deltaTime;
        if (Mathf.Abs(target.position.x - transform.position.x) <= attackRange) {
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                animator.SetTrigger("Attack");
                timeSinceLastAttack = 0;
            }
        }

        if (target.position.x >= transform.position.x) {
            sprite.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }


    }

    void SpawnFireBall() {
        GameEvents.PlaySound.Invoke(new AudioEventData(attack[Random.Range(0, attack.Count)], 1f));
        GameObject x = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        x.transform.rotation = Quaternion.FromToRotation(x.transform.right, (Vector2)transform.position - new Vector2(target.position.x, transform.position.y));
        x.GetComponent<EnemyProjectile>().setDmg(dmg);
    }


    void GotHit(HitData hitData) {
        if (hitData.hitted == gameObject) {
            //apply the hit effects
            sprite.material = hitMaterial;
            Invoke("ResetHit", 0.2f);
        }

    }

    void DeathKnockback(HitData hitData) {
        if (hitData.hitted == gameObject) {

            //get the knockback direction
            Vector2 knockBack = transform.position - hitData.hitPosition.position;
            knockBack = new Vector2(knockBack.x, 0);
            rb.velocity = Vector3.zero;
            rb.AddForce(knockBack.normalized * knockBackRange);
            knockBack = Vector2.zero;
        }
    }

    void ResetHit() {
        //revert the hit effects
        sprite.material = defaultMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //the collider for this is found on the texture
        if (collision.tag == "Player") {
            GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmg));
        }
    }
}


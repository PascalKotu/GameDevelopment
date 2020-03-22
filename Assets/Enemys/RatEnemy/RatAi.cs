using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAi : MonoBehaviour {
    //limits within which the rat moves
    [SerializeField] float maxX = 0f;
    [SerializeField] float minX = 0f;


    [SerializeField] float knockBackRange = 500f;
    Vector2 knockBack = Vector2.zero;

    [SerializeField] float speed = 5f;

    [SerializeField] int dmg = 1;

    Transform sprite = default;
    Rigidbody2D rb = default;
    Animator animator = default;
    bool hit = false;
    float direction = 1f;
    
    void Start() {
        GameEvents.enemyHit.AddListener(HitEffect);

        sprite = transform.GetChild(0);

        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }
    

    private void FixedUpdate() {
        if (hit) {
            //if rat is hit, no forward motion is applied
            //instead the rat gets knocked backed for once
            if (knockBack != Vector2.zero) {
                rb.velocity = Vector3.zero;
                rb.AddForce(knockBack.normalized * knockBackRange);
                knockBack = Vector2.zero;
            }
        } else {
            //apply forward motion
            if (transform.position.x >= maxX) {
                direction = -1f;
            } else if (transform.position.x <= minX) {
                direction = 1f;
            }
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            sprite.localScale = new Vector3(-1 * direction, 1, 1);
        }

    }

    void HitEffect(HitData hitData) {
        //is called via enemyHit-Event
        if (hitData.hitted == gameObject) {
            hit = true;

            //trigger hit animation
            animator.SetTrigger("Hit");

            //get the knockback direction
            knockBack = transform.position - hitData.hitPosition.position;
            knockBack = new Vector2(knockBack.x, 0);
        }
    }

    void ResetHitEffect() {
        //is called via animation-event
        hit = false;
        rb.velocity = new Vector2(-sprite.localScale.x, 0) * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        //the collider for this is found on the texture
        if(collision.tag == "Player") {
            GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmg));
        }
    }
}

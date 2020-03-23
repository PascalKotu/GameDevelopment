using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    [SerializeField] float speed = 15f;
    [SerializeField] PlayerStats playerStats = default;
    int dmg = 2;
    Rigidbody2D rb = default;
    void Start() {
        dmg = playerStats.rangedDamage;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(transform.localScale.x, 0) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            GameEvents.enemyHit.Invoke(new HitData(transform, collision.gameObject, dmg));
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}

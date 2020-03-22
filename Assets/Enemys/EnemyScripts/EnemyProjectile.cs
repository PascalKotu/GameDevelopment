using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    [SerializeField] float speed = 7f;
    [SerializeField] int dmg = 2;
    Rigidbody2D rb = default;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-transform.localScale.x, 0) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmg));
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void setDmg(int dmg) {
        this.dmg = dmg;
    }
}
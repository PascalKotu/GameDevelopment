using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    [SerializeField] int dmg = 1;
    [SerializeField] float timeBetweenAttacks = 1f;
    Collider2D col = default;

    void Start() {
        col = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            GameEvents.PlayerHit.Invoke(new HitData(transform, collision.gameObject, dmg));
            col.enabled = false;
            Invoke("EnableCollider", timeBetweenAttacks);
        }
    }
    void EnableCollider() {
        col.enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            col.enabled = true;
        }
    }

}

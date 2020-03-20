using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] List<AudioClip> dropSounds = new List<AudioClip>();
    [SerializeField] List<AudioClip> collectSounds = new List<AudioClip>();
    AudioSource audioSource = default;
    Rigidbody2D rb = default;
    float lastVelocity = 1f;

    int sound = 0;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sound = Random.Range(0, dropSounds.Count);
    }

    void Update() {
        //check if coin is bouncing upwards
        float currentVelocity = rb.velocity.y;
        if (lastVelocity <= 0 && currentVelocity > 0) {
            GameEvents.PlaySound.Invoke(new AudioEventData(dropSounds[sound], 1f));
        }
        lastVelocity = currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            GameEvents.PlaySound.Invoke(new AudioEventData(collectSounds[Random.Range(0, collectSounds.Count)],1f));
            Destroy(gameObject);
        }
    }
}

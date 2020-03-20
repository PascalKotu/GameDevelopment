using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSoundEffects : MonoBehaviour {
    AudioSource audioSource = default;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        GameEvents.PlaySound.AddListener(PlaySound);
    }
    

    void PlaySound(AudioEventData sound) {
        audioSource.PlayOneShot(sound.clip,sound.volume);
    }
}

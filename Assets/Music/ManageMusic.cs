using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMusic : MonoBehaviour {
    AudioSource audioSource = default;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        GameEvents.PlayMusic.AddListener(PlayMusic);
        GameEvents.ToggleMusicLoop.AddListener(ToggleLoop);
    }
    
    void ToggleLoop() {
        audioSource.loop = !audioSource.loop;
    }

    void PlayMusic(AudioEventData music) {
        audioSource.clip = music.clip;
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMusic : MonoBehaviour {
    AudioSource audioSource = default;
    [SerializeField] AudioClip start = default;
    [SerializeField] AudioClip loop = default;
    bool alreadyInLoop = false;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        GameEvents.PlayMusic.AddListener(PlayMusic);
        GameEvents.ToggleMusicLoop.AddListener(ToggleLoop);
        audioSource.clip = start;
        audioSource.Play();
    }

    private void Update() {
        if (!audioSource.isPlaying && !alreadyInLoop) {
            audioSource.clip = loop;
            audioSource.loop = true;
            audioSource.Play();
            alreadyInLoop = true;
        }
    }

    void ToggleLoop() {
        audioSource.loop = false;
    }

    void PlayMusic(AudioEventData music) {
        audioSource.clip = music.clip;
        audioSource.Play();
        alreadyInLoop = true;
    }
}

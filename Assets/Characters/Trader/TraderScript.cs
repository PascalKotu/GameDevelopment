﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderScript : MonoBehaviour
{
    [SerializeField] GameObject shop;
    [SerializeField] AudioSource effectsManager; 
    [SerializeField] AudioClip shopOpenSFX;
    GameObject speechBubble;
    bool showSpeechBubble = false, interactable = false;
    
    // Start is called before the first frame update
    void Start()
    {
        speechBubble = transform.Find("SpeechCanvas").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ToggleShop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            speechBubble.SetActive(true);
            interactable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            speechBubble.SetActive(false);
            interactable = false;
            shop.SetActive(false);
        }
    }

    void ToggleShop()
    {
        effectsManager.PlayOneShot(shopOpenSFX);
        shop.SetActive(!shop.activeSelf);
    }

}
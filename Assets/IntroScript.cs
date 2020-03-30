using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour

{
    [SerializeField] TMP_Text DialogText=default;
    [SerializeField] GameObject Canvas=default;
    [SerializeField] Animator animatorBG = default;


    [SerializeField] PlayerStats playerStats = default;
    [SerializeField] ShopStatistics shopStatistics = default;
    bool interactable = false;
    int currentText = 0;
    string[] texts = { "Brave soul", "A blood moon has risen as the abyss summons its hordes",
        "You shall walk the earth once more to defeat those nightmares\nfor else, sun may never rise again",
        "As my champion you cannot truly die, yet this will not be an easy task",
        "But hurry now, time is running short"
    };

    // Start is called before the first frame update
    void Start()
    {
        DialogText.text = texts[0];
        Canvas.SetActive(false);


        playerStats.maxHealth = 5;
        playerStats.maxMunition = 3;
        playerStats.meleeDamage = 1;
        playerStats.money = 0;
        playerStats.munition = 0;
        playerStats.rangedDamage = 2;

        shopStatistics.maxHealthUpgradeCount = 0;
        shopStatistics.maxMunitionUpgradeCount = 0;
        shopStatistics.meleeDamageUpgradeCount = 0;
        shopStatistics.munitionUpgradeCount = 0;
        shopStatistics.rangedDamageUpgradeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable && (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1")))
        {
            if (++currentText < texts.Length)
                DialogText.text = texts[currentText];
            else
            {
                Canvas.SetActive(false);
                animatorBG.SetBool("sceneChangeInit", true);
            }

        }
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
    }

}

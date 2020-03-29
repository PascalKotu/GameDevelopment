using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    [SerializeField] GameObject canvas = default;
    [SerializeField] IntroScript introScript = default;
    public void StarFadeinComplete()
    {
        introScript.SetInteractable(true);
        canvas.SetActive(true);
    }
}

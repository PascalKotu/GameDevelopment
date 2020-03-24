using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour {

    [SerializeField] int targetScene = default;
    bool canTravel = false;

    void Update() {
        if (canTravel && Input.GetAxis("Vertical") > 0) {
            SceneManager.LoadScene(targetScene);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            canTravel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            canTravel = false;
        }
    }
}

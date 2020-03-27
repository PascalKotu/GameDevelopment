using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerUnderCertainHeight : MonoBehaviour {
    [SerializeField] Transform player = default;
    [SerializeField] PlayerStats playerStats = default;
    [SerializeField] float certainHeight = -7f;
    void Start() {

    }

    
    void Update() {
        if(player.position.y <= certainHeight) {
            GameEvents.PlayerHit.Invoke(new HitData(transform, player.gameObject, playerStats.maxHealth));
        }
    }
}

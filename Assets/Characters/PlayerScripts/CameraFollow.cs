using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] float smoothing = 5f;

    void Start() {
        if (!target) {
            Debug.LogError("Assign a target to the camera");
        }
        GameEvents.CameraShake.AddListener(CamShake);
    }
    private void FixedUpdate() {
        //Camera smoothly follows the player
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }

    void CamShake(float ammount) {
        //move the position of the camera by + or - the ammount
        transform.position = new Vector3(transform.position.x + (Random.Range(0, 2) * 2 - 1) * ammount, transform.position.y + (Random.Range(0, 2) * 2 - 1) * ammount, transform.position.z);
    }
}

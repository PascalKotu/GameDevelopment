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
    }
    private void FixedUpdate() {
        //Camer smoothly follows the player
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }
}

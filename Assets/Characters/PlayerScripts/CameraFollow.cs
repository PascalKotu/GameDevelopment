using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform target = default;
    [SerializeField] float smoothing = 5f;

    [SerializeField] float maxX = 100f;
    [SerializeField] float minX = -100f;
    [SerializeField] float maxY = 100f;
    [SerializeField] float minY = -100f;

    Camera cam = default;

    void Start() {
        if (!target) {
            Debug.LogError("Assign a target to the camera");
        }
        GameEvents.CameraShake.AddListener(CamShake);
        cam = Camera.main;
    }
    

    private void FixedUpdate() {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        //Camera smoothly follows the player
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX+camWidth, maxX-camWidth), Mathf.Clamp(transform.position.y, minY+camHeight, maxY-camHeight), transform.position.z);
    }

    void CamShake(float ammount) {
        //move the position of the camera by + or - the ammount
        transform.position = new Vector3(transform.position.x + (Random.Range(0, 2) * 2 - 1) * ammount, transform.position.y + (Random.Range(0, 2) * 2 - 1) * ammount, transform.position.z);
    }
}

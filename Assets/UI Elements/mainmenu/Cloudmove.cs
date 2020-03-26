using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudmove : MonoBehaviour {
    [SerializeField] float speed = 1f;
    float length = 0;
    void Start() {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void Update() {
        transform.position +=  Vector3.left * speed * Time.deltaTime;
        if(transform.position.x + length < -36) {
            transform.position = new Vector2(36, 0);
        }
    }
}

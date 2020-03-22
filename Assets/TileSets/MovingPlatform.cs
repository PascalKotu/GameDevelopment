using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [SerializeField] float maxX = 0f;
    [SerializeField] float minX = 0f;
    [SerializeField] float speed = 5f;
    float direction = 1f;
    


    void Start() {
        
    }

    private void FixedUpdate() {
        //apply forward motion
        if (transform.position.x >= maxX) {
            direction = 1;
        } else if (transform.position.x <= minX) {
            direction = -1;
        }
        transform.position = ((Vector2)transform.position + Vector2.left * speed * direction * Time.deltaTime);


    }

    private void OnCollisionEnter2D(Collision2D collision) {
            collision.transform.parent = transform;
        
    }
    private void OnCollisionExit2D(Collision2D collision) {
            collision.transform.parent = null;
        
    }
    
}

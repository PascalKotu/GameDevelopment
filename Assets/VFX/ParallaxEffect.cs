using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour {
    float startpos = default;
    [SerializeField] float length = 0f;
    [SerializeField] float parallaxEffect = 1f;

    void Start() {
        startpos = transform.position.x;
        if(length == 0f) {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }


    void FixedUpdate() {
        float temp = (Camera.main.transform.position.x * (1 - parallaxEffect));
        float dist = (Camera.main.transform.position.x *  parallaxEffect);

        transform.position = new Vector2(startpos + dist, transform.position.y);

        if(temp > startpos + length) {
            startpos += length;
        }else if(temp < startpos - length) {
            startpos -= length;
        }
    }
}

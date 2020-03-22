using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour {
    private float startpos = default;
    private float length = default;
    [SerializeField] float parallaxEffect = 1f;
    // Start is called before the first frame update
    void Start() {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
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

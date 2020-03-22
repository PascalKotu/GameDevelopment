using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGetDown : MonoBehaviour {
    void Start() {
        GameEvents.PlatformPass.AddListener(ChangeRotation);

    }

    void ChangeRotation() {
        //set the layer of the platform to one the play does not collide with
        gameObject.layer = 2;
        Invoke("ResetRotation", 0.3f);
    }

    void ResetRotation() {
        //set the layer of the platform back to default
        gameObject.layer = 0;
    }
    
}

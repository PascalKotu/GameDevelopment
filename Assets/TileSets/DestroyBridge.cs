using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBridge : MonoBehaviour {
    int bridgeSegments = 0;
    [SerializeField] GameObject despawnEffect = default;
    [SerializeField] List<AudioClip> despawnSounds = new List<AudioClip>();

    void Start() {
        bridgeSegments = transform.childCount;
        GameEvents.BossStart.AddListener(StartDestruction);
    }
    
    void StartDestruction () {
        StartCoroutine("DestroyBridgeCoroutine");
    }

    IEnumerator DestroyBridgeCoroutine() {
        for(int i = bridgeSegments-1; i>0; i = i -2) {
            if (i >= 0) {
                Instantiate(despawnEffect, new Vector2(transform.GetChild(i).transform.position.x, transform.GetChild(i).transform.position.y - 0.5f), Quaternion.identity);
                Destroy(transform.GetChild(i).gameObject);
            }
            if (i-1 >= 0) {
                Instantiate(despawnEffect, new Vector2(transform.GetChild(i - 1).transform.position.x, transform.GetChild(i - 1).transform.position.y - 0.5f), Quaternion.identity);
                Destroy(transform.GetChild(i - 1).gameObject);
            }
            GameEvents.PlaySound.Invoke(new AudioEventData(despawnSounds[Random.Range(0, despawnSounds.Count)], 0.3f));
            
            
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
}

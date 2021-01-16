using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {
    float disappearTimer = 3f;
    Vector3 moveVector = Vector3.up;
    private float scaleFactor = 1f;
    Vector3 negetiveVector = new Vector3(-1, 1);

    private void Start() {
        Destroy(gameObject, .5f);
    }
    private void Update() {
        if (!transform) {
            return;
        }
        transform.position += moveVector * Time.deltaTime * 5f;
        moveVector -= moveVector * Time.deltaTime * 5f;

        if (disappearTimer > 1) {
            transform.localScale += Vector3.one * scaleFactor * Time.deltaTime * .1f;
        } else {
            transform.position -= negetiveVector * Time.deltaTime;
            transform.localScale -= Vector3.one * scaleFactor * Time.deltaTime * 4f;
        }
        disappearTimer -= Time.deltaTime * 8f;
    }
}
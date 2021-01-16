using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumstick : MonoBehaviour {
    public PowerUpType powerUpType;
    private PowerUpMesh pw;
    [HideInInspector] public bool isDestroyed;
    private float intervalTime;
    private float intervalTimeOrigin = 3f;
    private Vector3 meshPosition;
    private float animationSpeed = .5f;

    private void Awake() {
        pw = GetComponent<PowerUpMesh>();
    }
    private void Start() {
        pw.OnReActivate += ReActivate;
    }
    private void FixedUpdate() {
        if (isDestroyed) {
            transform.gameObject.SetActive(false);
        }
        if (intervalTime > intervalTimeOrigin / 2f) {
            meshPosition += Vector3.up * Time.fixedDeltaTime * animationSpeed;
            pw.quadSize += Vector2.up * Time.fixedDeltaTime * animationSpeed / 2;
        } else if (intervalTime <= intervalTimeOrigin / 2f && intervalTime > 0) {
            meshPosition += Vector3.down * Time.fixedDeltaTime * animationSpeed;
            pw.quadSize += Vector2.down * Time.fixedDeltaTime * animationSpeed / 2;
        } else {
            intervalTime = intervalTimeOrigin;
        }
        intervalTime -= Time.fixedDeltaTime * 3f;

        pw.UpdateQuad(0, pw.quadSize, meshPosition, 0);
    }
    private void LateUpdate() {
        pw.ImplementMeshValue();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isDestroyed) {
            isDestroyed = true;
            int randomValue = Random.Range(30, 40);
            collision.GetComponent<Health>().GainHealth(randomValue, transform.position);
        }
    }
    public void ReActivate() {
        isDestroyed = false;
        transform.gameObject.SetActive(true);
    }
}

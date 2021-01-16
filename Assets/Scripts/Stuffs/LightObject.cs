using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour {
    private Vector3 moveVelocity;

    private float wanderWaitingTime;
    private bool isWanderDone;
    private float spinWaitingTime;
    private bool isSpinDone;
    [SerializeField] private float wanderWaitingTimeMax;

    private bool isSmoothSpin;
    private Rigidbody2D rb;

    [SerializeField] private float spinDegreesMax;
    private Vector3 direction;
    private Transform endTransform;
    private Transform startTransform;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        endTransform = transform.Find("End");
        startTransform = transform.Find("Start");
        direction = (endTransform.position - startTransform.position).normalized;
    }
    private void Update() {
        wanderWaitingTime += Time.deltaTime;
        if (wanderWaitingTime >= wanderWaitingTimeMax && !isWanderDone) {
            isWanderDone = true;
            wanderWaitingTime -= wanderWaitingTimeMax;
            SmoothSpinAround(spinDegreesMax);
        } else if (wanderWaitingTime >= wanderWaitingTimeMax && isWanderDone) {
            isWanderDone = false;
            wanderWaitingTime -= wanderWaitingTimeMax;
            SmoothSpinAround(spinDegreesMax);
        }

        float spinWaitingTimeMax = 1.5f;
        spinWaitingTime += Time.deltaTime;
        if (spinWaitingTime >= spinWaitingTimeMax && !isSpinDone) {
            isSpinDone = true;
            spinWaitingTime -= spinWaitingTimeMax;
        } else if (spinWaitingTime >= spinWaitingTimeMax && isSpinDone) {
            isSpinDone = false;
            spinWaitingTime -= spinWaitingTimeMax;
        }
        float spinSpeed;
        if (!isSpinDone) {
            spinSpeed = +1;
        } else {
            spinSpeed = -1;
        }
        direction = Quaternion.Euler(0, 0, spinSpeed) * direction;
        moveVelocity = direction;
    }
    private void FixedUpdate() {
        rb.velocity = moveVelocity * 1;
    }
    private void SmoothSpinAround(float spinDegrees) {
        if (!isSmoothSpin) StartCoroutine(SmoothSpin(spinDegrees));
    }
    private IEnumerator SmoothSpin(float spinDegreesMax) {
        isSmoothSpin = true;
        int[] intArray = new int[2] { +2, -2 };
        int randomIndex = Random.Range(0, 2);
        int spinSpeed = intArray[randomIndex];
        int index = 0;
        while (index < spinDegreesMax / 2) {
            index++;
            direction = Quaternion.Euler(0, 0, spinSpeed) * direction;
            yield return null;
        }
        isSmoothSpin = false;
    }
}
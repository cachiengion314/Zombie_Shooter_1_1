using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Transform heroTarget;
    private Vector3 offSet;
    private Vector3 nextCamPos;

    public struct PointInSpace {
        public Vector3 position;
        public float Timetime;
    }
    private readonly float delay = 0.3f;
    private Queue<PointInSpace> pointInSpaces = new Queue<PointInSpace>();

    void Awake() {
        offSet = new Vector3(0, 0, -5);
    }
    private void Start() {
        heroTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update() {
        pointInSpaces.Enqueue(new PointInSpace() {
            position = heroTarget.position, Timetime = Time.time
        });
        if (pointInSpaces.Peek().Timetime <= Time.time - delay) {
            nextCamPos = pointInSpaces.Dequeue().position + offSet;
            transform.position = Vector3.Lerp(
                transform.position, nextCamPos, Time.deltaTime * 3f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombineBehaviour : MonoBehaviour {
#pragma warning disable
    [SerializeField] private float spinDegreesMax;
#pragma warning ensable

    // StunState
    private float intervalTimeStun;
    private Transform unknownPlayerTransform;
    private float unknownDamage;
    private float stunTime;
    //
    // AttactState
    internal Quaternion transformRotatinoWhenCollided;
    //
    // WanderState
    private float stuckTimeWander;
    private float wanderWaitingTime;
    private bool isWanderDone;
    private float spinWaitingTime;
    private bool isSpinDone;
    [SerializeField] private float wanderWaitingTimeMax;
    private float intervalTimeWander;
    private bool isSmoothSpin;
    //
    // Chasing
    private float intervalTimeChasing;
    //
    // Suspect
    private float stuckTimeSuspect;
    private Vector3 suspectPosition;
    public Vector3 SuspectPosition {
        get { return suspectPosition; }
        set {
            suspectPosition = value;
            Vector3 directionSuspect = suspectPosition - transform.position;
            Vector3 directionSuspectOne = directionSuspect.normalized;

            float[] floatArray = new float[4] { 90, -90, 135, -135 };
            int randomIndex = Random.Range(0, 4);
            directionSuspectOne = Quaternion.Euler(0, 0, floatArray[randomIndex]) * directionSuspectOne;

            suspectPosition += directionSuspectOne;
            Debug.DrawLine(transform.position, suspectPosition, Color.white, 5f);
        }
    }
    private float intervalTimeSuspect;
    //
    // MovingForward
    private float stuckTimeMovingForwad;
    private Quaternion rotationToTarget;
    private Vector3 movingForwardPosition;
    public Vector3 MovingForwardPosition {
        get {
            return movingForwardPosition;
        }
        set {
            movingForwardPosition = value;
            transformRotationWhenShotted = transform.rotation;
            timeRotationToTarget = 0;
            Debug.DrawLine(transform.position, movingForwardPosition, Color.blue, 5f);

            unknownPlayerTransform = GameManager.instance.playerTransform;
            unknownDamage =
                unknownPlayerTransform.GetComponent<HeroComponents>().currentGun.BulletDamage;
            unknownDamage = (100 - zh.armor) / 100 * unknownDamage;
            stunTime = unknownDamage / 40f;
            state = State.Stun;
        }
    }
    private Quaternion transformRotationWhenShotted;
    private float timeRotationToTarget;
    private float intervalTimeMovingForward;
    //
    // General
    public float defaultSpeed;
    public LayerMask layerMaskRaycast;
    public float viewRange;
    internal Vector3 direction;
    internal Transform playerTransform;
    internal State state;
    internal Rigidbody2D zombineRb;
    internal float aimAngle;
    internal Vector3 moveVelocity;
    private Transform startTransform;
    private Transform endTransform;
    private bool isCollide;
    private ZombieMelee zm;
    private Health zh;

    internal enum State {
        Wander,
        Chasing,
        MovingForward,
        Suspect,
        Attack,
        Stun,
    }
    private void Awake() {
        rotationToTarget = new Quaternion();
        zm = GetComponent<ZombieMelee>();
        zh = GetComponent<Health>();
        zombineRb = GetComponent<Rigidbody2D>();
        state = State.Wander;
        moveVelocity = Vector3.zero;
        aimAngle = 0f;
        endTransform = transform.Find("End");
        startTransform = transform.Find("Start");
        direction = (endTransform.position - startTransform.position).normalized;
    }
    private void Start() {
        EventManager.Instance.ListenTo("Health_Revive", OnRevive);
    }
    private void OnRevive(object data) {
        wanderWaitingTime = 0;
        spinWaitingTime = 0;
        intervalTimeWander = 0;
        stuckTimeWander = 0;

        intervalTimeStun = 0;
        stunTime = 0;

        intervalTimeSuspect = 0;
        stuckTimeSuspect = 0;

        intervalTimeChasing = 0;

        stuckTimeMovingForwad = 0;
        intervalTimeMovingForward = 0;

        state = State.Wander;
    }
    private void Update() {
        switch (state) {
        case State.Wander:
            // prevent stuck
            if (isCollide) {
                stuckTimeWander += Time.deltaTime;
                if (stuckTimeWander >= 3f) {
                    stuckTimeWander -= 3f;

                    movingForwardPosition = -direction * 2 + transform.position;
                    transformRotationWhenShotted = transform.rotation;
                    timeRotationToTarget = 0;
                    state = State.MovingForward;
                }
            }
            // Wandering for searching the player values
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
            // Spining around for searching the player values
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
            // Assign all required values
            direction = Quaternion.Euler(0, 0, spinSpeed) * direction;
            aimAngle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            moveVelocity = direction;
            //
            // Raycast beam up 
            intervalTimeWander += Time.deltaTime;
            if (intervalTimeWander <= .2f) break;
            intervalTimeWander -= .2f;
            RaycastHit2D hitWander;
            for (int i = 0; i < 90; i++) {
                Vector3 directionRadar = Quaternion.Euler(0, 0, -45 + i) * direction;
                hitWander = Physics2D.Raycast(transform.position, directionRadar, viewRange, layerMaskRaycast);

                if (hitWander && hitWander.transform.CompareTag("Player")) {
                    playerTransform = hitWander.transform;
                    suspectPosition = hitWander.point;
                    state = State.Chasing;
                    break;
                }
            }
            break;
        case State.Chasing:
            // Assign chasing values
            Vector3 directionToPlayer = Vector3.zero;
            if (playerTransform) {
                directionToPlayer = (playerTransform.position - transform.position).normalized;
                aimAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * 180 / Mathf.PI;
                direction = directionToPlayer; // Caution! This assign is very very important
                moveVelocity = direction;
            }
            // Beam up a single Raycast 
            intervalTimeChasing += Time.deltaTime;
            if (intervalTimeChasing <= .2f) break;
            intervalTimeChasing -= .2f;
            RaycastHit2D hitChasing = Physics2D.Raycast(transform.position, directionToPlayer, viewRange * 1.03f, layerMaskRaycast);
            Vector3 hitChasingPoint = new Vector3(hitChasing.point.x, hitChasing.point.y);
            // Assign SuspectPosition
            if (!hitChasingPoint.Equals(Vector3.zero) && hitChasing.transform.CompareTag("Player"))
                SuspectPosition = hitChasingPoint;
            //
            if (!hitChasing || !hitChasing.transform.CompareTag("Player")) {
                playerTransform = null;
                state = State.Suspect;
            }
            break;
        case State.MovingForward:
            // Assign MovingForward values
            Vector3 distanceVector;
            Vector3 distanceVectorNormalized;
            distanceVector = MovingForwardPosition - transform.position;
            distanceVectorNormalized = distanceVector.normalized;
            if (distanceVector.sqrMagnitude < .8f) {
                state = State.Wander;
            } else if (distanceVector.sqrMagnitude > .8f && isCollide) {
                stuckTimeMovingForwad += Time.deltaTime;
                if (stuckTimeMovingForwad >= 1f) {
                    stuckTimeMovingForwad -= 1f;
                    SuspectPosition = MovingForwardPosition;
                    state = State.Suspect;
                }
            }
            direction = distanceVectorNormalized;
            aimAngle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            rotationToTarget.eulerAngles = new Vector3(0, 0, aimAngle);
            moveVelocity = direction;
            //
            // Beam up Raycast each
            intervalTimeMovingForward += Time.deltaTime;
            if (intervalTimeMovingForward <= .2f) break;
            intervalTimeMovingForward -= .2f;
            RaycastHit2D hitMoving;
            for (int i = 0; i < 90; i++) {
                Vector3 directionRadar = Quaternion.Euler(0, 0, -45 + i) * direction;
                hitMoving = Physics2D.Raycast(transform.position, directionRadar, viewRange, layerMaskRaycast);

                if (hitMoving && hitMoving.transform.CompareTag("Player")) {
                    playerTransform = hitMoving.transform;
                    state = State.Chasing;
                    break;
                }
            }
            break;
        case State.Suspect:
            // Assign Suspect values
            Vector3 distanceVectorSuspect = SuspectPosition - transform.position;
            Vector3 distanceVectorSuspectOne = distanceVectorSuspect.normalized;
            direction = distanceVectorSuspectOne;
            aimAngle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            moveVelocity = direction;
            if (distanceVectorSuspect.sqrMagnitude < 1f) {
                state = State.Wander;
            } else if (distanceVectorSuspect.sqrMagnitude > 1f && isCollide) {
                stuckTimeSuspect += Time.deltaTime;
                if (stuckTimeSuspect >= 1f) { stuckTimeSuspect -= 1f; state = State.Wander; }
            }
            //
            // Beam up Raycast
            intervalTimeSuspect += Time.deltaTime;
            if (intervalTimeSuspect <= .2f) break;
            intervalTimeSuspect -= .2f;
            RaycastHit2D hitSuspect;
            for (int i = 0; i < 90; i++) {
                Vector3 directionRadar = Quaternion.Euler(0, 0, -45 + i) * direction;
                hitSuspect = Physics2D.Raycast(transform.position, directionRadar, viewRange, layerMaskRaycast);

                if (hitSuspect && hitSuspect.transform.CompareTag("Player")) {
                    playerTransform = hitSuspect.transform;
                    state = State.Chasing;
                    break;
                }
            }
            break;
        case State.Attack:
            zm.AttactMeleeUpdate();
            break;
        case State.Stun:
            aimAngle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            moveVelocity =
                -(MovingForwardPosition - transform.position).normalized * unknownDamage * .01f;

            intervalTimeStun += Time.deltaTime;
            if (intervalTimeStun <= stunTime) break;
            intervalTimeStun -= stunTime;

            state = State.MovingForward;
            break;
        }
    }
    private void FixedUpdate() {
        switch (state) {
        case State.Wander:
            zombineRb.velocity = moveVelocity * defaultSpeed;
            transform.eulerAngles = new Vector3(0, 0, aimAngle);
            break;
        case State.Chasing:
            zombineRb.velocity = moveVelocity * defaultSpeed * 1.95f;
            transform.eulerAngles = new Vector3(0, 0, aimAngle);
            break;
        case State.MovingForward:
            zombineRb.velocity = moveVelocity * defaultSpeed * 1.45f;
            transform.rotation = Quaternion.Slerp(
                transformRotationWhenShotted, rotationToTarget, timeRotationToTarget);
            timeRotationToTarget += Time.fixedDeltaTime * 2f;
            break;
        case State.Suspect:
            zombineRb.velocity = moveVelocity * defaultSpeed * 1.75f;
            transform.eulerAngles = new Vector3(0, 0, aimAngle);
            break;
        case State.Attack:
            zm.AttactMeleeFixedUpdate();
            break;
        case State.Stun:
            zombineRb.velocity = moveVelocity * defaultSpeed * 1.75f;
            transform.eulerAngles = new Vector3(0, 0, aimAngle);
            break;
        }
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
        while (index < spinDegreesMax / 2f) {
            index++;
            direction = Quaternion.Euler(0, 0, spinSpeed) * direction;
            yield return null;
        }
        isSmoothSpin = false;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player")) {
            transformRotatinoWhenCollided = transform.rotation;
            playerTransform = collision.transform;
            state = State.Attack;
        }
        isCollide = true;
    }
    private void OnCollisionExit2D(Collision2D collision) {
        isCollide = false;
    }
}
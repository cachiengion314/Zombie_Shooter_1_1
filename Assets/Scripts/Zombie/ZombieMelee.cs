using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMelee : MonoBehaviour {
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCriticalDamage;
    private float finalDamage;
    private float criticalRate;
    private bool isCritical;
    private float intensity = 7f;
    private Vector3 distanceVector;
    [SerializeField] private float attackRange;

    private Transform atkPlayerTransform;
    private Vector3 directionAttack;
    private Quaternion attackRotation;
    private bool isRotateDone;

    private float tRotatateValue;
    private ZombineBehaviour zb;

    private float intervalTime;

    private void Awake() {
        zb = GetComponent<ZombineBehaviour>();
    }

    public void AttactMeleeUpdate() {
        distanceVector = zb.playerTransform.position - zb.transform.position;
        zb.direction = distanceVector.normalized;
        zb.moveVelocity = zb.direction;

        if (tRotatateValue > 1 && !isRotateDone) {
            isRotateDone = true;
            tRotatateValue = 0;
            intensity = 7f;
            directionAttack = Quaternion.Euler(0, 0, 60) * zb.direction;
            zb.transformRotatinoWhenCollided = zb.transform.rotation;
        } else if (tRotatateValue > 1 && isRotateDone) {
            isRotateDone = false;
            tRotatateValue = 0;
            intensity = 7f;
            directionAttack = Quaternion.Euler(0, 0, -60) * zb.direction;
            zb.transformRotatinoWhenCollided = zb.transform.rotation;
            if (atkPlayerTransform)
                DealDamage(atkPlayerTransform);
        }
        zb.aimAngle = Mathf.Atan2(directionAttack.y, directionAttack.x) * 180 / Mathf.PI;
        attackRotation.eulerAngles = new Vector3(0, 0, zb.aimAngle);
        // Beam up Raycast
        intervalTime += Time.deltaTime;
        if (intervalTime <= .2f) return;
        intervalTime -= .2f;
        RaycastHit2D hitAttack = Physics2D.Raycast(
          zb.transform.position, zb.direction, attackRange, zb.layerMaskRaycast);
        if (hitAttack && hitAttack.transform.CompareTag("Player")) {
            atkPlayerTransform = hitAttack.transform;
        } else if (!hitAttack) {
            zb.state = ZombineBehaviour.State.Chasing;
            atkPlayerTransform = null;
        }
    }
    public void AttactMeleeFixedUpdate() {
        zb.zombineRb.velocity = zb.moveVelocity * zb.defaultSpeed;
        zb.transform.rotation = Quaternion.Slerp(
            zb.transformRotatinoWhenCollided, attackRotation, tRotatateValue);
        tRotatateValue += Time.fixedDeltaTime * intensity;
    }
    public void DealDamage(Transform player) {
        finalDamage = attackDamage + Random.Range(-3f, 3f);
        criticalRate = Random.Range(0f, 100f);
        if (criticalRate <= 15) {
            finalDamage = attackCriticalDamage + Random.Range(-3f, 3f);
            isCritical = true;
        } else {
            isCritical = false;
        }
        player.GetComponent<Health>().TakeDamage(finalDamage, isCritical, player.position);
    }
}
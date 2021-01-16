using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRaycast : MonoBehaviour {
    [SerializeField] private ParticleSystem smokeFxPrefab;
    private float finalDamage;
    private float criticalRate;
    private bool isCritical;

    public void ShootRaycast(Vector3 startPos, Vector3 direction, float shootRange,
        LayerMask layer, float damage, float criticalDamage, int rays) {

        finalDamage = damage + Random.Range(-3f, 3f);
        criticalRate = Random.Range(0f, 100f);
        if (criticalRate <= 15) {
            finalDamage = criticalDamage + Random.Range(-3f, 3f);
            isCritical = true;
        } else {
            isCritical = false;
        }

        RaycastHit2D hitShoot = Physics2D.Raycast(startPos, direction, shootRange, layer);

        if (!hitShoot.transform) {
            SmokeEffectWhenNotHit(startPos, direction, shootRange, finalDamage);
        } else {
            SmokeEffectWhenHit(hitShoot, finalDamage);
          
            if (hitShoot.transform.GetComponent<Health>()) {
                Health health = hitShoot.transform.GetComponent<Health>();

                if (hitShoot.transform.CompareTag("EnemiesTag")) {
                    EventManager.Instance.UpdateEvent("BulletRaycast_HitSmokeSFx", finalDamage);
                    ZombineBehaviour zombineBehaviour = hitShoot.transform.GetComponent<ZombineBehaviour>();
                    zombineBehaviour.MovingForwardPosition = new Vector3(startPos.x, startPos.y);

                    health.TakeDamage(finalDamage, isCritical, hitShoot.point);
                }
            }
        }
    }
    private void SmokeEffectWhenHit(RaycastHit2D hit, float damage) {
        ParticleSystem smoke = Instantiate(smokeFxPrefab, hit.point, Quaternion.identity);
        smoke.transform.localScale =
          new Vector3(0.5f * damage / 50f, 0.5f * damage / 50f, 0.5f * damage / 50f);
    }
    private void SmokeEffectWhenNotHit(Vector3 startPos, Vector3 direction, float range, float damage) {
        ParticleSystem smoke = Instantiate(smokeFxPrefab, startPos + (direction * range), Quaternion.identity);
        smoke.transform.localScale =
          new Vector3(0.5f * damage / 50f, 0.5f * damage / 50f, 0.5f * damage / 50f);
    }
}
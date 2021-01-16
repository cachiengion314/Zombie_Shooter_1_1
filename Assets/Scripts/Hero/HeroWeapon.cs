using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroWeapon : MonoBehaviour {
    [HideInInspector] public readonly int Max_Rays = 3;
    private HeroController hc;
    private HeroComponents he;
    private BulletRaycast br;

    private float[] floatArray;
    private int randomIndex;
    private float spinValue, shakeIntensity, intervalTime, forceOpposite;

    private void OnEnable() {
        hc = GetComponent<HeroController>();
        he = GetComponent<HeroComponents>();
        br = GetComponent<BulletRaycast>();
    }
    private void Start() {
        EventManager.Instance.ListenTo("HeroController_Shoot", OnShootingRaycast);

        EventManager.Instance.ListenTo("HeroController_PressChangingItem", BasicGunStatsUpdate);
        EventManager.Instance.ListenTo("HeroController_PressChangingItem", LaserEffect);
        EventManager.Instance.ListenTo("HeroController_PressChangingItem", BasicLight);

        EventManager.Instance.ListenTo("HeroController_AddItem", BasicLight);
        EventManager.Instance.ListenTo("HeroController_AddItem", BasicGunStatsUpdate);
        EventManager.Instance.ListenTo("HeroController_AddItem", LaserEffect);

        EventManager.Instance.ListenTo("HeroController_ThrowAwayWeapon", BasicLight);
        EventManager.Instance.ListenTo("HeroController_ThrowAwayWeapon", BasicGunStatsUpdate);
        EventManager.Instance.ListenTo("HeroController_ThrowAwayWeapon", LaserEffect);
    }
    public void BasicLight(object data) {
        he.gunFireTransform.gameObject.SetActive(false);
        he.pointLight.intensity = he.currentGun.PointLightIntensity;
        he.pointLight.range = he.currentGun.PointLightRange;
        he.pointLight.color = he.currentGun.GunLightColor;
        for (int i = 0; i < he.currentGun.NumberOfRay; i++) {
            he.lineRenderers[i].startWidth = he.currentGun.LrStarWidth;
            he.lineRenderers[i].endWidth = he.currentGun.LrEndWidth;
            Utility.SetLineColor(he.lineRenderers[i], he.currentGun.GunLightColor);
        }
    }
    public void LaserEffect(object data) {
        for (int i = 0; i < Max_Rays; i++) {
            RaycastHit2D hitLaser = Physics2D.Raycast(
              he.lineRenderersTransform[i].position,
              he.directions[i], he.currentGun.ShootRange, hc.layerMaskRaycast);
            Utility.DrawRayAndLine(
              he.lineRenderers[i],
              he.lineRenderersTransform[i].position,
              he.currentGun.ShootRange, hitLaser, he.directions[i]);
            if (i >= he.currentGun.NumberOfRay) {
                hitLaser.point = transform.position;
                Utility.DrawRayAndLine(
                  he.lineRenderers[i],
                  transform.position,
                  0, hitLaser, he.directions[i]);
            }
        }
    }
    public void ShootingEffect(Gun gun) {
        floatArray = new float[gun.FloatSpinArray.Length];

        if (!hc.isFlashOn) {
            randomIndex = UnityEngine.Random.Range(0, gun.FloatSpinArray.Length);
            spinValue = gun.FloatSpinArray[randomIndex];
            shakeIntensity = gun.ShakeIntensity;
            intervalTime = gun.IntervalTime;
            forceOpposite = gun.ForceOpposite;
        } else {
            for (int i = 0; i < gun.FloatSpinArray.Length; i++) {
                if (gun.FloatSpinArray[i] > 0)
                    floatArray[i] = gun.FloatSpinArray[i] - gun.FlashLightFactor;
                else {
                    floatArray[i] = gun.FloatSpinArray[i] + gun.FlashLightFactor;
                }
            }
            randomIndex = UnityEngine.Random.Range(0, gun.FloatSpinArray.Length);
            spinValue = floatArray[randomIndex];
            shakeIntensity =
                gun.ShakeIntensity - 1 / (4 - gun.FlashLightFactor * 0.1f * 1.7f) * gun.ShakeIntensity;
            intervalTime =
                gun.IntervalTime - 1 / (4 - gun.FlashLightFactor * 0.1f * 1.7f) * gun.IntervalTime;
            forceOpposite =
                gun.ForceOpposite - 1 / (4 - gun.FlashLightFactor * 0.1f * 1.7f) * gun.ForceOpposite;
        }
        he.Direction0 = Quaternion.Euler(0, 0, spinValue) * he.Direction0;
        he.rigidbody2DCom.AddForce(-he.Direction0 * forceOpposite, ForceMode2D.Impulse);
        Utility.ShakeCamera(shakeIntensity, intervalTime);
    }
    public void BasicGunStatsUpdate(object data) {
        hc.shootRange = he.currentGun.ShootRange;
        hc.fireRate = he.currentGun.FireRate;
        hc.slowdownFactor = he.currentGun.SlowdownFactor;
    }
    private void OnShootingRaycast(object data) {
        HeroController.OnShootRaycastEvent onShoot = (HeroController.OnShootRaycastEvent) data;
        if (onShoot.currentGun.GunType != GunType.Knife) {
            he.gunFireTransform.gameObject.SetActive(true);
            he.pointLight.intensity = 3.5f;
            he.pointLight.range = 9.7f;
            he.pointLight.color = Color.yellow;
            br.ShootRaycast(
                onShoot.lineRendererPosition, onShoot.heroDirection, onShoot.shootRange,
                onShoot.layerMaskRaycast, onShoot.currentGun.BulletDamage,
                onShoot.currentGun.BulletCriticalDamage, onShoot.currentGun.NumberOfRay);
        } else {
            he.gunFireTransform.gameObject.SetActive(false);
            he.pointLight.intensity = 1f;
            he.pointLight.range = 6.5f;
            he.pointLight.color = Color.gray;
            br.ShootRaycast(
                onShoot.lineRendererPosition, onShoot.heroDirection, onShoot.shootRange,
                onShoot.layerMaskRaycast, onShoot.currentGun.BulletDamage,
                onShoot.currentGun.BulletCriticalDamage, onShoot.currentGun.NumberOfRay);
        }
    }
}
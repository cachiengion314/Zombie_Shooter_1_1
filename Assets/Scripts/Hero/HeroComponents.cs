using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroComponents : MonoBehaviour {
    [HideInInspector] public Gun currentGun;
    [HideInInspector] public Vector3 direction0;
    [HideInInspector]
    public Vector3 Direction0 {
        get {
            directions[1] = Quaternion.Euler(
              0, 0, currentGun.RightLeftRotationArray[1]) * directions[0];
            directions[2] = Quaternion.Euler(
              0, 0, currentGun.RightLeftRotationArray[2]) * directions[0];
            return directions[0];
        }
        set {
            directions[0] = value;
            directions[1] = Quaternion.Euler(
                0, 0, currentGun.RightLeftRotationArray[1]) * directions[0];
            directions[2] = Quaternion.Euler(
                0, 0, currentGun.RightLeftRotationArray[2]) * directions[0];
        }
    }
    [HideInInspector] public Vector3 direction1;
    [HideInInspector] public Vector3 direction2;
    [HideInInspector] public Vector3[] directions;

    [HideInInspector] public Transform flashLightTransform;
    [HideInInspector] public Transform pointLightTransform;
    [HideInInspector] public Transform starGunTransform;
    [HideInInspector] public Transform endGunTransform;
    [HideInInspector] public Transform gunFireTransform;
    [HideInInspector] public Light pointLight;

    [HideInInspector] public Rigidbody2D rigidbody2DCom;
    [HideInInspector] public LineRenderer lineRendererCom0;
    [HideInInspector] public LineRenderer lineRendererCom1;
    [HideInInspector] public LineRenderer lineRendererCom2;
    [HideInInspector] public LineRenderer[] lineRenderers;
    [HideInInspector] public Transform lineRendererTransform0;
    [HideInInspector] public Transform lineRendererTransform1;
    [HideInInspector] public Transform lineRendererTransform2;
    [HideInInspector] public Transform[] lineRenderersTransform;

    private void OnEnable() {
        lineRendererTransform0 = transform.Find("Ray0").transform;
        lineRendererTransform1 = transform.Find("Ray1").transform;
        lineRendererTransform2 = transform.Find("Ray2").transform;
        rigidbody2DCom = GetComponent<Rigidbody2D>();
        lineRendererCom0 = lineRendererTransform0.GetComponent<LineRenderer>();
        lineRendererCom1 = lineRendererTransform1.GetComponent<LineRenderer>();
        lineRendererCom2 = lineRendererTransform2.GetComponent<LineRenderer>();
        lineRenderers = new LineRenderer[] {
            lineRendererCom0,
            lineRendererCom1,
            lineRendererCom2
        };
        lineRenderersTransform = new Transform[] {
            lineRendererTransform0,
            lineRendererTransform1,
            lineRendererTransform2,
        };
        starGunTransform = transform.Find("StartGun");
        endGunTransform = transform.Find("EndGun");
        flashLightTransform = transform.Find("FlashLight");
        pointLightTransform = transform.Find("PointLight");
        gunFireTransform = transform.Find("GunFire");

        gunFireTransform.gameObject.SetActive(false);
        flashLightTransform.gameObject.SetActive(false);
        pointLightTransform.gameObject.SetActive(true);
        pointLight = pointLightTransform.GetComponent<Light>();

        direction0 = (endGunTransform.position - starGunTransform.position).normalized;
        
        directions = new Vector3[] {
            direction0,
            direction1,
            direction2
        };
    }
}
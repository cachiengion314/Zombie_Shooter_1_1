using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HookUpdater : MonoBehaviour {
    public static event Action OnHookUpdater;
    public static HookUpdater instance;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    public static void Create(Action action) {
        GameObject hookUpdaterGameObject = new GameObject("HookUpdater", typeof(HookUpdater));
        OnHookUpdater += action;
        if (OnHookUpdater.GetInvocationList().Length > 11) {
            OnHookUpdater = null;
            OnHookUpdater += action;
        }
    }
    public static void Remove(Action action) {
        OnHookUpdater -= action;
    }
    private void Update() {
        OnHookUpdater?.Invoke();
    }
}
public class Utility {
    public static void DrawRayAndLine(LineRenderer lr, Vector3 position, float viewRange, RaycastHit2D hit, Vector3 direction) {
        Vector3 linePos = (viewRange * direction) + position;
        if (!hit) {
            lr.SetPosition(0, position);
            lr.SetPosition(1, linePos);
        } else {
            lr.SetPosition(0, position);
            lr.SetPosition(1, hit.point);
        }
    }
    public static void SetLineColor(LineRenderer lr, Color color) {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.7f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(.81f, 0.7f), new GradientAlphaKey(0.1f, 1.0f) }
        );
        lr.colorGradient = gradient;
    }
    public static void ShakeCamera(float shakeIntensity, float intervalTime) {
        Action newAction;

        HookUpdater.Create(newAction = new Action(() => {

            if (intervalTime <= 0) {
                return;
            }
            intervalTime -= Time.unscaledDeltaTime;

            float[] floatArray = new float[2] { +1, -1 };
            int randomIndexX = UnityEngine.Random.Range(0, 2);
            int randomIndexY = UnityEngine.Random.Range(0, 2);
            float randomX = floatArray[randomIndexX];
            float randomY = floatArray[randomIndexY];
            Vector3 randomDirection = new Vector3(randomX, randomY) * shakeIntensity;

            Camera.main.transform.position += randomDirection;

            shakeIntensity -= .07f;
        }));
    }
    public static void PlaySoundFx(AudioSource audioSource, AudioClip clip,
        float volume = .3f, bool isLoop = true, bool isOneShot = true) {

        audioSource.clip = clip;
        audioSource.volume = volume;
        if (isOneShot) {
            audioSource.PlayOneShot(clip);
            audioSource.loop = isLoop;
        } else {
            audioSource.Play();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour {
    [SerializeField] private AudioClip soundFx;
    private AudioSource audioSource;
    private Slider loadingSlider;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        loadingSlider = transform.Find("LoadingSlider").GetComponent<Slider>();
        loadingSlider.value = 0;
    }
    private void Start() {
        Utility.PlaySoundFx(audioSource, soundFx, .6f);
        if (LoadScene.OnloadSceneCallback != null) {
            LoadScene.OnloadSceneCallback.Invoke();
            LoadScene.OnloadSceneCallback = null;
        }
    }
    private void Update() {
        loadingSlider.value += Time.deltaTime * 250;
    }
}

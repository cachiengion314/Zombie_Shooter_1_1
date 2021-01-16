using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuCanvas : MonoBehaviour {
    private Button playButton;
    private Button quitButton;
    private TextMeshProUGUI titleGameText;
    [SerializeField] private AudioClip playSfx;
    [SerializeField] private AudioClip bgMusicSfx;
    private AudioSource audioSource;

    private void Awake() {
        playButton = transform.Find("PlayButton").GetComponent<Button>();
        quitButton = transform.Find("QuitButton").GetComponent<Button>();
        titleGameText = transform.Find("TitleGameText").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        Utility.PlaySoundFx(audioSource, bgMusicSfx,.7f);

        playButton.onClick.AddListener(() => {
            Utility.PlaySoundFx(audioSource, playSfx, .9f, false, false);
            Invoke("LoadSceneAction", .3f);
        });

        quitButton.onClick.AddListener(() => {
            Utility.PlaySoundFx(audioSource, playSfx, .9f, false, false);

#if UNITY_STANDALONE && !UNITY_EDITOR || UNITY_ANDROID && !UNITY_EDITOR
            PushNotificationAndroidPlugins.Instance.PushNotification();
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });
    }
    private void LoadSceneAction() {
        LoadScene.LoadSceneName(SceneName.Gameplay);
    }
    private float intervalTime = 5f;
    private void Update() {
        if (intervalTime < 0) {
            intervalTime = 5f;
        } else if (intervalTime > 5f / 2f) {
            titleGameText.fontSize += .1f;
        } else {
            titleGameText.fontSize -= .1f;
        }
        intervalTime -= Time.deltaTime * 5f;
    }
}

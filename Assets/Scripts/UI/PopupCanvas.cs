using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public struct PopupEventInfo {
    public string title;
    public string content;
    public Action okCallback;
    public Action cancelCallback;
}
public class PopupCanvas : MonoBehaviour {
    private TextMeshProUGUI reStartPopup_titleText;
    private TextMeshProUGUI reStartPopup_contentText;
    private Button reStartPopup_greenButton;
    private Button reStartPopup_redButton;
    private Action okCallback;
    private Action cancelCallback;
    private Transform reStartPopup;

    private void Awake() {
        reStartPopup = transform.Find("ReStartPopup");
        reStartPopup_titleText = reStartPopup.Find("TitleText").GetComponent<TextMeshProUGUI>();
        reStartPopup_contentText = reStartPopup.Find("ContentText").GetComponent<TextMeshProUGUI>();
        reStartPopup_greenButton = reStartPopup.Find("GreenButton").GetComponent<Button>();
        reStartPopup_redButton = reStartPopup.Find("RedButton").GetComponent<Button>();

        reStartPopup.gameObject.SetActive(false);
    }
    private void Start() {
        reStartPopup_greenButton.onClick.AddListener(() => {
            Utility.PlaySoundFx(SoundManager.instance.audioSources[0], SoundManager.instance.flashLightSfx);
            Invoke("OnClickDelay", .3f);
        });
        reStartPopup_redButton.onClick.AddListener(() => {
            Utility.PlaySoundFx(SoundManager.instance.audioSources[0], SoundManager.instance.changeItemsSfx);
            cancelCallback?.Invoke();
            reStartPopup.gameObject.SetActive(false);
        });

        EventManager.Instance.ListenTo("HeroController_WayOutEvent_EndLevel", OpenPopup);
    }
    private void OnClickDelay() {
        okCallback?.Invoke();
        reStartPopup.gameObject.SetActive(false);
    }
    public void OpenPopup(object data) {
        var info = (PopupEventInfo) data;

        reStartPopup_titleText.text = info.title;
        reStartPopup_contentText.text = info.content;
        okCallback = info.okCallback;
        cancelCallback = info.cancelCallback;

        reStartPopup.gameObject.SetActive(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TextState {
    EnableScreen, OnScreen, DisableScreen
}
public class TutorialText : MonoBehaviour {
    private bool isCollided;
    private TextMeshProUGUI tutorialText;
    private TextState textState;
    private readonly float disappearTimeOrigin = 2.4f;
    private float disappearTime;
    private Vector3 moveVector = Vector3.up;
    private float scaleFactor = 1f;
    private Vector3 negetiveVector = new Vector3(-1, 1);

    private void OnEnable() {
        tutorialText = GetComponent<TextMeshProUGUI>();
       
        moveVector += GunsMesh.GetRandomDirection();
        disappearTime = disappearTimeOrigin;
    }
    private void Start() {
        EventManager.Instance.ListenTo("Health_PlayerIsDead", OnPlayerIsDead);

        EventManager.Instance.ListenTo("CollidedEvent_EnterEvent", OnEnterText);
        EventManager.Instance.ListenTo("CollidedEvent_ExitEvent", OnExitText);

        EventManager.Instance.ListenTo("GameManager_6GemsMission", On6GemsMissionCompletedText);

        EventManager.Instance.ListenTo("WayOutEvent_DenyPlayerEnterText", OnDenyPlayerEnter);
        EventManager.Instance.ListenTo("WayOutEvent_DenyPlayerExitText", OnDenyPlayerExit);

        gameObject.SetActive(false);
    }
    private void OnPlayerIsDead(object data) {
        gameObject.SetActive(true);
        isCollided = true;
        transform.position = GameManager.instance.playerTransform.position + GunsMesh.GetRandomDirection(0);
        tutorialText.SetText("Oh no! Thats terrible! The game will auto restart to help you out!");
        tutorialText.color = Color.yellow;
        textState = TextState.EnableScreen;

        Invoke("IsCollidedFalse", 3f);
    }
    private void OnDenyPlayerExit(object data) {
        var info = (TextEventInfo) data;
        isCollided = info.isCollided;
    }
    private void OnDenyPlayerEnter(object data) {
        var info = (TextEventInfo) data;
        gameObject.SetActive(true);
        isCollided = info.isCollided;
        transform.position = info.position;
        tutorialText.SetText(info.text);
        tutorialText.color = Color.yellow;
        textState = TextState.EnableScreen;
       
        Utility.ShakeCamera(.7f, .15f);
    }
    private void On6GemsMissionCompletedText(object data) {
        var info = (TextEventInfo) data;
        gameObject.SetActive(true);
        isCollided = true;
        transform.position = info.position;
        tutorialText.SetText(info.text);
        tutorialText.color = Color.yellow;
        textState = TextState.EnableScreen;
        Invoke("IsCollidedFalse", 9f);
        Utility.ShakeCamera(1f, .3f);
    }
    private void OnExitText(object data) {
        var info = (TextEventInfo) data;
        isCollided = info.isCollided;
    }
    private void OnEnterText(object data) {
        var info = (TextEventInfo) data;
        isCollided = info.isCollided;
        if (isCollided) {
            gameObject.SetActive(true);
            isCollided = info.isCollided;
            transform.position = info.position;
            tutorialText.SetText(info.text);
            tutorialText.color = Color.white;
            textState = TextState.EnableScreen;
        }
    }
    private void OnDisable() {
        transform.localScale = Vector3.one * scaleFactor * 1f / 10f;
        textState = TextState.EnableScreen;
        disappearTime = disappearTimeOrigin;
    }
    private void FixedUpdate() {
        switch (textState) {
        case TextState.EnableScreen:
            transform.position += moveVector * Time.fixedDeltaTime * 5f;
            moveVector -= moveVector * Time.fixedDeltaTime * 5f;
            transform.localScale += Vector3.one * scaleFactor * Time.fixedDeltaTime * 5f;
            disappearTime -= Time.fixedDeltaTime * 8f;
            if (disappearTime <= disappearTimeOrigin / 2) {
                textState = TextState.OnScreen;
            }
            break;
        case TextState.OnScreen:
            if (!isCollided) {
                textState = TextState.DisableScreen;
            }
            break;
        case TextState.DisableScreen:
            transform.position -= negetiveVector * Time.fixedDeltaTime;
            transform.localScale -= Vector3.one * scaleFactor * Time.fixedDeltaTime * 4f;
            disappearTime -= Time.fixedDeltaTime * 8f;
            if (disappearTime <= 0f) {
                gameObject.SetActive(false);
            }
            break;
        }
    }
    private void IsCollidedFalse() {
        isCollided = false;
    }
}

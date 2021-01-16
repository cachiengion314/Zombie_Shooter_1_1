using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct TextEventInfo {
    public string text;
    public Vector3 position;
    public bool isCollided;
}
public class CollidedEvent : MonoBehaviour {
    [HideInInspector] public bool isCollided;
    public string eventText;

    private void Start() {
        EventManager.Instance.ListenTo("GameManager_5GemsMission", On5GemsMissionCollidedCompleted);
    }
    private void On5GemsMissionCollidedCompleted(object data) {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        isCollided = true;
        if (collision.CompareTag("Player")) {
            EventManager.Instance.UpdateEvent("CollidedEvent_EnterEvent",
                new TextEventInfo {
                    text = eventText,
                    position = GameManager.instance.playerTransform.position + GunsMesh.GetRandomDirection(),
                    isCollided = isCollided,
                });
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        isCollided = false;
        if (collision.CompareTag("Player")) {
            EventManager.Instance.UpdateEvent("CollidedEvent_ExitEvent",
                new TextEventInfo {
                    text = eventText,
                    position = GameManager.instance.playerTransform.position + GunsMesh.GetRandomDirection(),
                    isCollided = isCollided,
                });
        }
    }
}
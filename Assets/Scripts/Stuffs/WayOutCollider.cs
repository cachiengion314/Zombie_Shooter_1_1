using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayOutCollider : MonoBehaviour {
    private void Start() {
        EventManager.Instance.ListenTo("GameManager_6GemsMission", OnEndGame);
    }
    private void OnEndGame(object data) {
        gameObject.SetActive(false);
    }
}

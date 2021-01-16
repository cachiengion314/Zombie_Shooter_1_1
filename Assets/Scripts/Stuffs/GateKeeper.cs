using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour {
    private void Start() {
        EventManager.Instance.ListenTo("GameManager_5GemsMission", OnMissionCompleted);
    }
    private void OnMissionCompleted(object data) {
        gameObject.SetActive(false);
        Utility.ShakeCamera(.5f, .2f);
    }
}
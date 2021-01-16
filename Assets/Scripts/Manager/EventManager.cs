using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {
    public static EventManager Instance;

    Dictionary<string, List<Action<object>>> EventMap;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        EventMap = new Dictionary<string, List<Action<object>>>();
    }
    public void ListenTo(string eventName, Action<object> action) {
        if (!EventMap.ContainsKey(eventName)) { // <object> mean this below action only receive object type
            EventMap.Add(eventName, new List<Action<object>>());
        }
        EventMap[eventName].Add(action);
    }
    public void UnSubscribe(string eventName, Action<object> action = null, bool isNeedToClean = false) {
        if (EventMap.ContainsKey(eventName) && !isNeedToClean) {
            EventMap[eventName].Remove(action);
        } else if (EventMap.ContainsKey(eventName) && isNeedToClean) {
            EventMap[eventName].Clear();
        }
    }
    public void UpdateEvent(string eventName, object objectData) {
        if (EventMap.ContainsKey(eventName)) {
            List<Action<object>> listAction = EventMap[eventName];
            foreach (Action<object> action in listAction) {
                action(objectData);
            }
        }
    }
}
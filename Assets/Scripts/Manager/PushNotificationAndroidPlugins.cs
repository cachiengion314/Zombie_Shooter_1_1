using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotificationAndroidPlugins : MonoBehaviour {
    public static PushNotificationAndroidPlugins Instance {
        get {
            if (_instance == null) {
                _instance = new GameObject("PushNotification").AddComponent<PushNotificationAndroidPlugins>();
            }

            return _instance;
        }
    }
    private static PushNotificationAndroidPlugins _instance;

    private const string NOTIFICATION_CONTENT = "Please go back to the game. Don't leave us!";
    private const string _PLUGINS_PACKAGE_NAME = "com.company.my_pushnotification.PushNotification";
    private AndroidJavaClass _pluginClass;
    private AndroidJavaObject _pluginObject;
    private AndroidJavaClass _unityPlayer;
    private AndroidJavaObject _currentActivity;

    public AndroidJavaClass GetPluginClass() {
        if (_pluginClass == null) {
            _pluginClass = new AndroidJavaClass(_PLUGINS_PACKAGE_NAME);
            return _pluginClass;
        } else {
            return _pluginClass;
        }
    }
    public AndroidJavaObject GetPluginObject() {
        if (_pluginObject == null) {
            _pluginObject = GetPluginClass().CallStatic<AndroidJavaObject>("GetPushNotificationInstance");
            return _pluginObject;
        } else {
            return _pluginObject;
        }
    }
    private void Awake() {
        GetPluginObject();
        _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginObject.Call("createNotificationChannel", _currentActivity);
    }
    public void PushNotification() {
        _pluginObject.Call("pushNotification", _currentActivity, NOTIFICATION_CONTENT);
    }
}

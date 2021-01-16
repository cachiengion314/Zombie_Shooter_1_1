using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum SceneName {
    MainMenu, Loading, Gameplay
}
public class HookCoroutine : MonoBehaviour { }
public class LoadScene {
    public static Action OnloadSceneCallback;
    public static AsyncOperation async;

    public static void LoadSceneName(SceneName name) {
        SceneManager.LoadScene(SceneName.Loading.ToString());

        OnloadSceneCallback = () => {
            HookCoroutine hook = new GameObject("HookCoroutine").AddComponent<HookCoroutine>();
            hook.StartCoroutine(LoadSceneAsync(name));
        };
    }
    private static IEnumerator LoadSceneAsync(SceneName name) {
        yield return new WaitForSeconds(.5f);
        async = SceneManager.LoadSceneAsync(name.ToString());
        while (!async.isDone) {
            yield return null;
        }
    }
    public static float GetLoadingProgress() {
        if (async != null) {
            return async.progress;
        } else {
            return 1;
        }
    }
}

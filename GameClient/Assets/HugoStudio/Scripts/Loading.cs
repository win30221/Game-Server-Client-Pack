using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading : MonoBehaviour {

    public static string sceneName;
    public static IEnumerator loadingPreparation;

    private AsyncOperation async;

    public void Awake() {
        FadeManager.TurnLight(() => {
            StartCoroutine(loadScene());
        });
    }

    IEnumerator loadScene() {

        int displayProgress = 0;
        int toProgress = 0;
        Debug.Log("Scene load name : " + sceneName);
        yield return new WaitForEndOfFrame();
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        if (loadingPreparation != null) yield return StartCoroutine(loadingPreparation);

        while (async.progress < 0.9f) {
            toProgress = (int)async.progress * 100;
            while (displayProgress < toProgress) {
                ++displayProgress;
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress) {
            ++displayProgress;
            yield return new WaitForEndOfFrame();
        }
        
        FadeManager.TurnDark(() => {
            async.allowSceneActivation = true;
        });

    }
}
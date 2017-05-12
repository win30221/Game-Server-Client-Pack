using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour {

    public static void LoadScene(string sceneName, IEnumerator loadingPreparation) {
        Loading.sceneName = sceneName;
        Loading.loadingPreparation = loadingPreparation;
        
        FadeManager.TurnDark(() => {
            SceneManager.LoadScene("Loading");
        });
    }

    public static void LoadScene(string sceneName) {
        Loading.sceneName = sceneName;
        Loading.loadingPreparation = null;
        
        FadeManager.TurnDark(() => {
            SceneManager.LoadScene("Loading");
        });
    }

}
using UnityEngine;
using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour {

    void Awake() {
        FadeManager.TurnLight(() => { });
    }

    public void OnSubmitClick(Text name) {
        ServiceManager.instance.CreatePlayer(name.text, (JSONObject result) => {
            if (result["status"].n == Config.STATUS_OK) {
                PlayerPrefs.SetString("PlayerID", result["playerID"].str);
                PlayerPrefs.SetString("PlayerPWD", result["playerPWD"].str);
                Debug.Log(result["playerID"].str);
                GameManager.DoLogin();
            } else {
                Debug.LogError("Create player error: " + result);
            }
        });
    }
}
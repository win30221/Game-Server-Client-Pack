using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public LocalizationText name;
    public LocalizationText meteorite;

    public Button battleBtn;
    public LocalizationText btnText;

    void Start() {
        name.ReloadParameter(new string[] { GameManager.PlayerName });
        meteorite.ReloadParameter(new string[] { GameManager.PlayerMeteorites.ToString() });
        FadeManager.TurnLight(() => { });

        battleBtn.onClick.AddListener(Battle);
    }

    public void SwitchLanguage() {
        LocalizationManager.SwitchLanguage((PlayerPrefs.GetInt("Language") +1) % LocalizationManager.languageCount);
    }

    public void Battle() {
        JSONObject info = new JSONObject();
        info.AddField("level", 1);
        SocketManager.RequestBattle(info);
        battleBtn.onClick.RemoveListener(Battle);
        battleBtn.onClick.AddListener(Cancel);
        btnText.ReloadKey("Cancel");
    }

    public void Cancel() {
        SocketManager.CancelBattle();
        battleBtn.onClick.RemoveListener(Cancel);
        battleBtn.onClick.AddListener(Battle);
        btnText.ReloadKey("Battle");
    }

}
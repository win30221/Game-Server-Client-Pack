using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    public Material ship1;
    public Material ship2;

    public static BattleManager instance;

    public LocalizationText startLeft;
    public LocalizationText timeLeft;
    public LocalizationText result;
    public Button pushBtn;
    public Button exitBtn;
    public Transform spaceShip;
    private float spaceShipPosition;

    private bool isStart = false;


    private void Awake() {
        instance = this;
    }

    private void Start() {
        if (SocketManager.ISMASTER) {
            ship1.SetFloat("_IsMe", 1);
            ship2.SetFloat("_IsMe", 0);
        } else {
            ship1.SetFloat("_IsMe", 0);
            ship2.SetFloat("_IsMe", 1);
        }

        timeLeft.gameObject.SetActive(false);
        result.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);

        FadeManager.TurnLight(() => { });
        
        StartCoroutine(GetGameStatus());

        pushBtn.onClick.AddListener(OnPushClick);
        exitBtn.onClick.AddListener(OnExit);
    }

    private void Update() {
        float step = Time.deltaTime / 2;
        spaceShip.position = Vector3.MoveTowards(spaceShip.position, new Vector3(spaceShipPosition, 0, -7), step);
    }
    
    public IEnumerator GetGameStatus() {
        while (true) {
            SocketManager.GetGameStatus();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnBattleStartCountDown(float s) {
        int t = (int)Mathf.Ceil(s);
        startLeft.ReloadParameter(new string[] { t.ToString() });
    }

    public void OnBattleStart() {
        startLeft.gameObject.SetActive(false);
        timeLeft.gameObject.SetActive(true);

        isStart = true;
    }
    
    public void OnUpdateSpaceShipPosition(float s, float p) {
        int t = (int)Mathf.Ceil(s);
        timeLeft.ReloadParameter(new string[] { t.ToString() });
        spaceShipPosition = p;
    }

    public void OnBattleEnd(int winUID) {
        timeLeft.gameObject.SetActive(false);
        result.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);

        if (winUID == 2) {
            result.ReloadKey("Draw");
        } else if (SocketManager.UID == winUID) {
            result.ReloadKey("Win");
            spaceShipPosition = Mathf.Sign(spaceShipPosition) * 0.6f;
            GameManager.PlayerMeteorites++;
        } else {
            result.ReloadKey("Lose");
            spaceShipPosition = Mathf.Sign(spaceShipPosition) * 0.6f;
            GameManager.PlayerMeteorites--;
        }
        
        isStart = false;
        
    }

    public void OnPushClick() {
        if (!isStart) return;
        JSONObject inRoomMessage = new JSONObject();
        inRoomMessage.AddField("UID", SocketManager.UID);
        SocketManager.InRoomMessage(0, inRoomMessage);
    }

    public void OnExit() {
        LoadingManager.LoadScene("Menu");
    }

}

using UnityEngine;
using SocketIO;
using System;


public class SocketManager : MonoBehaviour {
    
    private static SocketIOComponent socket;

    public static int UID;
    public static bool ISMASTER { get { return UID == 0; } }
    public static bool inRoom;

    // Use this for initialization
    void Start () {
        socket = GetComponentInChildren<SocketIOComponent>();
        socket.On("OnSocketIOConnected", OnSocketIOConnected);
        socket.On("StartBattle", OnStartBattle);

        socket.On("OnBattleStartCountDown", OnBattleStartCountDown);
        socket.On("OnBattleStart", OnBattleStart);
        socket.On("OnUpdateSpaceShipPosition", OnUpdateSpaceShipPosition);
        socket.On("OnBattleEnd", OnBattleEnd);
        

    }

    public static void Connect() {
        socket.Connect();
    }
    
    #region Login Socket
    private void OnSocketIOConnected(SocketIOEvent e) {
        JSONObject loginInfo = new JSONObject();
        loginInfo.AddField("playerID", GameManager.PlayerID);
        loginInfo.AddField("playerName", GameManager.PlayerName);
        socket.Emit("Login", loginInfo, OnSocketIOLoggedInCallback);
    }

    

    private event Action<JSONObject> OnSocketIOLoggedInCallback = delegate (JSONObject msg) {
        Debug.Log("OnSocketIOLoggedIn :" + msg);
        Debug.Log("Logged in");
    };
    #endregion



    #region Battle Socket

    public static void RequestBattle(JSONObject battleInfo) {
        socket.Emit("RequestBattle", battleInfo);
    }

    public static void CancelBattle() {
        socket.Emit("CancelBattle");
    }

    private void OnStartBattle(SocketIOEvent e) {
        JSONObject data = new JSONObject(e.data.ToString());
        UID = (int)data["UID"].n;
        inRoom = true;
        LoadingManager.LoadScene("Battle");
    }




    private void OnBattleStartCountDown(SocketIOEvent e) {
        Debug.Log("OnBattleStartCountDown");

        JSONObject data = new JSONObject(e.data.ToString());
        float s = data["time"].n;
        BattleManager.instance.OnBattleStartCountDown(s);
    }

    private void OnBattleStart(SocketIOEvent e) {
        Debug.Log("OnBattleStart");
        BattleManager.instance.OnBattleStart();
    }

    private void OnUpdateSpaceShipPosition(SocketIOEvent e) {
        Debug.Log("OnUpdateSpaceShipPosition");
        JSONObject data = new JSONObject(e.data.ToString());
        float s = data["time"].n;
        float p = data["spaceShipPosition"].f;
        BattleManager.instance.OnUpdateSpaceShipPosition(s, p);
    }

    private void OnBattleEnd(SocketIOEvent e) {
        Debug.Log("OnBattleEnd");
        JSONObject data = new JSONObject(e.data.ToString());
        int winUID = (int)data["winUID"].n;
        BattleManager.instance.OnBattleEnd(winUID);
    }



    public static void InRoomMessage(int handle, JSONObject data) {
        JSONObject inRoomMessage = new JSONObject();
        inRoomMessage.AddField("handle", handle);
        inRoomMessage.AddField("data", data);
        socket.Emit("InRoomMessage", inRoomMessage);
    }

    public static void GetGameStatus() {
        socket.Emit("GetGameStatus", new JSONObject());
    }

    #endregion
}

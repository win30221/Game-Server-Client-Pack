using UnityEngine;
using System.Collections;
using SocketIO;

public class GameManager : MonoBehaviour {
    
    public static string PlayerID { set { playerID = value; } get { return playerID; } }
    private static string playerID;

    public static string PlayerName { set { playerName = value; } get { return playerName; } }
    private static string playerName;

    public static int PlayerMeteorites { set { playerMeteorites =  value; if (playerMeteorites < 0) playerMeteorites = 0; } get { return playerMeteorites; } }
    private static int playerMeteorites;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("PlayerID")) {
            DoLogin();
        } else {
            LoadingManager.LoadScene("Registration");
        }
    }

    public static void DoLogin() {
        string id = PlayerPrefs.GetString("PlayerID");
        string pwd = PlayerPrefs.GetString("PlayerPWD");
        LoadingManager.LoadScene("Menu", ServiceManager.instance.DoLogin(id, pwd, (JSONObject result) => {
            if (result["status"].n == Config.STATUS_OK) {
                playerID = result["account"]["_id"].str;
                playerName = result["account"]["playerName"].str;
                playerMeteorites = (int)result["account"]["playerMeteorites"].n;
                SocketManager.Connect();
            } else {
                Debug.LogError("Login error: " + result);
                PlayerPrefs.DeleteAll();
                LoadingManager.LoadScene("Registration");
            }
        }));
    }

}
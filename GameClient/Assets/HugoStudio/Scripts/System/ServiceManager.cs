using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class ServiceManager : MonoBehaviour {

    public static ServiceManager instance;
    private string address = Config.SERVER_IP + ":" + Config.SERVER_PORT + "/" + Config.SERVER_DOMAIN + "/";

    private void Awake() {
        instance = this;
    }

    public void CreatePlayer(string playerName, Callback.ServiceCallback callback) {
        StartCoroutine(DoCreatePlayer(playerName, callback));
    }

    IEnumerator DoCreatePlayer(string playerName, Callback.ServiceCallback callback) {
        WWWForm form = new WWWForm();
        form.AddField("playerName", playerName);
        var downloader = new DownloadHandlerBuffer();
        using (UnityWebRequest www = UnityWebRequest.Post(address + Config.CREATE_PLAYER, form)) {

            www.downloadHandler = downloader;

            yield return www.Send();

            if (www.isError) {
                Debug.Log(www.error);
            } else {
                string sJson = System.Text.Encoding.UTF8.GetString(downloader.data);
                print("Get from server : " + sJson);
                JSONObject jsonNode = new JSONObject(sJson);
                callback(jsonNode);
            }
        }
    }

    public void Login(string id, string pwd, Callback.ServiceCallback callback) {
        StartCoroutine(DoLogin(id, pwd, callback));
    }

    public IEnumerator DoLogin(string id, string pwd, Callback.ServiceCallback callback) {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("pwd", pwd);
        var downloader = new DownloadHandlerBuffer();

        using (UnityWebRequest www = UnityWebRequest.Post(address + Config.LOGIN, form)) {

            www.downloadHandler = downloader;

            yield return www.Send();

            if (www.isError) {
                Debug.Log(www.error);
            } else {
                string sJson = System.Text.Encoding.UTF8.GetString(downloader.data);
                print("Get from server : " + sJson);
                JSONObject jsonNode = new JSONObject(sJson);
                callback(jsonNode);
            }
        }
    }
}
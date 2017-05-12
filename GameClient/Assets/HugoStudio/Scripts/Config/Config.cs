public class Config {

    public static string IP = "192.168.56.1";

    //Server
    public static string SERVER_IP = "http://" + IP;
    public static string SERVER_PORT = "8080";
    public static string SERVER_DOMAIN = "GameServer";

    //Socket
    public static string SOCKET_IP = "ws://" + IP;
    public static string SOCKET_PORT = "3000";

    //Service
    public static string CREATE_PLAYER = "CreatePlayer";
    public static string LOGIN = "Login";
    public static string GET_PLAYER_INFO = "GetPlayerInfo";

    //Status
    public static int STATUS_NOT_EXIST = 404,
        /**
         * 成功
         * @constant
         */
        STATUS_OK = 1,
        /**
         * 失敗
         * @constant
         */
        STATUS_FAIL = 9001,
        /**
         * 驗證不成功
         * @constant
         */
        STATUS_NOT_AUTHORIZED = 9002,
        /**
         * 使用者不存在
         * @constant
         */
        STATUS_USER_NOT_EXISTED = 9003,
        /**
         * 使用者被鎖
         * @constant
         */
        STATUS_USER_LOCKED = 9004,
        /**
         * 密碼錯誤
         * @constant
         */
        STATUS_INCORRECT_PASSWORD = 9005;
}
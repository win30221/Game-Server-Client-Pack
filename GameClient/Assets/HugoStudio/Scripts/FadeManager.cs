using UnityEngine;

public class FadeManager : MonoBehaviour {

    private static bool isStart;
    public static float gradient;
    public static float difference;
    private static Callback.FadeCallback fadeCallback;

    public Material mat;

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, mat);
    }

    private void Update() {
        if (isStart) {
            gradient += (Time.deltaTime * GameConfig.FADE_SPEED * difference);
            if (gradient >= 1) {
                gradient = 1;
                isStart = false;
                fadeCallback();
            } else if (gradient <= 0) {
                gradient = 0;
                isStart = false;
                fadeCallback();
            }
            mat.SetFloat("gradient", gradient);
        }
    }

    public static void TurnDark(Callback.FadeCallback callback) {

        LocalizationManager.RemoveAllComponent();

        fadeCallback = callback;
        difference = -1;
        isStart = true;
    }

    public static void TurnLight(Callback.FadeCallback callback) {
        fadeCallback = callback;
        difference = 1;
        isStart = true;
    }
}
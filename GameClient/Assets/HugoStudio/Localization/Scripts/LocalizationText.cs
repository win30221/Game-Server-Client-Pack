using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationText : MonoBehaviour, ILocalizationComponent {
    
    public string[] parameters;
	private Text text = null;
	private int index;

	public void Awake () {
		text = GetComponent<Text> ();
        text.font = LocalizationManager.GetFont();
        index = LocalizationManager.GetIndexByKey(text.name);
        LocalizationManager.AddComponent(this);
        UpdateContent();
    }

    public void UpdateContent() {
        try {
            if (index >= 0) {
                text.text = string.Format(LocalizationManager.GetTextByIndex(index), parameters);
            } else {
                Debug.LogError("You don't fill up the correct ID for \"" + gameObject.name + "\"");
                if (transform.parent) Debug.LogError("Its parent: " + transform.parent.name);
            }
        } catch {
            Debug.LogError(gameObject.name + " fills text wrong.");
        }
    }
    
    public void ReloadKey(string key) {
        text.name = key;
        index = LocalizationManager.GetIndexByKey( key );
        UpdateContent();
    }

    public void ReloadKey(string key, string[] parm) {
        parameters = parm;
        InterpreterParameter();
        ReloadKey(key);
    }

    public void ReloadParameter(string[] parm) {
        parameters = parm;
        InterpreterParameter();
        UpdateContent();
    }

    private void InterpreterParameter() {
        for (int i = 0; i < parameters.Length; i++) {
            parameters[i] = LocalizationManager.Interpreter(parameters[i]);
        }
    }
}
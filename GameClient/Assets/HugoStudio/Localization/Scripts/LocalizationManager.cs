using UnityEngine;
using System.Collections.Generic;

public class LocalizationManager : MonoBehaviour {
    
	public static string[][] language;
	public static int languageCount;
	public static Font font;

    public static List<ILocalizationComponent> allComponent = new List<ILocalizationComponent>();

    public static void AddComponent(ILocalizationComponent c) {
        allComponent.Add(c);
    }

    public static void RemoveAllComponent() {
        allComponent = new List<ILocalizationComponent>();
    }

    private void Awake() {
        DontDestroyOnLoad(this);
        //Read language data from "Language"
        TextAsset binAsset = Resources.Load ("Language", typeof(TextAsset)) as TextAsset;         
		string [] lineArray = binAsset.text.Split ("\n"[0]);
		language = new string [lineArray.Length][];
		for (int i = 0; i < lineArray.Length; i++) {  
			language[i] = lineArray[i].Split(',');
		}

		//Set how many language its have.
		languageCount = language[0].Length - 1;

		//Fill up font you need.
		font = Resources.Load( "Font/TTF/Language", typeof(Font) ) as Font;

	}

    public static string Interpreter(string str) {
        str = str.Replace("\\n", "\n");
        str = str.Replace("\\comma", ",");
        return str;
    }
    
    public static string GetTextByKey(string key) {
		return GetTextByIndex( GetIndexByKey(key) );
	}
	
	public static string GetTextByIndex(int index) {
		string tmp = language [ index ] [ PlayerPrefs.GetInt( "Language" )+1 ];
		return Interpreter(tmp);
	}
	
	public static int GetIndexByKey(string key) {
		int tmp = -1;
        for (int i = 0; i < language.Length; i++) {
			if (language[i][0].Trim() == key.Trim()) {
				tmp = i;
			}
		}
		return tmp;
	}

	public static Font GetFont() {
		return font;
	}

    public static void SwitchLanguage(int index) {
        PlayerPrefs.SetInt("Language", index);
        UpdateComponent();
    }

    private static void UpdateComponent() {
        for (int i = 0; i < allComponent.Count; i++) {
            allComponent[i].UpdateContent();
        }
    }
}
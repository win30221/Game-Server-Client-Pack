using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocalizationComponent {

    void UpdateContent();
    void ReloadKey(string key);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAskedToggle : MonoBehaviour {
    public Text title;
    public Text label;
    public FloatObject modifiedField;
    public Toggle tog;

    public string onLabel;
    public string offLabel;

    public void Init(FloatObject modifFloat, bool interactible, string title, string onLabel, string offLabel,
        bool toggleIsOn) {
        modifiedField = modifFloat;
        this.title.text = title;
        this.onLabel = onLabel;
        this.offLabel = offLabel;
        tog.interactable = interactible;

        tog.isOn = toggleIsOn;
    }

    public void OnToggle() {
        if (tog.isOn)
            modifiedField.value = 1;
        else
            modifiedField.value = 0;
    }
}

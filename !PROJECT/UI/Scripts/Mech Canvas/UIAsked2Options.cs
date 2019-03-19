using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAsked2Options : MonoBehaviour {
    public Text title;
    public Text option1Value;
    public Text option2Value;
    public FloatObject modifiedField;
    public Toggle tog1;
    public Toggle tog2;

    public float op1;
    public float op2;

    public void Init(FloatObject modifFloat, bool interactible, string title, string option1, string option2,
        float op1, float op2, int onToggleIndex) {
        modifiedField = modifFloat;
        this.title.text = title;
        option1Value.text = option1;
        option2Value.text = option2;
        this.op1 = op1;
        this.op2 = op2;
        tog1.interactable = interactible;
        tog2.interactable = interactible;
        if (onToggleIndex == 1)
            tog1.isOn = true;
        else
            tog2.isOn = true;
    }

    public void SelectOption1() {
        modifiedField.value = op1;
    }
    public void SelectOption2() {
        modifiedField.value = op2;
    }

}

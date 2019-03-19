using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechanicalUI : MonoBehaviour {
    [HideInInspector]
    public MechanicalPart currentMech;
    public TMP_Text title;
    public TMP_Text itemDescription;

    [Header("DeletionButton")]
    public Material selectionMaterial;
    public Color deletionColor;
    public Button deletionButton;

    private bool interactible;

    public Transform FieldAlignGroup;
    [Header("Prefabs for Asked Fields")]
    public UIAsked2Options uiAsked2Options;
    public UIAskedToggle uiAskedToggle;


    public void Init(MechanicalPart parent, bool interact) {
        currentMech = parent;

        title.text = currentMech.mechName;
        itemDescription.text = currentMech.description;
        this.interactible = interact;

        if (interact) {  // daca suntem in creation mode
            deletionButton.gameObject.SetActive(true);
        }

        UpdateText();
    }

    private void Update() {
        UpdateText();
    }

    public void UpdateText() {

    }

    public void Ask4Field2Options(FloatObject modifFloat, string title, string option1, string option2,
        float op1, float op2, int onToggleIndex) {
        UIAsked2Options spawn = Instantiate(uiAsked2Options.gameObject, FieldAlignGroup).GetComponent<UIAsked2Options>();
        spawn.Init(modifFloat, interactible, title, option1, option2, op1, op2, onToggleIndex);
    }

    public void Ask4FieldToggle(FloatObject modifFloat, string title, string onLabel, string offLabel,
        bool toggleIsOn) {
        UIAskedToggle spawn = Instantiate(uiAskedToggle.gameObject, FieldAlignGroup).GetComponent<UIAskedToggle>();
        if (!spawn) {
            Debug.Log("asdf");
        }
        spawn.Init(modifFloat, interactible, title, onLabel, offLabel, toggleIsOn);
    }
}

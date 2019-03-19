using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UIButtonController : MonoBehaviour {
    private Button button;
    public TMP_Text textOnButton;

    private UnityAction savedButtonAction;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void SetAndStartCurrentAction(UnityAction action) {
        button.onClick.AddListener(action);
        savedButtonAction = action;
    }

    public void SetCurrentAction(UnityAction action) {
        savedButtonAction = action;
    }

    public void PauseListener() {
        button.onClick.RemoveAllListeners();
    }
    public void ResumeSavedListener() {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(savedButtonAction);
    }

}

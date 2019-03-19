using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalBodyHolder : MonoBehaviour {
    public const float bodyHolderSize = 1f;
    public enum InputOrOutput { Output, Input };
    public InputOrOutput usedFor;

    public SpriteRenderer holderRenderer;
    private bool wasActive;
    private bool isActive;

    private void Awake() {
        SetLightOff();
    }

    private void SetLightOn() {
        holderRenderer.material.SetFloat("_LightOn", 1);
    }
    private void SetLightOff() {
        holderRenderer.material.SetFloat("_LightOn", 0);
    }

    private void Update() {
        if (wasActive && !isActive) {
            SetLightOff();
        }
        if (!wasActive && isActive) {
            SetLightOn();
        }
        wasActive = isActive;
    }

    public void SetActiveSignal(bool active) {
        isActive = active;
    }
}

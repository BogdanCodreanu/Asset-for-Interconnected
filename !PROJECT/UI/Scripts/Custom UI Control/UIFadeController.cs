using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFadeController : MonoBehaviour {

    private MaskableGraphic[] allGraphics = null;
    private float[] initialAlphas;

    private void Awake() {
        if (allGraphics == null) {
            allGraphics = GetComponentsInChildren<MaskableGraphic>();
            initialAlphas = new float[allGraphics.Length];
            for (int i = 0; i < initialAlphas.Length; i++) {
                initialAlphas[i] = allGraphics[i].color.a;
            }
        }
    }

    public void SetToZeroAll() {
        if (allGraphics == null)
            Awake();
        for (int i = 0; i < allGraphics.Length; i++) {
            allGraphics[i].SetAlpha(0);
        }
    }

    public void Appear(float duration) {
        if (allGraphics == null)
            Awake();
        for (int i = 0; i < allGraphics.Length; i++) {
            allGraphics[i].StopAllCoroutines();
            allGraphics[i].Fade(initialAlphas[i], duration);
        }
    }

    public void Disappear(float duration) {
        if (allGraphics == null)
            Awake();
        for (int i = 0; i < allGraphics.Length; i++) {
            allGraphics[i].StopAllCoroutines();
            allGraphics[i].Fade(0, duration);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour {

    public Slider loadingSlider;
    public TMP_Text pressAnyKeyText;

    public void SetLoadingSlider(float clamped01) {
        loadingSlider.value = clamped01;
    }

    public void PressAnyKeyTurnOn() {
        pressAnyKeyText.gameObject.SetActive(true);
    }
}

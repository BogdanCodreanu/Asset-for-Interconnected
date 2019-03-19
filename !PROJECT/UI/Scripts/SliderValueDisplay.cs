using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueDisplay : MonoBehaviour {
    private Slider slider;
    private TMP_Text text;

    private void Awake() {
        slider = transform.parent.GetComponent<Slider>();
        text = GetComponent<TMP_Text>();
    }

    void Update () {
        text.text = slider.value.ToString();
	}
}

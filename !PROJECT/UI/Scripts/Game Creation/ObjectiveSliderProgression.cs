using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSliderProgression : MonoBehaviour {

    private Slider slider;
    public AnimationCurve fillingObjectiveSliderCurve;
    private Coroutine objectiveProgressFilling;
    private float currentSliderValue;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public void SetObjectiveSliderSmooth(float value) {
        if (value != currentSliderValue) {
            if (objectiveProgressFilling != null)
                StopCoroutine(objectiveProgressFilling);
            objectiveProgressFilling = StartCoroutine(MoveSliderTo(value));
            currentSliderValue = value;
        }
    }
    public void SetObjectiveSliderDirect(float value) {
        if (value != currentSliderValue) {
            slider.value = value;
            currentSliderValue = value;
        }
    }

    private IEnumerator MoveSliderTo(float value) {
        float initialValue = slider.value;
        float lerper = 0;
        float startTime = Time.time;

        while (lerper < 1) {
            lerper = (Time.time - startTime) / 1f;
            slider.value = Mathf.Lerp(initialValue, value, fillingObjectiveSliderCurve.Evaluate(lerper));
            yield return new WaitForEndOfFrame();
        }

        slider.value = value;
        objectiveProgressFilling = null;
    }
}

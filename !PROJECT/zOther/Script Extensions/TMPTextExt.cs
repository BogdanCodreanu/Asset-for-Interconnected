using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class TMPTextExt {
    public static void Fade(this TMP_Text text, float from, float to, float duration) {
        text.StartCoroutine(FadeTextFromTo(text, from, to, duration));
    }
    private static IEnumerator FadeTextFromTo(TMP_Text text, float from, float to, float duration) {
        float startTime = Time.time;
        float lerper = 0;
        text.color = new Color(text.color.r, text.color.g, text.color.b, from);
        while (lerper < 1) {
            lerper = (Time.time - startTime) / duration;
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(from, to, lerper));
            yield return new WaitForEndOfFrame();
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, to);
    }
}

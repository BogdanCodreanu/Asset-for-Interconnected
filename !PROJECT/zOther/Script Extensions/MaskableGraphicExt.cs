﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MaskableGraphicExt {

    public static Coroutine Fade(this MaskableGraphic img, float from, float to, float duration, MonoBehaviour workingMono = null) {
        if (img.gameObject.activeInHierarchy) {
            if (workingMono)
                return workingMono.StartCoroutine(FadeTextFromTo(img, from, to, duration));
            return img.StartCoroutine(FadeTextFromTo(img, from, to, duration));
        }
        return null;
    }

    public static Coroutine Fade(this MaskableGraphic grf, float to, float duration, MonoBehaviour workingMono = null) {
        if (grf.gameObject.activeInHierarchy) {
            if (workingMono)
                return workingMono.StartCoroutine(FadeTextFromTo(grf, grf.color.a, to, duration));
            return grf.StartCoroutine(FadeTextFromTo(grf, grf.color.a, to, duration));
        }
        return null;
    }

    private static IEnumerator FadeTextFromTo(MaskableGraphic img, float from, float to, float duration) {
        float startTime = Time.time;
        float lerper = 0;
        img.color = new Color(img.color.r, img.color.g, img.color.b, from);
        while (lerper < 1) {
            lerper = (Time.time - startTime) / duration;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(from, to, lerper));
            yield return new WaitForEndOfFrame();
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, to);
    }

    public static void SetAlpha(this MaskableGraphic grf, float alpha) {
        grf.color = new Color(grf.color.r, grf.color.g, grf.color.b, alpha);
    }
}

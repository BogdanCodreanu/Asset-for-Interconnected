using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightExt {

    public static void FadeAndDie(this Light lite, MonoBehaviour mono, float deathDuration, bool fadeIntensity = true, bool fadeRadius = false) {
        float initialInt = lite.intensity;
        float initialRad = lite.range;
        mono.StartCoroutine(LightFading(lite, mono, deathDuration, fadeIntensity, fadeRadius, initialInt, initialRad, true));
    }

    public static void FadeAndDie(this Light lite, MonoBehaviour mono, float deathDuration, AnimationCurve animCurve, bool fadeIntensity = true, bool fadeRadius = false) {
        float initialInt = lite.intensity;
        float initialRad = lite.range;
        mono.StartCoroutine(LightFading(lite, mono, deathDuration, animCurve, fadeIntensity, fadeRadius, initialInt, initialRad, true));
    }

    private static IEnumerator LightFading(Light lite, MonoBehaviour mono, float deathDuration, AnimationCurve animCurve, bool fadeIntensity, bool fadeRadius, float initialInt, float initialRad, bool destroyObject) {
        float startTime = Time.time;

        while (Time.time - startTime <= deathDuration) {
            if (fadeIntensity)
                lite.intensity = initialInt * animCurve.Evaluate((Time.time - startTime) / deathDuration);
            if (fadeRadius)
                lite.range = initialRad * animCurve.Evaluate((Time.time - startTime) / deathDuration);
            yield return new WaitForEndOfFrame();
        }
        if (destroyObject)
            Object.Destroy(lite.gameObject);
    }

    private static IEnumerator LightFading(Light lite, MonoBehaviour mono, float deathDuration, bool fadeIntensity, bool fadeRadius, float initialInt, float initialRad, bool destroyObject) {
        float startTime = Time.time;

        while (Time.time - startTime <= deathDuration) {
            if (fadeIntensity)
                lite.intensity = initialInt * (1 - ((Time.time - startTime) / deathDuration));
            if (fadeRadius)
                lite.range = initialRad * (1 - ((Time.time - startTime) / deathDuration));
            yield return new WaitForEndOfFrame();
        }
        if (fadeIntensity)
            lite.intensity = 0;
        if (fadeRadius)
            lite.range = 0;

        if (destroyObject)
            Object.Destroy(lite.gameObject);
    }


    public static void FadeAndKeepAlive(this Light lite, MonoBehaviour mono, float deathDuration, bool fadeIntensity = true, bool fadeRadius = false) {
        float initialInt = lite.intensity;
        float initialRad = lite.range;
        mono.StartCoroutine(LightFading(lite, mono, deathDuration, fadeIntensity, fadeRadius, initialInt, initialRad, false));
    }

    public static void FadeAndKeepAlive(this Light lite, MonoBehaviour mono, float deathDuration, AnimationCurve animCurve, bool fadeIntensity = true, bool fadeRadius = false) {
        float initialInt = lite.intensity;
        float initialRad = lite.range;
        mono.StartCoroutine(LightFading(lite, mono, deathDuration, animCurve, fadeIntensity, fadeRadius, initialInt, initialRad, false));
    }

    public static void Appear(this Light lite, MonoBehaviour mono, float appearingTime, bool fadedIntensity = true, float futureIntensity = 1, bool fadedRadius = false, float futureRadius = 0) {
        float initialIntensity, initialRadius;
        initialIntensity = lite.intensity;
        initialRadius = lite.range;
        mono.StartCoroutine(AppearLight(lite, mono, appearingTime, fadedIntensity, futureIntensity, fadedRadius, futureRadius, initialIntensity, initialRadius));
    }

    private static IEnumerator AppearLight(Light lite, MonoBehaviour mono, float appearingTime, bool fadedIntensity, float futureIntensity, bool fadedRadius, float futureRadius, float initialInt, float initialRadius) {
        float startTime = Time.time;

        while (Time.time - startTime <= appearingTime) {
            if (fadedIntensity)
                lite.intensity = Mathf.Lerp(initialInt, futureIntensity, (Time.time - startTime) / appearingTime);
            if (fadedRadius)
                lite.range = Mathf.Lerp(initialRadius, futureRadius, (Time.time - startTime) / appearingTime);
            yield return new WaitForEndOfFrame();
        }
        if (fadedIntensity)
            lite.intensity = futureIntensity;
        if (fadedRadius)
            lite.range = futureRadius;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoUIInformationalText : MonoBehaviour {

    public TMP_Text theText;

    private const float fadeDuration = .5f;
    private const float upwardsSpeed = 25f;

    private float shownDuration;
    private float initialAlpha;

    //
    // Summary:
    //     ///
    //     Disabling this lets you skip the GUI layout phase.
    //     ///
    public void Init(string text, float timeOnScreen = 2f) {
        theText.text = text;
        shownDuration = timeOnScreen;
        initialAlpha = theText.alpha;
        StartCoroutine(MoveUpwards());
        StartCoroutine(Fade());
    }

    private IEnumerator MoveUpwards() {
        while (gameObject) {
            transform.position += new Vector3(0, upwardsSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Fade() {
        yield return new WaitForSeconds(shownDuration);
        float startTime = Time.time;
        float lerper = 0;
        while (lerper < 1) {
            lerper = (Time.time - startTime) / fadeDuration;
            theText.color = new Color(theText.color.r, theText.color.g, theText.color.b, Mathf.Lerp(initialAlpha, 0, lerper));
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private void OnDestroy() {
        
    }
}

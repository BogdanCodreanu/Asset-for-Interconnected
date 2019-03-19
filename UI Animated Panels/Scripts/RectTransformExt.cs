using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Razziel.AnimatedPanels {
    public static class RectTransformExt {
        public static Coroutine SlideStretchVertical(this RectTransform rectTransf, MonoBehaviour mono, float from, float to, float duration) {
            return mono.StartCoroutine(StretchRectTransVertical(rectTransf, from, to, duration));
        }

        private static IEnumerator StretchRectTransVertical(RectTransform rectTransf, float from, float to, float duration) {
            rectTransf.sizeDelta = new Vector2(rectTransf.sizeDelta.x, from);
            float startTime = Time.time;
            float lerper = 0;

            while (lerper < 1f) {
                lerper = (Time.time - startTime) / duration;
                rectTransf.sizeDelta = new Vector2(rectTransf.sizeDelta.x, Mathf.Lerp(from, to, lerper));

                yield return new WaitForEndOfFrame();
            }
            rectTransf.sizeDelta = new Vector2(rectTransf.sizeDelta.x, to);
        }

        public static Coroutine SlideStretchHorizontal(this RectTransform rectTransf, MonoBehaviour mono, float from, float to, float duration) {
            return mono.StartCoroutine(StretchRectTransHorizontal(rectTransf, from, to, duration));
        }

        private static IEnumerator StretchRectTransHorizontal(RectTransform rectTransf, float from, float to, float duration) {
            rectTransf.sizeDelta = new Vector2(from, rectTransf.sizeDelta.y);
            float startTime = Time.time;
            float lerper = 0;

            while (lerper < 1f) {
                lerper = (Time.time - startTime) / duration;
                rectTransf.sizeDelta = new Vector2(Mathf.Lerp(from, to, lerper), rectTransf.sizeDelta.y);

                yield return new WaitForEndOfFrame();
            }
            rectTransf.sizeDelta = new Vector2(to, rectTransf.sizeDelta.y);
        }
    }
}
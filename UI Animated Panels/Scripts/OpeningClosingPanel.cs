using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Razziel.AnimatedPanels {
    public class OpeningClosingPanel {
        private const float procentImpartireTimpLaAppearingPanel = .4f;

        private MonoBehaviour coroutinesMono;
        private float animationTime;

        private RectTransform slidingTrans;
        private Vector2 slidingTransInitialDelta;

        private MaskableGraphic[] allGraphics;
        private float[] allGraphicsInitials;

        private MaskableGraphic slidingGraf;
        private float slidingGrafInitial;

        private Action actionAfterFade;
        private Action actionAfterAppear;
        private Action actionBeforeFade;
        private Action actionBeforeAppear;

        public enum OpeningPanelMode { Vertical, Horizontal, FadeOnly };
        public OpeningPanelMode openingMode;

        public class AdditionalFadingElement {
            public AdditionalFadingElement(MaskableGraphic element, float percentOfAnimationTimeFade) {
                this.element = element;
                this.percentOfAnimationTimeFade = percentOfAnimationTimeFade;
            }
            public MaskableGraphic element;
            public float percentOfAnimationTimeFade;
            public float initialAlpha;
        }

        public bool opened;
        private List<AdditionalFadingElement> additionalFadingElements;

        private float initialAnimationTime;

        public OpeningClosingPanel(RectTransform slidingTransform, MonoBehaviour monoUsedToStoppAllCoroutines, float time, OpeningPanelMode mode, GameObject fadingContent = null, bool initialOpened = false) {

            coroutinesMono = monoUsedToStoppAllCoroutines;
            animationTime = time;
            slidingTrans = slidingTransform;
            openingMode = mode;

            slidingTransInitialDelta = slidingTrans.sizeDelta;

            if (fadingContent)
                allGraphics = fadingContent.GetComponentsInChildren<MaskableGraphic>();
            else
                allGraphics = slidingTrans.GetComponentsInChildren<MaskableGraphic>();

            allGraphicsInitials = new float[allGraphics.Length];
            for (int i = 0; i < allGraphicsInitials.Length; i++) {
                allGraphicsInitials[i] = allGraphics[i].color.a;
            }
            slidingGraf = slidingTransform.GetComponent<MaskableGraphic>();
            if (slidingGraf)
                slidingGrafInitial = slidingGraf.color.a;
            else
                slidingGrafInitial = 0;
            initialAnimationTime = -1;
            opened = initialOpened;
        }

        /// <summary>
        /// Override default animation time.
        /// </summary>
        public void NewAnimationTime(float newAnimTime) {
            animationTime = newAnimTime;
        }

        public void SetActionBeforeFade(Action actionBeforeFade) {
            this.actionBeforeFade = actionBeforeFade;
        }
        public void SetActionAfterFade(Action actionAfterFade) {
            this.actionAfterFade = actionAfterFade;
        }

        public void SetActionBeforeAppear(Action actionBeforeAppear) {
            this.actionBeforeAppear = actionBeforeAppear;
        }
        public void SetActionAfterAppear(Action actionAfterAppear) {
            this.actionAfterAppear = actionAfterAppear;
        }

        private Action animatedPanelAfterFade;
        private Action animatedPanelBeforeAppear;

        /// <summary>
        /// Don't use this method. It will override the data created by the panel group.
        /// </summary>
        /// <param name="actionAfterFade"></param>
        public void AnimatedPanelAfterFade(Action actionAfterFade) {
            this.animatedPanelAfterFade = actionAfterFade;
        }
        /// <summary>
        /// Don't use this method. It will override the data created by the panel group.
        /// </summary>
        /// <param name="actionAfterFade"></param>
        public void AnimatedPanelBeforeAppear(Action actionBeforeAppear) {
            this.animatedPanelBeforeAppear = actionBeforeAppear;
        }

        public void AddAditionalFadingElement(AdditionalFadingElement newElement) {
            if (additionalFadingElements == null) {
                additionalFadingElements = new List<AdditionalFadingElement>();
            }
            newElement.initialAlpha = newElement.element.color.a;
            additionalFadingElements.Add(newElement);
        }


        public void AppearFromZero() {
            coroutinesMono.StopAllCoroutines();

            coroutinesMono.StartCoroutine(AppearPanel());
        }

        public void FadeToZero() {
            coroutinesMono.StopAllCoroutines();

            coroutinesMono.StartCoroutine(FadePanel());
        }

        public void Toggle() {
            if (opened)
                FadeToZero();
            else
                AppearFromZero();
        }

        /// <summary>
        /// Next animation will be executed in a custom time duration.
        /// </summary>
        /// <param name="customAnimationTime">Next animation time</param>
        public void NextFadeWithCustomAnimationTime(float customAnimationTime) {
            initialAnimationTime = animationTime;
            animationTime = customAnimationTime;
        }
        private void ResetInitialAnimationTime() {
            if (initialAnimationTime != -1) {
                animationTime = initialAnimationTime;
                initialAnimationTime = -1;
            }
        }

        private IEnumerator AppearPanel() {
            opened = true;
            if (actionBeforeAppear != null)
                actionBeforeAppear();
            if (animatedPanelBeforeAppear != null) {
                animatedPanelBeforeAppear();
            }

            foreach (MaskableGraphic graphic in allGraphics) {
                graphic.SetAlpha(0);
            }
            if (additionalFadingElements != null)
                foreach (AdditionalFadingElement element in additionalFadingElements) {
                    element.element.Fade(0, element.initialAlpha, element.percentOfAnimationTimeFade * animationTime, coroutinesMono);
                }

            if (slidingGraf) {
                slidingGraf.Fade(0, slidingGrafInitial, animationTime * procentImpartireTimpLaAppearingPanel, coroutinesMono);
            }
            if (openingMode != OpeningPanelMode.FadeOnly) {
                if (openingMode == OpeningPanelMode.Vertical) {
                    slidingTrans.SlideStretchVertical(coroutinesMono, 0, slidingTransInitialDelta.y, animationTime * procentImpartireTimpLaAppearingPanel);
                }
                else {
                    slidingTrans.SlideStretchHorizontal(coroutinesMono, 0, slidingTransInitialDelta.x, animationTime * procentImpartireTimpLaAppearingPanel);
                }
            }

            yield return new WaitForSeconds(animationTime * procentImpartireTimpLaAppearingPanel);

            for (int i = 0; i < allGraphics.Length; i++) {
                if (allGraphics[i] != slidingGraf) {
                    allGraphics[i].Fade(allGraphicsInitials[i], animationTime * (1 - procentImpartireTimpLaAppearingPanel), coroutinesMono);
                }
            }

            yield return new WaitForSeconds(animationTime * (1 - procentImpartireTimpLaAppearingPanel));

            if (actionAfterAppear != null)
                actionAfterAppear();

            ResetInitialAnimationTime();
        }

        private IEnumerator FadePanel() {
            if (actionBeforeFade != null)
                actionBeforeFade();

            for (int i = 0; i < allGraphics.Length; i++) {
                if (allGraphics[i] != slidingGraf) {
                    allGraphics[i].Fade(0, animationTime * (procentImpartireTimpLaAppearingPanel), coroutinesMono);
                }
            }
            if (additionalFadingElements != null)
                foreach (AdditionalFadingElement element in additionalFadingElements) {
                    element.element.Fade(0, element.percentOfAnimationTimeFade * animationTime, coroutinesMono);
                }

            yield return new WaitForSeconds(animationTime * (procentImpartireTimpLaAppearingPanel));

            foreach (MaskableGraphic graphic in allGraphics) {
                if (graphic != slidingGraf)
                    graphic.SetAlpha(0);
            }

            if (slidingGraf)
                slidingGraf.Fade(0, animationTime * (1 - procentImpartireTimpLaAppearingPanel), coroutinesMono);

            if (openingMode != OpeningPanelMode.FadeOnly) {
                if (openingMode == OpeningPanelMode.Vertical)
                    slidingTrans.SlideStretchVertical(coroutinesMono, slidingTrans.sizeDelta.y, 0, animationTime * (1 - procentImpartireTimpLaAppearingPanel));
                else
                    slidingTrans.SlideStretchHorizontal(coroutinesMono, slidingTrans.sizeDelta.x, 0, animationTime * (1 - procentImpartireTimpLaAppearingPanel));
            }

            yield return new WaitForSeconds(animationTime * (1 - procentImpartireTimpLaAppearingPanel));

            if (actionAfterFade != null)
                actionAfterFade();
            if (animatedPanelAfterFade != null) {
                animatedPanelAfterFade();
            }
            opened = false;
            ResetInitialAnimationTime();
        }
    }
}
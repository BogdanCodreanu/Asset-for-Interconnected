using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Razziel.AnimatedPanels {
    public class UIAnimatedPanelControllerNode : MonoBehaviour {
        private UIAnimatedPanel panel;
        /// <summary>
        /// This is the Controller of the animation. Use this only to apply actions before/after the fade/apparence.
        /// </summary>
        [HideInInspector] public OpeningClosingPanel openingClosingPanel;
        private float fadeAwayTime;
        private Coroutine turningOff;

        public UIAnimatedPanelControllerNode parent;
        public List<UIAnimatedPanelControllerNode> children;
        public bool currentlyOpened;
        public bool willFadeAway;
        public bool currentlyOpendInGroup;  // used by group
        private MonobehaviourEmpty monoForOpeningClosingPanel;

        private UIAnimatedPanelGroup groupToOpenAfterClose;

        /// <summary>
        /// Function called On initialization of OpeningClosingPanel.
        /// </summary>
        public void Init(UIAnimatedPanel panel, UIAnimatedPanel.AnimationTimePanelDetail appear, UIAnimatedPanel.AnimationTimePanelDetail fade, OpeningClosingPanel.OpeningPanelMode openingMode,
            GameObject interiorContent, bool initialHide, List<MaskableGraphic> additionalFadingElements) {
            this.panel = panel;
            RectTransform rectTrans = panel.GetComponent<RectTransform>();
            monoForOpeningClosingPanel = new GameObject("OpeningClosingPanel Mono for " + panel.name).AddComponent<MonobehaviourEmpty>();
            monoForOpeningClosingPanel.gameObject.hideFlags = HideFlags.HideInHierarchy;

            openingClosingPanel = new OpeningClosingPanel(rectTrans, monoForOpeningClosingPanel, appear.customTime, openingMode, interiorContent, !initialHide);
            if (additionalFadingElements != null) {
                foreach (MaskableGraphic mg in additionalFadingElements) {
                    openingClosingPanel.AddAditionalFadingElement(new OpeningClosingPanel.AdditionalFadingElement(mg, 1));
                }
            }

            openingClosingPanel.AnimatedPanelAfterFade(delegate {
                panel.gameObject.SetActive(false);
                if (additionalFadingElements != null) {
                    foreach (MaskableGraphic mg in additionalFadingElements) {
                        mg.gameObject.SetActive(false);
                    }
                }
            });
            openingClosingPanel.AnimatedPanelBeforeAppear(delegate {
                panel.gameObject.SetActive(true);
                if (additionalFadingElements != null) {
                    foreach (MaskableGraphic mg in additionalFadingElements) {
                        mg.gameObject.SetActive(true);
                    }
                }
            });
            turningOff = null;
            currentlyOpened = !initialHide;
            currentlyOpendInGroup = !initialHide;
            fadeAwayTime = fade.customTime;
        }

        /// <summary>
        /// Function called On initialization of OpeningClosingPanel. Creates tree relations between Nodes.
        /// </summary>
        public void AssignFamilyTree() {
            children = new List<UIAnimatedPanelControllerNode>();
            UIAnimatedPanel auxPanel;
            for (int i = 0; i < panel.transform.childCount; i++) {
                auxPanel = panel.transform.GetChild(i).GetComponent<UIAnimatedPanel>();
                if (auxPanel) {
                    children.Add(auxPanel.controller);

                    auxPanel.controller.SetParent(this);
                }
            }
        }

        public void SetParent(UIAnimatedPanelControllerNode newParent) {
            parent = newParent;
        }

        /// <summary>
        /// Set a new time for animation.
        /// </summary>
        public void SetAnimationTime(float animationTime) {
            openingClosingPanel.NewAnimationTime(animationTime);
        }
        public void SetFadeTime(float fadeTime) {
            fadeAwayTime = fadeTime;
        }

        public void Toggle() {
            bool canToggle = true;
            if (turningOff != null) {
                canToggle = false;
            }
            if (canToggle) {
                if (SearchUpwardsForFadeAway()) {
                    canToggle = false;
                }
            }
            if (canToggle) {
                if (openingClosingPanel.opened) {
                    TurnOffRecursive();
                    willFadeAway = true;
                    currentlyOpendInGroup = false;
                }
                else {
                    openingClosingPanel.AppearFromZero();
                    currentlyOpened = true;
                    currentlyOpendInGroup = true;
                }
            }
        }

        private void LateUpdate() {
            if (!panel)
                Destroy(gameObject);
        }

        public bool SearchUpwardsForFadeAway() {
            if (willFadeAway)
                return true;
            if (parent == null) {
                return willFadeAway;
            }
            return parent.SearchUpwardsForFadeAway();
        }

        public Coroutine TurnOffRecursive() {
            if (turningOff == null)
                turningOff = StartCoroutine(TurnOffAllPanels());
            return turningOff;
        }

        public void SetPanelNeededToBeOpendAfterThisCloses(UIAnimatedPanelGroup willOpenInGroup) {
            groupToOpenAfterClose = willOpenInGroup;
        }

        private IEnumerator TurnOffAllPanels() {
            List<Coroutine> waiters = new List<Coroutine>();
            currentlyOpendInGroup = false;
            foreach (UIAnimatedPanelControllerNode node in children) {
                if (node.currentlyOpened)
                    waiters.Add(node.TurnOffRecursive());
            }

            foreach (Coroutine co in waiters) {
                yield return co;
            }
            waiters.Clear();

            openingClosingPanel.NextFadeWithCustomAnimationTime(fadeAwayTime);
            openingClosingPanel.FadeToZero();
            yield return new WaitForSeconds(fadeAwayTime);

            turningOff = null;
            currentlyOpened = false;
            willFadeAway = false;

            if (groupToOpenAfterClose) {
                if (groupToOpenAfterClose.WillBeLastOpened) {
                    groupToOpenAfterClose.WillBeLastOpened.Toggle();
                    groupToOpenAfterClose.WillBeLastOpened = null;
                    groupToOpenAfterClose = null;
                }
            }
        }
        private void OnDestroy() {
            Destroy(gameObject);
            Destroy(monoForOpeningClosingPanel);
        }
    }
}
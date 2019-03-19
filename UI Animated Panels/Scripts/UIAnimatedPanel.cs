using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Razziel.AnimatedPanels {
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Animated Panel")]
    public class UIAnimatedPanel : MonoBehaviour {
        public bool root;
        public GameObject content;
        public OpeningClosingPanel.OpeningPanelMode openingMode;
        private UnityAction togglingPanelAction;

        public bool grabAnimationFromRoot;
        public AnimationTimePanelDetail appearTime;
        public bool customFadeAwayDuration;
        public AnimationTimePanelDetail fadeTime;

        [Serializable]
        public struct AnimationTimePanelDetail {
            public bool grabFromRoot;
            public float customTime;
        }

        public List<Button> toggledByButtons;
        [HideInInspector] public UIAnimatedPanelControllerNode controller;
        public bool initialHide = true;

        public List<MaskableGraphic> additionalFadingElements;
        private bool buttonsAssignedByGroup;

        private bool awakeDone = false;

        private void Awake() {
            Initialize();
        }

        /// <summary>
        /// Initialize panel via script. Also initializes all panels sub-children.
        /// </summary>
        public void Initialize() {
            if (!awakeDone) {
                FirstInitialization();
                awakeDone = true;
            }
        }

        private void FirstInitialization() {
            if (root) {
                UIAnimatedPanelGroup[] groups = GetComponentsInChildren<UIAnimatedPanelGroup>(true);
                foreach (UIAnimatedPanelGroup group in groups)
                    group.Awake();

                UIAnimatedPanel[] childrenPanels = GetComponentsInChildren<UIAnimatedPanel>(true);
                foreach (UIAnimatedPanel panel in childrenPanels) {
                    panel.Init();
                }
                foreach (UIAnimatedPanel panel in childrenPanels) {
                    panel.controller.AssignFamilyTree();

                    if (!panel.root) {
                        if (panel.appearTime.grabFromRoot) {
                            panel.controller.SetAnimationTime(appearTime.customTime);
                        }
                        if (panel.fadeTime.grabFromRoot) {
                            panel.controller.SetFadeTime(fadeTime.customTime);
                        }
                    }
                }
            }
        }

        private void Init() {
            GameObject controllerObj = new GameObject("UI Panel Controller for " + name) {
                hideFlags = HideFlags.HideInHierarchy
            };
            if (!root)
                initialHide = true;

            if (!customFadeAwayDuration) {
                fadeTime = appearTime;
            }
            controller = controllerObj.AddComponent<UIAnimatedPanelControllerNode>();
            controller.Init(this, appearTime, fadeTime, openingMode, content, initialHide, additionalFadingElements);

            if (!buttonsAssignedByGroup)
                togglingPanelAction = delegate { controller.Toggle(); };

            foreach (Button but in toggledByButtons) {
                but.onClick.AddListener(togglingPanelAction);
            }


            if (initialHide) {
                gameObject.SetActive(false);
                if (additionalFadingElements != null) {
                    foreach (MaskableGraphic mg in additionalFadingElements) {
                        mg.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void AssignGroupPanel(UIAnimatedPanelGroup group) {
            if (group.openOnlyOnClosedAll) {
                togglingPanelAction = delegate { group.CheckForOpeningOnlyPanel(this); };
            }
            else {
                togglingPanelAction = delegate { group.CheckForOpeningOnlyPanel(this); controller.Toggle(); };
            }
            buttonsAssignedByGroup = true;
        }


        public UnityAction GetActionThatTogglesPanel() {
            return togglingPanelAction;
        }

        public void Toggle() {
            togglingPanelAction();
        }
        public void Toggle(bool value) {
            if ((value && !controller.currentlyOpendInGroup) || (!value && controller.currentlyOpendInGroup)) {
                togglingPanelAction();
            }
        }

        private void OnDestroy() {
            if (controller) {
                Destroy(controller);
            }
        }
    }
}
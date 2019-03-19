using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Razziel.AnimatedPanels {
    [AddComponentMenu("UI/Animated Panel Group")]
    public class UIAnimatedPanelGroup : MonoBehaviour {
        public List<UIAnimatedPanel> panelsInGroup;
        [Tooltip("The clicked panel opens after the other active panel has finished closing")]
        public bool openOnlyOnClosedAll;
        [HideInInspector] public UIAnimatedPanelControllerNode WillBeLastOpened;
        private bool awakeDone = false;

        public void Awake() {
            if (!awakeDone) {
                awakeDone = true;
                foreach (UIAnimatedPanel panel in panelsInGroup) {
                    panel.AssignGroupPanel(this);
                }
            }
        }

        private bool somethingTurnedOff;
        public void CheckForOpeningOnlyPanel(UIAnimatedPanel needToOpen) {
            if (!openOnlyOnClosedAll) {
                foreach (UIAnimatedPanel panel in panelsInGroup) {
                    if (panel != needToOpen) {
                        if (panel.controller.currentlyOpendInGroup) {
                            panel.controller.Toggle();
                        }
                    }
                }
            }
            else {
                somethingTurnedOff = false;
                WillBeLastOpened = needToOpen.controller;
                foreach (UIAnimatedPanel panel in panelsInGroup) {
                    if (panel != needToOpen) {
                        if (panel.controller.currentlyOpendInGroup) {
                            panel.controller.TurnOffRecursive();
                            panel.controller.currentlyOpendInGroup = false;
                            panel.controller.SetPanelNeededToBeOpendAfterThisCloses(this);
                            somethingTurnedOff = true;
                        }
                    }
                }
                if (!somethingTurnedOff) {
                    needToOpen.controller.Toggle();
                    WillBeLastOpened = null;
                }
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Razziel.AnimatedPanels {
    public class UIAnimatedPanelHiddenInitializer : MonoBehaviour {
        public UIAnimatedPanel[] rootPanelsToInitialize;

        private void Awake() {
            foreach (UIAnimatedPanel panel in rootPanelsToInitialize)
                panel.Initialize();
        }
    }
}
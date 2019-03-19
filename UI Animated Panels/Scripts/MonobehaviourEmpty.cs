using UnityEngine;

namespace Razziel.AnimatedPanels {
    public class MonobehaviourEmpty : MonoBehaviour {
        private void OnDestroy() {
            Destroy(gameObject);
        }
    }
}

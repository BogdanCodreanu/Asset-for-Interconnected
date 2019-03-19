using UnityEngine;

public class MonobehaviouEmpty : MonoBehaviour {
    private void OnDestroy() {
        Destroy(gameObject);
    }
}

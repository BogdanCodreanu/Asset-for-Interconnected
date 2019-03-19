using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberedTransform {

    private Vector3 position;
    private Transform parent;
    private Vector3 scale;
    private Quaternion rotation;

    public void Remember(Transform remembered) {
        parent = remembered.parent;
        remembered.parent = null;
        position = remembered.position;
        scale = remembered.lossyScale;
        rotation = remembered.rotation;
    }

    public void Assign(Transform changed, MonoBehaviour mono, float duration) {
        mono.StartCoroutine(MoveTowardsNicely(changed, position, duration));
        changed.position = position;
        changed.localScale = scale;
        changed.rotation = rotation;
    }

    public void AssignParents(Transform changed) {
        changed.parent = parent;
    }

    private static IEnumerator MoveTowardsNicely(Transform mover, Vector3 towards, float duration) {
        float startTime = Time.time;
        Vector3 initialPos = mover.position;
        while (Time.time - startTime <= duration) {
            if (!mover) {
                break;
            }

            mover.transform.position = Vector3.Lerp(initialPos, towards, (Time.time - startTime) / duration);
            yield return new WaitForEndOfFrame();
        }

        if (mover)
            mover.position = towards;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExt {
    public static Coroutine MoveToDuring(this Transform trans, MonoBehaviour mono, Vector3 destination, float duration) {
        return mono.StartCoroutine(MovingObj(trans, destination, duration));
    }

    private static IEnumerator MovingObj(Transform trans, Vector3 dest, float duration) {
        float startTime = Time.time;
        Vector3 initialPos = trans.position;
        float lerper = 0f;

        while (lerper < 1) {
            lerper = (Time.time - startTime) / duration;
            trans.position = Vector3.Lerp(initialPos, dest, lerper);
            yield return new WaitForEndOfFrame();
        }
        trans.position = dest;
    }

    public static int GetChildDepth(this Transform trans, Transform relativeTo) {
        if (trans.parent == relativeTo) {
            return 1;
        }
        if (trans.parent == null) {
            Debug.LogError("Calculated a transform that is not a child of relativeTo");
            return 10000;
        }

        return 1 + GetChildDepth(trans.parent, relativeTo);
    }
}

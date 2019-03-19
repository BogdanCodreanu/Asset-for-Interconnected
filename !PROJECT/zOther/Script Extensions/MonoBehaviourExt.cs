using System.Collections;
using System;
using UnityEngine;

public static class MonoBehaviourExt {

    public static void ExecuteFunctionWithDelay(this MonoBehaviour mono, float delay, Action function) {
        if (delay != 0)
            mono.StartCoroutine(Cooldown(delay, function));
        else
            function();
    }

    private static IEnumerator Cooldown(float delay, Action function) {
        yield return new WaitForSeconds(delay);
        function();
    }
}

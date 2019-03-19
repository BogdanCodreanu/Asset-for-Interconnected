using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingSpawnDie : MonoBehaviour {
    private Vector3 initialScale;
    private bool scaling;
    private Vector3 futureScale;
    private float counter;
    private float scalingDuration;
    private bool dying;
    
	void Awake () {
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
	}

    public void ScaleTo(float duration) {
        ScaleTo(initialScale, duration);
    }
    public void ScaleTo(Vector3 scale, float duration) {
        scaling = true;
        futureScale = scale;
        counter = 0;
        scalingDuration = duration;
    }

    public void Die(float duration) {
        dying = true;
        scalingDuration = duration;
        initialScale = transform.localScale;
        counter = 0f;
    }

    private void Update() {
        if (dying) {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, counter / scalingDuration);
            if (counter >= scalingDuration)
                Destroy(gameObject);
        }
        else {
            if (scaling) {
                counter += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, futureScale, counter / scalingDuration);
                if (counter >= scalingDuration)
                    scaling = false;
            }
        }
    }
}

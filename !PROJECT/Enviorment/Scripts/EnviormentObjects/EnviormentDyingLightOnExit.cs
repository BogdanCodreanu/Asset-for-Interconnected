using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentDyingLightOnExit : EnviormentObject {
    private Light lite;
    private float initialIntensity;

    public override void Start2() {
        lite = GetComponent<Light>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform == GameController.mainCube.transform) {
            KillUnit();
        }
    }

    public override void StopEffects() {
        initialIntensity = lite.intensity;
        lite.FadeAndKeepAlive(this, 1f);
    }
    public override void StartEffects() {
        lite.Appear(this, .5f, true, initialIntensity, false, 0);
    }
}

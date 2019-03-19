using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAndLightControl : MonoBehaviour {
    private ParticleSystem ps;
    private Light lite;

    private void Awake() {
        ps = GetComponent<ParticleSystem>();
        lite = transform.GetChild(0).GetComponent<Light>();
    }

    public void KillBoth(float particleDuration, float liteDuration) {
        LightFlicker liteFlicker = lite.GetComponent<LightFlicker>();
        if (liteFlicker)
            liteFlicker.enabled = false;
        
        ps.StopEmmittingAndDie(particleDuration + liteDuration);
        lite.FadeAndDie(this, liteDuration);
    }
}

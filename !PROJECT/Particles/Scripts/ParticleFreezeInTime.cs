using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFreezeInTime : MonoBehaviour {
    public ParticleSystem[] stoppedParticles;
    public float pausedAt = 1f;
	
	void Start () {
        foreach (ParticleSystem ps in stoppedParticles) {
            ps.WorkingRandomSeed();
            StartCoroutine(StopParticle(ps, pausedAt));
        }
	}


    private static IEnumerator StopParticle(ParticleSystem ps, float time) {
        var mn = ps.main;
        float startSec = Time.time;

        while (Time.time - startSec < time) {
            mn.simulationSpeed = 1 - ((Time.time - startSec) / time);
            yield return new WaitForEndOfFrame();
        }
        mn.simulationSpeed = 0;
    }
}

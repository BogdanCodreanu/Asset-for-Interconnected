using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleSystemExt {
    public static void StopEmmitting(this ParticleSystem ps) {
        var em = ps.emission;
        em.enabled = false;
        
    }

    public static void StartEmmitting(this ParticleSystem ps) {
        var em = ps.emission;
        em.enabled = true;
    }

    public static void StopEmmittingAndDie(this ParticleSystem ps, float timeUntilDeath) {
        var em = ps.emission;
        em.enabled = false;
        Object.Destroy(ps.gameObject, timeUntilDeath);
    }

    public static void WorkingRandomSeed(this ParticleSystem ps) {
        ps.Clear();
        ps.Stop();
        ps.randomSeed = (uint)Random.Range(0, 5000);
        ps.Simulate(0, true, true);
        ps.Play();
    }
}

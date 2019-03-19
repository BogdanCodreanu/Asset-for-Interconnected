using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRandomAndKillOnStop : MonoBehaviour {
    public ParticleSystem[] allExplosions;
    private bool allNotPlaying;

    private void Start() {
        for (int i = 0; i < allExplosions.Length; i++) {
            allExplosions[i].WorkingRandomSeed();
        }
    }

    private void Update() {
        allNotPlaying = true;
        for (int i = 0; i < allExplosions.Length; i++) {
            if (allExplosions[i].isPlaying)
                allNotPlaying = false;
        }
        if (allNotPlaying)
            Destroy(gameObject);
    }
}

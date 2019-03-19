using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFloatingObj : UnitControl {
    public GameObject particleSpawnedOnDeath;

    public override void OnKill() {
        if (particleSpawnedOnDeath)
            Instantiate(particleSpawnedOnDeath, transform.position, Quaternion.identity);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        KillUnit();
    }
}

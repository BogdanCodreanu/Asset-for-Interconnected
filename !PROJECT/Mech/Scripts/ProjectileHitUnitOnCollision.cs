using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitUnitOnCollision : MonoBehaviour {
    public bool instaDeath;

    private void OnCollisionEnter2D(Collision2D collision) {
        UnitControl hitEnemy = collision.gameObject.GetComponent<UnitControl>();
        if (hitEnemy) {
            if (instaDeath)
                hitEnemy.KillUnit();
        }
    }
}

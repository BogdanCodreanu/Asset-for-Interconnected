using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelfAndChildrenPSOnLostVelocity : MonoBehaviour {
    public Rigidbody2D rbWithVelocity;
    public float velocityLimit;
    public float destroyGameobjectTimeAfterDeath = 2f;
    public bool dieOnFirstCollision;

    private Collider2D coll;

    [HideInInspector] public bool dead;

    private void Awake() {
        coll = GetComponent<Collider2D>();
    }

    void Update () {
        if (rbWithVelocity.velocity.magnitude <= velocityLimit) {
            Kill();
        }
    }

    private void Kill() {
        if (!dead) {
            dead = true;
            rbWithVelocity.StopAllForces();
            ParticleSystem[] allPs = rbWithVelocity.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in allPs) {
                ps.StopEmmittingAndDie(destroyGameobjectTimeAfterDeath);
            }
            Light[] allLights = rbWithVelocity.gameObject.GetComponentsInChildren<Light>();
            foreach (Light lite in allLights) {
                lite.FadeAndDie(this, .5f);
            }
            Destroy(gameObject, destroyGameobjectTimeAfterDeath);
            coll.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (dieOnFirstCollision)
            Kill();
    }
}

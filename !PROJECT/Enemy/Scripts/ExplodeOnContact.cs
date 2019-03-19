using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnContact : MonoBehaviour {

    public float explodeRadius = 3f;
    public float explosionForce = 1f;
    public LayerMask machinaLayer;
    public GameObject bombEffect;
    public float bombEffectDuration;
    public float contactSensitivity = 7f;

    private UnitControl unit;

    private void Start() {
        unit = GetComponent<UnitControl>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.CompareTag("Mechanical Part")) {
            Explode();
        }
        if (collision.relativeVelocity.magnitude > contactSensitivity) {
            Explode();
        }
    }

    public void Explode() {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explodeRadius, machinaLayer);
        MechanicalPart mechKilled;

        foreach (Collider2D col in cols) {
            mechKilled = col.GetComponent<MechanicalPart>();
            if ((mechKilled.handleTransform.position - transform.position).magnitude <= explodeRadius) {

                mechKilled.KillPart();

                mechKilled.ownRb.AddForceAtPosition((mechKilled.handleTransform.position - transform.position).normalized * explosionForce, transform.position, ForceMode2D.Impulse);
            } else {
                mechKilled.ownRb.AddForceAtPosition((mechKilled.transform.position - transform.position).normalized * explosionForce, transform.position, ForceMode2D.Impulse);
            }
        }
        Destroy(Instantiate(bombEffect, transform.position, Quaternion.identity), bombEffectDuration);
        unit.KillUnit();
    }
}

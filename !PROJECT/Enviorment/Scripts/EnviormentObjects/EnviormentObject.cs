using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentObject : MonoBehaviour {

    [HideInInspector] public Collider2D ownCollider;
    [HideInInspector] public Rigidbody2D ownRb;

    private bool enabledState;
    private bool alive;

    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private Vector3 savedLocalScale;

    private void Awake() {
        ownCollider = GetComponent<Collider2D>();
        ownRb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Start2();
    }

    public virtual void Start2() { }

    public void SetEnabledState(bool value) {
        enabledState = value;
        if (value) {
            alive = true;
            SaveLifeProperties();
        }
        if (!value) {
            Respawn();
            AssignLifeProperties();
        }
        EnabledState(value);
    }

    public void KillUnit() {
        if (alive && enabledState) {
            if (ownCollider) {
                ownCollider.enabled = false;
            }

            alive = false;
            StopEffects();
        }
    }

    public virtual void StopEffects() { }
    public virtual void StartEffects() { }
    public virtual void EnabledState(bool value) { }

    private void Respawn() {
        if (!alive) {
            StopAllCoroutines();
            if (ownCollider) {
                ownCollider.enabled = true;
            }
            alive = true;
            StartEffects();
        }
    }


    public virtual void SaveLifeProperties() {
        savedPosition = transform.localPosition;
        savedRotation = transform.localRotation;
        savedLocalScale = transform.localScale;
    }
    public virtual void AssignLifeProperties() {
        transform.localPosition = savedPosition;
        transform.localRotation = savedRotation;
        transform.localScale = savedLocalScale;
    }
}

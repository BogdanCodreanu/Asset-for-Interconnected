using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitControl : MonoBehaviour {

    public string unitName;
    public bool alwaysKinematic;

    [HideInInspector] public Collider2D ownCollider;
    [HideInInspector] public Rigidbody2D ownRb;
    private SpriteRenderer spriteRend;

    private bool enabledState;
    [HideInInspector] public bool alive;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    [HideInInspector] public bool canExecuteBehaviour;
    private BehaviourController behaviourController;

    private void Awake() {
        behaviourController = GetComponent<BehaviourController>();
        ownCollider = GetComponent<Collider2D>();
        ownRb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        if (GameController.GetGameState() == GameController.GameState.CREATING && !ownRb.isKinematic) {
            ownRb.isKinematic = true;
        }
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
            canExecuteBehaviour = true;
        }
        if (!value) {
            canExecuteBehaviour = false;
            RespawnUnit();
            AssignLifeProperties();
        }
        if (!alwaysKinematic) {
            ownRb.isKinematic = !value;
            ownRb.StopAllForces();
        }
        if (behaviourController) {
            behaviourController.SetEnabledState(value);
            behaviourController.StopBehaviourCoroutines();
        }
    }

    public void KillUnit() {
        if (alive && enabledState) {
            spriteRend.enabled = false;
            ownCollider.enabled = false;
            if (!alwaysKinematic) {
                ownRb.isKinematic = true;
                ownRb.StopAllForces();
            }
            alive = false;
            canExecuteBehaviour = false;
            StopPassiveEffects();
            OnKill();
        }
    }

    public virtual void StopPassiveEffects() { }
    public virtual void StartPassiveEffects() { }

    private void RespawnUnit() {
        if (!alive) {
            spriteRend.enabled = true;
            ownCollider.enabled = true;
            alive = true;
            StartPassiveEffects();
        }
    }

    public virtual void OnKill() { }


    public virtual void SaveLifeProperties() {
        savedPosition = transform.localPosition;
        savedRotation = transform.localRotation;
    }
    public virtual void AssignLifeProperties() {
        transform.localPosition = savedPosition;
        transform.localRotation = savedRotation;
    }
}

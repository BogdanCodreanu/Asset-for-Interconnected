using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourMoveOnPlatform : BehaviourState {
    public float speed = 5f;
    public float detectionRadius = 0.1f;
    public Vector2 onGroundPoint = Vector2.down;
    public Vector2 inFrontPoint = Vector3.right;
    public bool fallFromLedges;
    public Vector2 inLedgePoint = Vector3.right;
    public LayerMask groundLayer;
    public float turnTime;
    private bool onGround;
    private bool swapCooldown;
    private bool turnCooldown;

    private bool shouldChangeDirection;
    private float initialLocalScale;

    //[Header("Next states indexes")]
    //public int OnSight;

    public override void Init2() {
        initialLocalScale = transform.localScale.x;
    }

    public override void FixedTick() {
        onGround = Physics2D.OverlapCircle(transform.TransformPoint(onGroundPoint), detectionRadius, groundLayer);

        if (!turnCooldown) {
            unit.ownRb.velocity = new Vector2(speed * Time.deltaTime * 20f * Mathf.Sign(transform.localScale.x), unit.ownRb.velocity.y);

            if (!swapCooldown && onGround) {
                shouldChangeDirection = false;
                if (Physics2D.OverlapCircle(transform.TransformPoint(inFrontPoint), detectionRadius, groundLayer)) {
                    shouldChangeDirection = true;
                }

                if (!fallFromLedges)
                    if (!Physics2D.OverlapCircle(transform.TransformPoint(inLedgePoint), detectionRadius, groundLayer))
                        shouldChangeDirection = true;

                if (shouldChangeDirection)
                    ChangeDirection();
            }
        }
    }

    private void ChangeDirection() {
        unit.ownRb.StopAllForces();
        this.ExecuteFunctionWithDelay(turnTime, SwapSize);
        swapCooldown = true;
        turnCooldown = true;
        this.ExecuteFunctionWithDelay(0.1f, ResetSwapCooldown);
        this.ExecuteFunctionWithDelay(turnTime, ResetStopToTurnCooldown);
    }

    private void ResetSwapCooldown() {
        swapCooldown = false;
    }
    private void ResetStopToTurnCooldown() {
        turnCooldown = false;
    }

    private void SwapSize() {
        if (unit.canExecuteBehaviour)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public override void SetEnabledState(bool value) {
        ResetStopToTurnCooldown();
        ResetSwapCooldown();
        if (!value) {
            transform.localScale = new Vector3(initialLocalScale, transform.localScale.y, transform.localScale.z);
        }
    }

}

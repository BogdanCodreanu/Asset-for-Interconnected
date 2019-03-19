using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechJump : MechanicalOperational {
    public float cooldown = 2f;
    private float counterCooldown = 0;
    public GameObject particlePrefab;


    private bool canJump = true;

    public override void Start2() {
        base.Start2();

        counterCooldown = cooldown;
    }

    public override void Operate() {
        base.Operate();
        if (counterCooldown >= cooldown && canJump && ownAnchoredJoint.connectedBody) {
            ownAnchoredJoint.connectedBody.AddForceAtPosition(
                (transform.rotation * Vector2.up).normalized * 40,
                transform.position, ForceMode2D.Impulse);

            counterCooldown = 0;
            Instantiate(particlePrefab, transform.position, Quaternion.LookRotation((transform.rotation * Vector2.down).normalized, Vector3.forward));
        }
    }

    public override void OnUpdate() {
        if (counterCooldown < cooldown)
            counterCooldown += Time.deltaTime;
    }

    public override void ChangedEnabledState(bool value) {
        canJump = value;
    }
}

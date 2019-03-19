using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBombHead : UnitControl {
    public ParticleSystem ps;
    public LightFlicker lite;
    private ExplodeOnContact explodeOnContact;

    public override void Start2() {
        explodeOnContact = GetComponent<ExplodeOnContact>();
    }

    public override void StartPassiveEffects() {
        ps.StartEmmitting();
        lite.StartLight();
    }

    public override void StopPassiveEffects() {
        ps.StopEmmitting();
        lite.StopLight();
    }

    public override void OnKill() {
        explodeOnContact.Explode();
    }
}

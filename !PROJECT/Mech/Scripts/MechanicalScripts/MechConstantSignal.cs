using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechConstantSignal : MechanicalCalculator {

    public override void Start2() {
        outgoingSignal.value = 1;
    }
}

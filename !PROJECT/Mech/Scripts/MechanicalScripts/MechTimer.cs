using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechTimer : MechanicalOperational {    
    public float timeDelay = 1f;
    private float currentOutputValue;

    public override void Start2() {
        outgoingSignal.value = 0;
    }

    public override void StartOperate() {
        currentOutputValue = 0;
        CheckForSignalChange();
    }

    public override void Operate() {
        CheckForSignalChange();
    }

    private void CheckForSignalChange() {
        if (incomingSignal.outgoingSignal.value != currentOutputValue) {

        }
    }
}

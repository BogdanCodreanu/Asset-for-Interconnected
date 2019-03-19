using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSignalReverser : MechanicalCalculator {

    public override void Start2() {
        outgoingSignal.value = 1;
    }

    public override void StopOperate() {
        outgoingSignal.value = 0;
    }
    public override void StartOperate() {
        base.StartOperate();
    }
    public override void Operate() {
        outgoingSignal.value = (incomingSignal.outgoingSignal.OperationalActive()) ? 0 : 1;
    }
}

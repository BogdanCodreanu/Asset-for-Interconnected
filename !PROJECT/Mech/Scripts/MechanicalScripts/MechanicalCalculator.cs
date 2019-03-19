using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanicalCalculator : MechanicalOperational {
    public sealed override bool SignalCondition(MechSignal sig) {
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSignal {

    public MechSignal(SignalType type) {
        signalType = type;
        value = 0;
    }

    public enum SignalType { Operational, Numeric }

    private SignalType signalType;
    
    public float value;

    public SignalType GetSignalType() {
        return signalType;
    }

    public void Nullify() {
        value = 0;
    }

    public bool OperationalActive() {
        if (signalType == SignalType.Operational && value == 1)
            return true;
        return false;
    }

    public override string ToString() {
        string typeS = "";
        if (signalType == SignalType.Operational)
            typeS = "o";
        if (signalType == SignalType.Numeric)
            typeS = "N";
        return "" + value + typeS;
    }
}

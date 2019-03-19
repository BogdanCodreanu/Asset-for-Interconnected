using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanicalOperational : MechanicalPart {
    [Header("Operational")]
    
    public MechanicalOperational incomingSignal;
    public List<MechanicalOperational> goingSignalsToMechs;

    public MechSignal.SignalType outgoingSignalType;
    public bool outputsSignal = true;
    public bool receivesSignal = true;
    [HideInInspector] public MechSignal outgoingSignal;

    protected MechSignal recievedSignal;
    
    private bool isOperating;

    private SignalBodyHolder outputtingSignalBodyHolder;
    [HideInInspector] public SignalBodyHolder receivingSignalBodyHolder;

    private MechanicalOperational savedIncomingSignal;

    private WireRendererLine wireRend;
    private bool wireRendeWasActive;

    [HideInInspector] public bool executedOperations;  // pentru atunci cand vreau sa stiu de catre un tun daca semnalul de la care primeste output si-a facut Update-ul, ca sa nu traga degeaba.

    public sealed override void Awake() {
        SignalBodyHolder[] signalHolders = GetComponents<SignalBodyHolder>();
        foreach (SignalBodyHolder holder in signalHolders) {
            if (outputsSignal && holder.usedFor == SignalBodyHolder.InputOrOutput.Output)
                outputtingSignalBodyHolder = holder;
            if (receivesSignal && holder.usedFor == SignalBodyHolder.InputOrOutput.Input) {
                receivingSignalBodyHolder = holder;
            }
        }
        outgoingSignal = new MechSignal(outgoingSignalType);
        base.Awake();
    }

    public sealed override void UpdateMoreLogic() {
        if (!executedOperations) {
            executedOperations = true;

            if (enabledState) {
                RecieveSignal();
                if (isOperating)
                    Operate();
            }
            OnUpdate();

            SignalBodyHoldersLogic();
            WireRendLogic();
        }
    }

    private void LateUpdate() {
        executedOperations = false;
    }

    public WireRendererLine AssignLineRendererInitialization() {
        return VisualsGameEffects.CreateNewWire(transform.position, transform.position + Vector3.one, VisualsGameEffects.WireMaterialType.GreenWire, Color.white, .3f, 2, .03f, 5);
    }

    private void SignalBodyHoldersLogic() {
        if (outputtingSignalBodyHolder) {
            outputtingSignalBodyHolder.SetActiveSignal(outgoingSignal.OperationalActive() || outgoingSignal.GetSignalType() == MechSignal.SignalType.Numeric);
        }
        if (receivingSignalBodyHolder) {
            if (incomingSignal)
                receivingSignalBodyHolder.SetActiveSignal(SignalCondition(incomingSignal.outgoingSignal));
            else
                receivingSignalBodyHolder.SetActiveSignal(false);
        }
    }

    private void WireRendLogic() {
        if (incomingSignal) {
            if (!wireRend) {
                wireRend = AssignLineRendererInitialization();
                wireRendeWasActive = false;
            }
            if (wireRendeWasActive && !incomingSignal.outgoingSignal.OperationalActive()) {
                wireRendeWasActive = false;
                wireRend.SetWireLight(false);
            }
            if (!wireRendeWasActive && incomingSignal.outgoingSignal.OperationalActive()) {
                wireRendeWasActive = true;
                wireRend.SetWireLight(true);
            }
            wireRend.SetPoints(GetInputWireHolePosition(), incomingSignal.GetOutputWireHolePosition());
        }
        else {
            if (wireRend) {
                Destroy(wireRend);
            }
        }
    }
    public Vector3 GetInputWireHolePosition() {
        return receivingSignalBodyHolder.holderRenderer.transform.position;
    }
    public Vector3 GetOutputWireHolePosition() {
        return outputtingSignalBodyHolder.holderRenderer.transform.position;
    }

    public virtual void Operate() { }
    
    public virtual void StartOperate() { }
    
    public virtual void StopOperate() { }
    
    private void RecieveSignal() {
        if (incomingSignal) {
            if (incomingSignal.alive) {
                if (!incomingSignal.executedOperations) {  // aici verificam daca cel de la care ia semnalul si-a facut calculele.
                    incomingSignal.Update();
                }
                recievedSignal = incomingSignal.outgoingSignal;
            }
            else {
                recievedSignal = null;
                incomingSignal = null;
            }
        }
        else {
            recievedSignal = null;
        }
        SignalsLogic(recievedSignal);
    }
    
    private void SignalsLogic(MechSignal sig) {
        if (!alive) {  // check for health own conditions
            if (isOperating) {
                isOperating = false;
                outgoingSignal.Nullify();
            }
            return;
        }
        bool willOperate = false;
        if (sig != null) {
            if (SignalCondition(sig)) {
                if (!isOperating) {
                    isOperating = true;
                    StartOperate();
                }
                willOperate = true;
            }
        }

        if (!willOperate) {
            if (isOperating) {
                isOperating = false;
                outgoingSignal.Nullify();
                StopOperate();
            }
        }
    }

    public virtual bool SignalCondition(MechSignal sig) {
        if (sig.GetSignalType() == MechSignal.SignalType.Operational) {
            return OperationalSignalCondition(sig);
        }
        if (sig.GetSignalType() == MechSignal.SignalType.Numeric) {
            return NumericSignalCondition(sig);
        }
        return false;
    }
    private bool OperationalSignalCondition(MechSignal sig) {
        return sig.value == 1;
    }
    public virtual bool NumericSignalCondition(MechSignal sig) {
        return false;
    }

    public sealed override void SetEnabledStateMoreLogic(bool value) {
        if (value) {
            StopOperate();

            if (!gameObject.activeInHierarchy)
                KillWires();

            if (incomingSignal)
                if (!incomingSignal.IsValidMech) {
                    KillWires();
                    Disconnect();
                }
        }
        if (!value) {
            isOperating = false;
            StopOperate();
        }
    }

    public void ConnectTo(MechanicalOperational willGetSignal) {
        incomingSignal = willGetSignal;
        incomingSignal.goingSignalsToMechs.Add(this);
    }
    /// <summary>
    /// Disconnects from whatever it's connected to
    /// </summary>
    public void Disconnect() {
        if (incomingSignal)
            incomingSignal.goingSignalsToMechs.Remove(this);
        incomingSignal = null;
    }

    /// <summary>
    /// Disconnects from the given Mech
    /// </summary>
    public void Disconnect(MechanicalOperational fromThis) {
        incomingSignal = null;
        fromThis.goingSignalsToMechs.Remove(this);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        KillWires();
        Disconnect();
    }

    public sealed override void KillPart(bool killJointConnection = true, bool recursiveKillChildrenCondition = true) {
        base.KillPart(killJointConnection, recursiveKillChildrenCondition);
        if (enabledState) {
            incomingSignal = null;
            isOperating = false;
            outgoingSignal.Nullify();
        }
    }

    public sealed override void SaveLifeProperties() {
        base.SaveLifeProperties();
        savedIncomingSignal = incomingSignal;
    }

    public sealed override void AssignLifeProperties() {
        base.AssignLifeProperties();
        incomingSignal = savedIncomingSignal;
    }

    private void KillWires() {
        if (wireRend)
            Destroy(wireRend);
    }

    public override string GenerateSavedString() {
        string outer = "OPERATIONAL#";
        outer += ((incomingSignal != null) ? incomingSignal.indexInSavingGroup.ToString() : "-1") + " ";
        return base.GenerateSavedString() + outer;
    }

    public override void AssignReadSavedString(string read, MechanicalPart[] allSpawns) {
        base.AssignReadSavedString(read, allSpawns);
        int auxInt;
        string auxString;

        System.Text.RegularExpressions.MatchCollection allMatches;
        auxString = System.Text.RegularExpressions.Regex.Replace(read, @"(.*OPERATIONAL#)(.*)", "$2");
        allMatches = System.Text.RegularExpressions.Regex.Matches(auxString, @"-?\d+");
        
        auxInt = int.Parse(allMatches[0].ToString());
        if (auxInt != -1) {
            ConnectTo(allSpawns[auxInt] as MechanicalOperational);
        }
    }

}

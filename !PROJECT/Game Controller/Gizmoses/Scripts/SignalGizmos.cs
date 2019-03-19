using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalGizmos : GizmosControl {
    private WireRendererLine wireRend;
    
    private MechanicalOperational willGetSignal;
    private Collider2D determinedColl;

    private MechanicalOperational currentMechOp;
    public float translatedFactor = 1;

    private MechanicalOperational initialOperational;

    //protected override bool AllowValidMechOnTheCurrentState() {
    //    if (dragging) {
    //        if (willGetSignal)
    //            return willGetSignal.IsValidMech;
    //        else
    //            return true;
    //    }
    //    if (currentMechOp.incomingSignal)
    //        return currentMechOp.incomingSignal.IsValidMech;
    //    else
    //        return true;
    //}

    public override void Init(MechanicalPart modifyingObject) {
        base.Init(modifyingObject);
        currentMechOp = modifyingObject as MechanicalOperational;
        if (!currentMechOp)
            Debug.LogError("Signal gizmos on non mechanical operational " + modifyingObject.gameObject.name);

        transform.position = modifyingObject.handleTransform.position + new Vector3(1, 1, 0) * cameraScaling * translatedFactor;

        dragging = false;
    }

    private void CreateLine(Vector3 to) {
        wireRend = currentMechOp.AssignLineRendererInitialization();
    }

    public override void OnFirstClick(Vector3 mousePosition) {
        initialOperational = currentMechOp.incomingSignal;

        currentMechOp.Disconnect();
        
        if (!wireRend) {
            CreateLine(GlobalMousePosition.mouseWorldPos);
        }

        currentMechOp.incomingSignal = null;
    }

    public override void OnDrag(Vector3 mousePosition) {
        wireRend.SetPoints(currentMechOp.GetInputWireHolePosition(), (willGetSignal) ? willGetSignal.GetOutputWireHolePosition() : GlobalMousePosition.mouseWorldPos);

        HoverSelectionLogic(mousePosition);
    }

    public override void ShowDescription() {
        itemDescriptionSpawn.SetText("Signal From: " + ((willGetSignal) ? willGetSignal.mechName : "Nothing"));
    }

    public override void OnReleaseClick(Vector3 mousePosition) {
        base.OnReleaseClick(mousePosition);

        if (willGetSignal) {
            currentMechOp.ConnectTo(willGetSignal);
        }
        if (wireRend)
            Destroy(wireRend);
        
    }

    public override void OnUpdate() {
        transform.position = currentMech.handleTransform.position + new Vector3(1, 1, 0) * cameraScaling * translatedFactor;
    }

    private void HoverSelectionLogic(Vector3 mouseWorldPos) {
        determinedColl = SelectionFuncs.DetermineCollider(mouseWorldPos,
            SelectionFuncs.DetermineColliderLogic.MostImportantMech, currentMech, false, false, true);
        if (determinedColl)
            willGetSignal = determinedColl.GetComponent<MechanicalOperational>();
        else
            willGetSignal = null;
    }

    public override bool ShouldRecordUndoOnSuccessPlacement() {
        return initialOperational != willGetSignal;
    }
}

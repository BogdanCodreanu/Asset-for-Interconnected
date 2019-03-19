using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSnapTranslationGizmos : GizmosControl {

    private Vector3 initialPos;

    public override void Init(MechanicalPart modifyingObject) {
        base.Init(modifyingObject);
        currentMech = modifyingObject;
    }

    public override void OnFirstClick(Vector3 mousePosition) {
        initialPos = currentMech.transform.position;
    }

    public override void OnDrag(Vector3 mousePosition) {
        currentMech.DragMove(mousePosition + grabDiff);
        transform.position = mousePosition + grabDiff;
    }

    public override void ShowDescription() {
        itemDescriptionSpawn.SetText("Position: (" + (currentMech.handleTransform.position.x).ToString("0.00") + ", " + (currentMech.handleTransform.position.y).ToString("0.00") + ")");
    }

    public override void OnUpdate() {
        if (!dragging)
            transform.position = currentMech.handleTransform.position;
    }

    public override void OnReleaseClick(Vector3 mousePosition) {
        base.OnReleaseClick(mousePosition);
        //firstPlacement = false;
        transform.position = currentMech.transform.position;
    }

    //public override void SuccessfulPlacement(Vector3 mousePosition) {
    //    base.SuccessfulPlacement(mousePosition);
    //    if (firstPlacement)
    //        currentMech.SuccessfulFirstPlacement();
    //    firstPlacement = false;
    //}

    //public override void FailedPlacement(Vector3 mousePosition) {
    //    base.SuccessfulPlacement(mousePosition);
    //    currentMech.transform.position = initialMechPos;
    //}

    public override bool ShouldRecordUndoOnSuccessPlacement() {
        return initialPos != currentMech.transform.position;
    }
}

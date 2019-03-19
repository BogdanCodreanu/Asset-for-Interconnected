using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnctionGizmos : GizmosControl {
    private SelectionHover hoveringSelection;
    public Color selectionColor;

    private Collider2D determinedColl;
    private MechanicalPart willSnapToMech;

    private Vector3 pointDiff;
    

    public override void Init(MechanicalPart modifyingObject) {
        transform.position = currentMech.handleTransform.position;
        modifyingObject.SetMovingGizmosTo(this);
        movementGizmosAndCanDuplicate = true;
    }

    protected override bool AllowValidMechOnTheCurrentState() {
        if (dragging) {
            if (willSnapToMech)
                return willSnapToMech.IsValidMech;
            else
                return false;
        }
        if (!currentMech.connectedToMech)
            return false;
        return currentMech.connectedToMech.IsValidMech;
    }

    public override void SetObjectOnPointByGizmos(Vector3 mousePosition) {
        grabDiff = Vector3.zero;
        OnDrag(mousePosition);
    }

    public override void OnFirstClick(Vector3 mousePosition) {
        base.OnFirstClick(mousePosition);
        pointDiff = currentMech.transform.position - currentMech.handleTransform.position;
    }

    public override void OnDrag(Vector3 mousePosition) {
        HoverSelectionLogic(mousePosition + grabDiff);
        transform.position = mousePosition + grabDiff;
        currentMech.DragMove(transform.position + pointDiff);
    }

    public override void ShowDescription() {
        itemDescriptionSpawn.SetText("Connect To: " + ((willSnapToMech) ? willSnapToMech.mechName : "Nothing"));
    }

    public override void OnReleaseClick(Vector3 mousePosition) {
        base.OnReleaseClick(mousePosition);
        if (hoveringSelection)
            Destroy(hoveringSelection);
        //firstPlacement = false;

        if (willSnapToMech) {
            currentMech.ConnectToRigidbody(willSnapToMech, currentMech.transform.InverseTransformPoint(transform.position));
            currentMech.RecreateFixedJoint();
            currentMech.DragMove(mousePosition + grabDiff + pointDiff);
            currentMech.transform.parent = willSnapToMech.transform;
        } else {
            currentMech.DisconnectFromRigidBody();
            currentMech.transform.parent = null;
        }
    }

    //public override bool SuccessCondition() {
    //    return willSnapToMech; // && !someoneTouchesBounds;
    //}

    //public override void SuccessfulPlacement(Vector3 mousePosition) {
    //    base.SuccessfulPlacement(mousePosition);
    //    currentMech.ConnectToRigidbody(willSnapToMech.ownRb, currentMech.transform.InverseTransformPoint(transform.position));
    //    currentMech.RecreateFixedJoint();
    //    currentMech.DragMove(mousePosition + grabDiff + pointDiff);
    //    currentMech.transform.parent = willSnapToMech.transform;

    //    if (firstPlacement)
    //        currentMech.SuccessfulFirstPlacement(true);
    //}

    //public override void FailedPlacement(Vector3 mousePosition) {
    //    base.FailedPlacement(mousePosition);
    //    currentMech.DragMove(initialMechPos);
    //    transform.position = initialJointPos;

    //    if (firstPlacement)
    //        currentMech.FailedFirstPlacement();
    //}
    

    private void HoverSelectionLogic(Vector3 mouseWorldPos) {
        determinedColl = SelectionFuncs.DetermineCollider(mouseWorldPos,
            SelectionFuncs.DetermineColliderLogic.LeastImportantMech, currentMech, false, true);
        if (determinedColl)
            willSnapToMech = determinedColl.GetComponent<MechanicalPart>();
        else
            willSnapToMech = null;

        if (willSnapToMech) {  // if i have somethign on mouse
            if (hoveringSelection) {  // if i had a hover selection
                if (hoveringSelection.followedObject != willSnapToMech.transform) {  // that is not the hoverSelection over the object on mouse
                    Destroy(hoveringSelection);  // kill it
                }
            }

            if (!hoveringSelection) {
                hoveringSelection = SelectionFuncs.CreateSelectionOver(determinedColl, SelectingParts.SelectionMaterial, selectionColor);
            }
        } else {
            if (hoveringSelection)
                Destroy(hoveringSelection);
        }
    }

    public override void OnUpdate() {
        if (!dragging)
            transform.position = currentMech.handleTransform.position;
    }

    public override void OnKilledGizmos() {
        if (hoveringSelection)
            Destroy(hoveringSelection);
    }

    public override bool ShouldRecordUndoOnSuccessPlacement() {
        return currentMech.transform.position != initialMechPos;
    }

}

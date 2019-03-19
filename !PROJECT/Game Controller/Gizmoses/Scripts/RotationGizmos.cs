using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGizmos : GizmosControl {
    private Vector3 diffPos;
    private float rot_z;
    private Quaternion futureRot;
    private GameObject rotator;
    public float translatedFactor = 1;

    private Vector4 initialRot;

    public override void Init(MechanicalPart modifyingObject) {
        base.Init(modifyingObject);
        currentMech = modifyingObject;
        transform.position = currentMech.handleTransform.position
            + currentMech.handleTransform.TransformDirection(Quaternion.Euler(0, 0, currentMech.GizmosRotationAngle)
            * Vector3.right) * cameraScaling * translatedFactor;
    }

    public override void OnDrag(Vector3 mousePosition) {
        transform.position = currentMech.handleTransform.position
            + (mousePosition + grabDiff - currentMech.handleTransform.position).normalized
            * cameraScaling * translatedFactor;

        diffPos = (transform.position - currentMech.handleTransform.position).normalized;

        rot_z = Mathf.Atan2(diffPos.y, diffPos.x) * Mathf.Rad2Deg;
        futureRot = Quaternion.Euler(0f, 0f, rot_z - currentMech.GizmosRotationAngle);
        rotator.transform.rotation = futureRot;
    }

    public override void ShowDescription() {
        itemDescriptionSpawn.SetText("Rotation: " + (360 - rotator.transform.eulerAngles.z).ToString("000.00"));
    }

    public override void OnFirstClick(Vector3 mousePosition) {
        base.OnFirstClick(mousePosition);
        initialRot = new Vector4(currentMech.transform.rotation.x, currentMech.transform.rotation.y, currentMech.transform.rotation.z, currentMech.transform.rotation.w);
        rotator = new GameObject("Rotator TwoPoint");
        rotator.transform.position = currentMech.handleTransform.position;
        rotator.transform.parent = currentMech.transform.parent;
        rotator.transform.rotation = currentMech.transform.rotation;
        currentMech.transform.parent = rotator.transform;
    }

    public override void OnReleaseClick(Vector3 mousePosition) {
        base.OnReleaseClick(mousePosition);
        if (rotator) {
            currentMech.transform.parent = rotator.transform.parent;
            Destroy(rotator);
        }
        currentMech.RecreateFixedJoint();
    }

    //public override void SuccessfulPlacement(Vector3 mousePosition) {
    //    base.SuccessfulPlacement(mousePosition);
    //    if (rotator) {
    //        currentMech.transform.parent = rotator.transform.parent;
    //        Destroy(rotator);
    //    }
    //    currentMech.RecreateFixedJoint();
    //}

    //public override void FailedPlacement(Vector3 mousePosition) {
    //    base.FailedPlacement(mousePosition);
    //    if (rotator) {
    //        currentMech.transform.parent = rotator.transform.parent;
    //        Destroy(rotator);
    //    }
    //    currentMech.transform.position = initialMechPos;
    //    currentMech.transform.rotation = initialMechRot;
    //}

    public override void OnUpdate() {
        if (!dragging)
            transform.position = currentMech.handleTransform.position
                + currentMech.handleTransform.TransformDirection(Quaternion.Euler(0, 0, currentMech.GizmosRotationAngle)
                * Vector3.right) * cameraScaling * translatedFactor;
    }

    public override void OnKilledGizmos() {
        if (rotator) {
            currentMech.transform.parent = rotator.transform.parent;
            Destroy(rotator);
        }
    }

    public override bool ShouldRecordUndoOnSuccessPlacement() {
        return initialRot != new Vector4(currentMech.transform.rotation.x, currentMech.transform.rotation.y, currentMech.transform.rotation.z, currentMech.transform.rotation.w);
    }
}

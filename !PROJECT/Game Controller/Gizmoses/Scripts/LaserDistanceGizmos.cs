using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDistanceGizmos : GizmosControl {
    
    private MechLaser laserMech;
    private LineRenderer visualLine;
    public Material visualLineMaterial;
    public Color visualLineColor;
    private float initialDistance;

    public override void Init(MechanicalPart modifyingObject) {
        laserMech = currentMech as MechLaser;
        transform.position = laserMech.barrel.position + laserMech.barrel.TransformDirection(new Vector3(
            laserMech.distance, 0, 0));
        cameraReductionSize = 0.6f;
        visualLine = VisualsGameEffects.CreateNewLine(laserMech.barrel.position, transform.position, visualLineMaterial, visualLineColor, 0.05f, 3, "Gizmos", -100);
    }

    public override void OnFirstClick(Vector3 mousePosition) {
        initialDistance = laserMech.distance;
    }

    public override void OnDrag(Vector3 mousePosition) {
        laserMech.distance = laserMech.barrel.InverseTransformDirection(mousePosition - laserMech.barrel.position).x;

        if (laserMech.distance > laserMech.maxDistance)
            laserMech.distance = laserMech.maxDistance;
        if (laserMech.distance < laserMech.minDistance)
            laserMech.distance = laserMech.minDistance;
        transform.position = laserMech.barrel.position + laserMech.barrel.TransformDirection(new Vector3(
            laserMech.distance, 0, 0));
        visualLine.SetPosition(1, transform.position);
    }

    public override void ShowDescription() {
        itemDescriptionSpawn.SetText("Distance: " + (laserMech.distance).ToString("0.00"));
    }

    public override void OnUpdate() {
        if (!dragging)
            transform.position = laserMech.barrel.position + laserMech.barrel.TransformDirection(new Vector3(
                laserMech.distance, 0, 0));
        visualLine.SetPosition(0, laserMech.barrel.position);
        visualLine.SetPosition(1, transform.position);
    }

    public override void OnKilledGizmos() {
        if (visualLine) {
           Destroy(visualLine.gameObject);
        }
    }

    public override bool ShouldRecordUndoOnSuccessPlacement() {
        return laserMech.distance != initialDistance;
    }
}

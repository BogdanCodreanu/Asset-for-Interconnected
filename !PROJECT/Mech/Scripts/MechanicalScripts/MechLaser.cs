using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechLaser : MechanicalOperational {
    private Ray2D scanningRay = new Ray2D();
    private RaycastHit2D hit;
    public float distance = 5;
    public float maxDistance = 10f;
    public float minDistance = 1f;
    public Transform barrel;
    public LayerMask detectionLayer;
    private LineRenderer lineRend;

    public GameObject laserSparklePrefab;
    private ParticleAndLightControl laserSparkleSpawn;

    public Material laserMaterial;
    public Color laserColor;

    public override void Start2() {
        base.Start2();
    }
    

    public override void Operate() {
        scanningRay.origin = barrel.position;
        scanningRay.direction = (barrel.position - transform.position).normalized;
        hit = Physics2D.Raycast(scanningRay.origin, scanningRay.direction, distance, detectionLayer);

        lineRend.SetPosition(0, scanningRay.origin);

        if (hit.collider) {
            if (!laserSparkleSpawn)
                laserSparkleSpawn = Instantiate(laserSparklePrefab.gameObject, hit.point, Quaternion.identity).GetComponent<ParticleAndLightControl>();
            else
                laserSparkleSpawn.transform.position = hit.point;

            lineRend.SetPosition(1, hit.point);
            lineRend.endColor = new Color(1, 1, 1, 1);
            outgoingSignal.value = 1;

        }
        else {
            KillLaserSparkleSpawn();
            lineRend.SetPosition(1, scanningRay.origin + scanningRay.direction * distance);
            lineRend.endColor = new Color(1, 1, 1, 0);
            outgoingSignal.value = 0;
        }
    }

    private void KillLaserSparkleSpawn() {
        if (laserSparkleSpawn) {
            laserSparkleSpawn.KillBoth(1f, 0.2f);
            laserSparkleSpawn = null;
        }
    }

    public override void StopOperate() {
        outgoingSignal.value = 0;
        KillLaserSparkleSpawn();
        if (lineRend) {
            Destroy(lineRend.gameObject);
        }
    }
    public override void StartOperate() {
        lineRend = VisualsGameEffects.CreateNewLine(barrel.position, barrel.position + (barrel.position - transform.position).normalized, laserMaterial, Color.white, 0.05f, 0, "Gameplay Effects", 0);
        lineRend.endColor = new Color(1, 1, 1, 0);
    }

    public override void KillAllSpawns() {
        KillLaserSparkleSpawn();
        if (lineRend)
            Destroy(lineRend.gameObject);
    }

    public override string GenerateSavedString() {
        string outer = "LASER#" + distance + " ";

        return base.GenerateSavedString() + outer;
    }

    public override void AssignReadSavedString(string read, MechanicalPart[] allSpawns) {
        base.AssignReadSavedString(read, allSpawns);

        string auxString;

        System.Text.RegularExpressions.MatchCollection allMatches;
        auxString = System.Text.RegularExpressions.Regex.Replace(read, @"(.*LASER#)(.*)", "$2");
        allMatches = System.Text.RegularExpressions.Regex.Matches(auxString, @"-?\d+(\.\d*)?");
        distance = float.Parse(allMatches[0].ToString());
    }
}

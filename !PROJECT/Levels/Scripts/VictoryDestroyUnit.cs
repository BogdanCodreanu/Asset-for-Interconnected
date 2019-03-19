using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDestroyUnit : VictoryCondition {
    public UnitControl[] unitsNeededToDie;
    private bool allDead;
    private int unitsDead;
    
    public override void OnUpdate() {
        unitsDead = 0;
        foreach (UnitControl unit in unitsNeededToDie) {
            if (!unit.alive) {
                unitsDead++;
            }
        }
        allDead = (unitsDead == unitsNeededToDie.Length);
    }

    public override void SetHeaderSliderVictory() {
        headerUI.progressionSlider.SetObjectiveSliderSmooth((float)unitsDead / unitsNeededToDie.Length);
    }

    public override bool WinCondition() {
        return allDead;
    }

    public override Vector3 CameraShowObjectiveVictory() {
        return unitsNeededToDie[unitsNeededToDie.Length - 1].transform.position;
    }
}

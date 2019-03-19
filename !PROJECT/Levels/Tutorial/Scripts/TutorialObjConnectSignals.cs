using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjConnectSignals : TutorialObjective {
    public Animator panelUIAnimator;
    public SpawningParts spawningParts;
    public MechanicalPart mechConstantSignalPrefab;

    private bool oneWheelConnected;


    public override void OnUpdate() {
        CheckConnection();
    }

    public override bool ConditionToWin() {
        if (oneWheelConnected)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        spawningParts.AddSpawnToGrid(mechConstantSignalPrefab);
        InfoUI.InformationalMoving("<style=Game>Constant Signal Part</style> added in the <style=Game>Parts Panel</style>.", 5);
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
        InfoUI.InformationalClosablePopup("Connection", "Notice that by starting the Simulation, the wheels connected to the Constant Signal now move.", 20, false);
    }

    private void CheckConnection() {
        oneWheelConnected = false;
        foreach (MechanicalPart m in GameController.mechs) {
            if (m is MechWheel) {
                if ((m as MechWheel).incomingSignal != null) {
                    if ((m as MechWheel).incomingSignal is MechConstantSignal) {
                        oneWheelConnected = true;
                        return;
                    }
                }
            }
        }
    }
}

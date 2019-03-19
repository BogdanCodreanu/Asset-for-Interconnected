using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjPlaceBodyAndWheels : TutorialObjective {
    public Animator panelUIAnimator;
    private bool containsBody;
    private int nrOfWheels;
    public GameObject spawningPartsPanel;
    

    public override void OnUpdate() {
        CheckBody();
        CheckWheels();
    }

    public override bool ConditionToWin() {
        if (containsBody && nrOfWheels >= 2)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        spawningPartsPanel.SetActive(true);
        InfoUI.InformationalMoving("Two new mechs added in the <style=Game>Parts Panel</style>.", 5, 1f);
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
        InfoUI.InformationalClosablePopup("No Connection", "Notice that by starting the Simulation, the wheels will not work, that's because they don't have an <style=Game>Input Signal</style>.", 25, false);
    }

    private void CheckBody() {
        foreach (MechanicalPart m in GameController.mechs) {
            if (m.mechName == "Stone Extension") {
                containsBody = true;
                return;
            }
        }
    }

    private void CheckWheels() {
        nrOfWheels = 0;
        foreach (MechanicalPart m in GameController.mechs) {
            if (m is MechWheel) {
                nrOfWheels++;
            }
        }
    }
}

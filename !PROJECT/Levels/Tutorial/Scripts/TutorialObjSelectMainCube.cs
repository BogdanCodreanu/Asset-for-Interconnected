using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjSelectMainCube : TutorialObjective {
    public Animator panelUIAnimator;
    private SelectingParts selectingParts;

    private void Start() {
        selectingParts = GameController.gameController.GetComponent<SelectingParts>();
    }


    public override bool ConditionToWin() {
        if (selectingParts.GetSelectedObject() == GameController.mainCube)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        GameController.notWorkinDueTutorial = true;
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
        InfoUI.InformationalClosablePopup("Handles", "When you select an object, its <style=Game>Handles</style> are going to appear. These modify the selected object's properties", 20f);
    }
}

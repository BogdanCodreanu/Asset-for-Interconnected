using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjMoveRotateMainCube : TutorialObjective {
    public Animator panelUIAnimator;
    private Transform trans;
    private Vector3 initialPos;
    private Quaternion initialRot;

    private void Start() {
        trans = GameController.mainCube.transform;
    }


    public override bool ConditionToWin() {
        if (trans.position != initialPos && trans.rotation != initialRot)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        InfoUI.InformationalClosablePopup("Creation Bounds", "Objects cannot be placed outside the creation bounds.", 20f);
        initialPos = trans.position;
        initialRot = trans.rotation;
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
    }
}

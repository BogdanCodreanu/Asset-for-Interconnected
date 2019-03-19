using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjMoveCamera : TutorialObjective {
    public Animator panelUIAnimator;
    private Vector3 lastCameraPos;
    public float distanceTravelledNeeded = 4f;
    private float currentDistanceTravelled;

    public GameObject[] turnedOffGameCanvas;

    private void Start() {
        lastCameraPos = GameController.cameraControl.transform.position;
        currentDistanceTravelled = 0f;
        foreach (GameObject g in turnedOffGameCanvas) {
            g.SetActive(false);
        }
    }

    public override void OnUpdate() {
        lastCameraPos.z = GameController.cameraControl.transform.position.z;
        currentDistanceTravelled += (GameController.cameraControl.transform.position - lastCameraPos).magnitude;
        lastCameraPos = GameController.cameraControl.transform.position;
    }

    public override bool ConditionToWin() {
        if (currentDistanceTravelled >= distanceTravelledNeeded)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        GameController.notWorkinDueTutorial = true;
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
    }
}

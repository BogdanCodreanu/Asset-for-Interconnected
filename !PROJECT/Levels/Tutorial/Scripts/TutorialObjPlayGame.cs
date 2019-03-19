using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjPlayGame : TutorialObjective {
    private const float groundSettingDuration = 1f;
    public Animator panelUIAnimator;
    public GameObject headerToTurnOn;
    public GameObject firstGround;
    public Vector3 firstGroundPositionDesired;
    public Light toTurnOnLight;

    private bool played;
    private bool stopped;
    

    public override void OnUpdate() {
        if (!played) {
            if (GameController.GetGameState() == GameController.GameState.PLAYING) {
                played = true;
            }
        }
        else {
            if (GameController.GetGameState() == GameController.GameState.CREATING) {
                stopped = true;
            }
        }
    }

    public override bool ConditionToWin() {
        if (played && stopped)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        StartCoroutine(DesignGround());
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
    }

    private IEnumerator DesignGround() {
        firstGround.SetActive(true);
        Vector3 initialGroundPos = firstGround.transform.position;
        float startTime = Time.time;
        float lerper = 0;

        CameraControl cameraControl = GameController.cameraControl;
        Vector3 initialCameraPos = cameraControl.transform.position;
        cameraControl.DisableThinking();

        while (lerper < 1) {
            lerper = (Time.time - startTime) / groundSettingDuration;
            firstGround.transform.position = Vector3.Lerp(initialGroundPos, firstGroundPositionDesired, lerper);

            cameraControl.transform.position = Vector3.Lerp(initialCameraPos, new Vector3(0, 0, -45), lerper);
            yield return new WaitForEndOfFrame();
        }
        toTurnOnLight.Appear(this, 1f, true, 1, false, 0);
        cameraControl.EnableThinking();

        headerToTurnOn.SetActive(true);
        FindObjectOfType<HeaderUI>().ShowPlayButton();
        GameController.notWorkinDueTutorial = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjZoomCamera : TutorialObjective {
    public Animator panelUIAnimator;
    private float initialCameraPos;
    public float differenceNeeded = 10f;

    public Vector3 nextCameraTopRightCorner;
    public Vector3 nextCameraBottomLeftCorner;
    public AnimationCurve translatingCamera;
    public float movingCameraDuration;

    public Light[] willTurnOnLights;

    public override bool ConditionToWin() {
        if (Mathf.Abs(GameController.cameraControl.transform.position.z - initialCameraPos) >= differenceNeeded)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
        initialCameraPos = GameController.cameraControl.transform.position.z;
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
        StartCoroutine(MoveTheCamera());
    }

    private IEnumerator MoveTheCamera() {
        yield return new WaitForSeconds(0.5f);

        CameraControl cameraControl = GameController.cameraControl;
        float startTime = Time.time;
        float lerper = 0f;
        Vector3 initialCameraPos = cameraControl.transform.position;

        cameraControl.DisableThinking();

        foreach (Light l in willTurnOnLights) {
            l.Appear(this, 1, true, 1, false, 0);
        }

        while (lerper < 1f) {
            lerper = (Time.time - startTime) / movingCameraDuration;

            cameraControl.transform.position = Vector3.Lerp(initialCameraPos, new Vector3(0, 0, initialCameraPos.z), translatingCamera.Evaluate(lerper));
            yield return new WaitForEndOfFrame();
        }

        cameraControl.ChangeLimits(nextCameraTopRightCorner, nextCameraBottomLeftCorner);
        cameraControl.EnableThinking();

    }
}

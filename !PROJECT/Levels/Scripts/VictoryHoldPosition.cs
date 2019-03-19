using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VictoryHoldPosition : VictoryCondition {

    private bool insideCollider;
    public float timeNeededInside = 1f;
    private float counter;
    

    private void Start() {
        counter = 0;
    }

    public override void OnUpdate() {
        if (insideCollider) {
            counter += Time.deltaTime;
        }
    }
    public override void SetHeaderSliderVictory() {
        if (insideCollider) {
            headerUI.progressionSlider.SetObjectiveSliderDirect(counter / timeNeededInside);
        } else {
            headerUI.progressionSlider.SetObjectiveSliderSmooth(0);
        }
    }

    public override bool WinCondition() {
        if (counter >= timeNeededInside)
            return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == GameController.mainCube.gameObject) {
            counter = 0f;
            insideCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == GameController.mainCube.gameObject) {
            counter = 0f;
            insideCollider = false;
        }
    }

    public override Vector3 CameraShowObjectiveVictory() {
        return transform.position;
    }
}

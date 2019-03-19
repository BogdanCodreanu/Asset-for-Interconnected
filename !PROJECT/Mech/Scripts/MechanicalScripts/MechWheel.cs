using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechWheel : MechanicalOperational {
    
    public float motorPower = 700f;
    public FloatObject autoBreak;
    [Range(0, 1)]
    public float accelerationTime = .5f;

    public override void Start2() {
        base.Start2();
    }

    public override void Operate() {
        base.Operate();
        //if (joint)
        //    joint.motor = new JointMotor2D { motorSpeed = motorPower, maxMotorTorque = 10000 };
    }


    public override void StartOperate() {
        base.StartOperate();
        (ownAnchoredJoint as HingeJoint2D).useMotor = true;
        StartCoroutine(ChangeWheelPower(motorPower, accelerationTime));
    }

    public override void StopOperate() {
        base.StopOperate();
        if (!(ownAnchoredJoint as HingeJoint2D))
            Debug.LogError("HingeJoint2D missing!!");
        if ((ownAnchoredJoint as HingeJoint2D)) {
            (ownAnchoredJoint as HingeJoint2D).useMotor = (autoBreak.value == 1);

            if ((ownAnchoredJoint as HingeJoint2D).useMotor) {
                if (gameObject.activeInHierarchy)
                    StartCoroutine(ChangeWheelPower(0, accelerationTime));
            }
        }
    }

    private IEnumerator ChangeWheelPower(float futureValue, float duration) {
        float startTime = Time.time;
        float initialVal = (ownAnchoredJoint as HingeJoint2D).motor.motorSpeed;
        while (Time.time - startTime < duration) {
            (ownAnchoredJoint as HingeJoint2D).motor = new JointMotor2D { maxMotorTorque = 1000, motorSpeed = Mathf.Lerp(initialVal, futureValue, (Time.time - startTime) / duration) };
            yield return new WaitForEndOfFrame();
        }
        (ownAnchoredJoint as HingeJoint2D).motor = new JointMotor2D { motorSpeed = futureValue, maxMotorTorque = 1000 };
    }

    public override void ChangedEnabledState(bool value) {
        base.ChangedEnabledState(value);
        StopAllCoroutines();
    }

    public override void OnKilledPart() {
        base.OnKilledPart();
        StopAllCoroutines();
    }
    
    public override void AskForUIFields(MechanicalUI spawnUI) {
        //spawnUI.Ask4FieldToggle(autoBreak, "Auto Break", "On", "Off", autoBreak.value == 1);
    }
}

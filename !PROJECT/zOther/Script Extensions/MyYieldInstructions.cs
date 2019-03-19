using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsOrInput : CustomYieldInstruction {
    private float startTime;
    private float neededTime;
    public WaitForSecondsOrInput(float seconds) {
        startTime = Time.time;
        neededTime = seconds;
    }
    public override bool keepWaiting {
        get {
            return !Input.anyKeyDown && (Time.time - startTime < neededTime);
        }
    }
}

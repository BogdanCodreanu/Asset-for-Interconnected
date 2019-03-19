using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjReachDestination : TutorialObjective {
    private const float groundSettingDuration = 1f;
    public Animator panelUIAnimator;
    public TutorialVictory tutorialVictory;
    

    public override bool ConditionToWin() {
        if (tutorialVictory.victorious)
            return true;
        return false;
    }

    public override void OnStartedObjective() {
        panelUIAnimator.SetBool("OnScreen", true);
    }

    public override void OnFinishObjective() {
        panelUIAnimator.SetBool("OnScreen", false);
    }
}

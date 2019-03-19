using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialObjective : MonoBehaviour {
    private TutorialController tutorialController;
    private bool finished = false;

    public void SetTutorialController(TutorialController tc) {
        tutorialController = tc;
    }

    public void Tick() {
        if (!finished) {
            OnUpdate();
            if (ConditionToWin()) {
                tutorialController.NextTutorial();
                finished = true;
            }
        }
    }

    public abstract bool ConditionToWin();
    public virtual void OnUpdate() { }
    public virtual void OnFinishObjective() { }
    public virtual void OnStartedObjective() { }

}

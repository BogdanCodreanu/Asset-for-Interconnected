using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourState : MonoBehaviour {

    private BehaviourController behaviourController;
    protected UnitControl unit;

    public void Init(BehaviourController behaviourController, UnitControl unit) {
        this.behaviourController = behaviourController;
        this.unit = unit;
        Init2();
    }
    public virtual void Init2() { }


    public abstract void FixedTick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public void MoveToState(int index) {
        behaviourController.SetState(index);
    }

    public virtual void SetEnabledState(bool value) { }

}

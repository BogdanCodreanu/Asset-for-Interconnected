using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourController : MonoBehaviour {

    public BehaviourState[] allStates;
    private BehaviourState currentState;
    private UnitControl unit;

    private void Awake() {
        unit = GetComponent<UnitControl>();

        foreach (BehaviourState state in allStates) {
            state.Init(this, unit);
        }
        SetState(0);
    }

    private void FixedUpdate() {
        if (unit.canExecuteBehaviour)
            currentState.FixedTick();
    }

    public void SetState(int newStateIndex) {
        if (currentState) {
            currentState.OnStateExit();
        }

        if (newStateIndex >= allStates.Length) {
            Debug.LogError("Next state index too large - " + name);
            return;
        }

        currentState = allStates[newStateIndex];
        currentState.OnStateEnter();
    }

    public void SetEnabledState(bool value) {
        foreach (BehaviourState state in allStates) {
            state.SetEnabledState(value);
        }
    }

    public void StopBehaviourCoroutines() {
        foreach (BehaviourState state in allStates) {
            state.StopAllCoroutines();
        }
    }

}

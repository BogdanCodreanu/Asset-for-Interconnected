using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VictoryCondition : MonoBehaviour {
    protected HeaderUI headerUI;

    protected bool victorious;

    private void Awake() {
        headerUI = GameController.mainCanvasStatic.headerUI;
        OnAwake();
        GameController.cameraControl.BeginShowingTheObjective(this, CameraShowObjectiveVictory());
    }

    private void Victory() {
        if (GameController.GetGameState() == GameController.GameState.PLAYING && !victorious) {
            victorious = true;
            GameController.Victory();
        }
    }

    private void Update() {
        if (GameController.GetGameState() == GameController.GameState.PLAYING && GameController.mainCube.IsValidMech) {
            if (WinCondition())
                Victory();
            OnUpdate();
            SetHeaderSliderVictory();
        }
    }

    public virtual void OnUpdate() { }
    public virtual void OnAwake() { }
    public abstract bool WinCondition();
    public abstract Vector3 CameraShowObjectiveVictory();
    public abstract void SetHeaderSliderVictory();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVictory : MonoBehaviour {
    public bool victorious;
    private bool insideCollider;
    public float timeNeededInside = 1f;
    private float counter;



    private void Start() {
        counter = 0;
    }

    private void Victory() {
        if (GameController.GetGameState() == GameController.GameState.PLAYING) {
            victorious = true;
        }
    }

    private void Update() {
        if (!victorious) {
            if (WinCondition())
                Victory();
            OnUpdate(Time.deltaTime);
        }
    }

    private void OnUpdate(float deltaTime) {
        if (insideCollider) {
            counter += deltaTime;
        }
    }

    private bool WinCondition() {
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

}

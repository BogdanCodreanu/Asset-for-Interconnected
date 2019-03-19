using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutosavingController : MonoBehaviour {
    private static float counter;
    public float cooldown;

    private void Update() {
        if (GameController.GetGameState() == GameController.GameState.CREATING && !GameController.pausedInput) {
            counter += Time.deltaTime;
            if (counter >= cooldown) {
                Autosave();
            }
        }
    }

    private void Autosave() {
        counter = 0;
        SaveMachine.SaveToFile("Autosaved machine", SaveMachine.SaveAndGet(true));
        InfoUI.InformationalMoving("Autosaved.", 2);
        GameProgressionController.SaveToFile();
    }
}

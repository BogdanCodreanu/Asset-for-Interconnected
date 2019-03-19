using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public float timeBetweenObjectives = 1;
    private float counter;
    private bool counting;
    private bool finished;

    public TutorialObjective[] objectives;
    private TutorialObjective currentActiveObject;
    private int currentIndex;

    private void Start() {
        SetCurrentObjective(0);
    }

    public void NextTutorial() {
        if (currentActiveObject) {
            currentActiveObject.OnFinishObjective();
        }
        counting = true;
        counter = 0f;
    }

    private void Update() {
        if (!counting) {
            currentActiveObject.Tick();
        } else {
            counter += Time.deltaTime;
            if (counter >= timeBetweenObjectives) {
                SetCurrentObjective(currentIndex + 1);
                counting = false;
            }
        }
    }

    private void SetCurrentObjective(int index) {
        if (!finished) {
            if (index != objectives.Length) {
                currentActiveObject = objectives[index];
                currentActiveObject.SetTutorialController(this);
                currentActiveObject.OnStartedObjective();
                currentIndex = index;
            }
            else {
                finished = true;
                OnFinishedTutorial();
            }
        }
    }

    private void OnFinishedTutorial() {
        InfoUIPanelPopup.ButtonAction[] actions = new InfoUIPanelPopup.ButtonAction[2];
        actions[0] = new InfoUIPanelPopup.ButtonAction("Return to Main Menu", delegate { MainMenuReturn(); });
        actions[1] = new InfoUIPanelPopup.ButtonAction("Go to Next Tutorial", delegate { InfoUI.InformationalMoving("Next Tutorial not done yet"); });
        InfoUI.PanelPopup("Tutorial Finished", "Would you like to return to Main Menu or go to Next Tutorial that explains the game's shortcuts?", actions, true);
    }
    private void MainMenuReturn() {
        SceneController.GoToMainMenu();
    }
}

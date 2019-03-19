using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PanelSpawnerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Animator anim;
    public bool onScreen;
    private bool overMe;

    public ButtonSpawner buttonSpawnPrefab;

    public GridLayoutGroup gridLayoutGrp;
    private SpawningParts spawningParts;

    private static ButtonSpawner[] buttonsSpawned;

    private bool finishedExittingAnim;

    private static PanelSpawnerUI thisPanelSpawner;

    private void Awake() {
        anim = GetComponent<Animator>();
        finishedExittingAnim = true;
        thisPanelSpawner = this;
    }
    public void SetSpawningParts(SpawningParts sp) {
        spawningParts = sp;
    }

    public void EnterScreen() {
        if (GameController.GetGameState() == GameController.GameState.CREATING && finishedExittingAnim) {
            onScreen = true;
            anim.SetBool("EnterScreen", onScreen);
        }
    }
    

    public void ExitScreen() {
        if (GameController.GetGameState() == GameController.GameState.CREATING && onScreen) {
            finishedExittingAnim = false;
            onScreen = false;

            anim.SetBool("EnterScreen", onScreen);
        }
    }

    public void FinishExittingAnimation() {
        finishedExittingAnim = true;
    }

    public static void GameStarted() {
        if (thisPanelSpawner)
            thisPanelSpawner.ExitScreen();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        SelectingParts.mouseOverSpawningPanel = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        SelectingParts.mouseOverSpawningPanel = false;
    }

    public void CreateGrid(List<MechanicalPart> spawns) {
        buttonsSpawned = new ButtonSpawner[spawns.Count];
        int i = 0;
        foreach (MechanicalPart m in spawns) {
            buttonsSpawned[i] = Instantiate(buttonSpawnPrefab.gameObject,
                gridLayoutGrp.transform).GetComponent<ButtonSpawner>();
            buttonsSpawned[i].AssignJob(spawningParts, i, spawns[i]);
            i++;
        }
    }

    public void DestroyCurrentGrid() {
        foreach (ButtonSpawner b in buttonsSpawned) {
            Destroy(b.gameObject);
        }
    }

}

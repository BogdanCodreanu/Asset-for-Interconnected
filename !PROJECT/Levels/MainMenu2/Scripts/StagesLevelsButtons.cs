using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagesLevelsButtons : MonoBehaviour {
    private const float panelCooldownChanging = .5f;

    public Animator levelsPanelAnimator;
    public Button backButton;
    public Transform stagesButtonHolder;
    public StageButton stageButtonPrefab;
    public Transform levelsButtonsPanelHolder;

    private DatabaseSceneInfo databaseScene;
    private LevelsButtonsHolder activeLevelsButtons;

    private List<StageButton> stageButtons;

    private void Awake() {
        backButton.onClick.AddListener(delegate { ExitScreen(); CloseAnyLevelsButtonsHolder(null); activeLevelsButtons = null; });
        databaseScene = Resources.Load("Database Scenes Info") as DatabaseSceneInfo;

        if (!databaseScene) {
            InfoUIPanelPopup.ButtonAction[] actions = new InfoUIPanelPopup.ButtonAction[1];
            actions[0] = new InfoUIPanelPopup.ButtonAction("Ok", delegate { Application.Quit(); });
            InfoUI.PanelPopup("Database Error", "Unable to load database by stagesLevelsButtons.", actions, false);
        }
        stageButtons = new List<StageButton>();
        GenerateStagesButtons();
    }

    private void GenerateStagesButtons() {
        StageButton spawn;
        foreach (StageInfo stage in databaseScene.stagesInfo) {

            spawn = Instantiate(stageButtonPrefab.gameObject, stagesButtonHolder).GetComponent<StageButton>();
            spawn.Init(this, stage, GetScenesWithStage(stage), levelsButtonsPanelHolder);
            stageButtons.Add(spawn);
        }
    }

    //public void DisableAllStageButtons() {
    //    foreach (StageButton button in stageButtons) {
    //        button.DisableButtonAction();
    //    }
    //}
    //public void EnableAllStageButtons() {
    //    foreach (StageButton button in stageButtons) {
    //        button.DisableButtonAction();
    //    }
    //}

    private List<SceneInfo> GetScenesWithStage(StageInfo stage) {
        List<SceneInfo> scenes = new List<SceneInfo>();
        foreach (SceneInfo si in databaseScene.scenesInfo) {
            if (si.stageIndex == stage.stageIndex)
                scenes.Add(si);
        }
        return scenes;
    }
    public void EnterScreen() {
        levelsPanelAnimator.SetBool("OnScreen", true);
        activeLevelsButtons = null;
    }
    public void ExitScreen() {
        levelsPanelAnimator.SetBool("OnScreen", false);
    }

    public void SetActiveLevelsButtons(LevelsButtonsHolder lbHolder) {
        lbHolder.ExecuteFunctionWithDelay((activeLevelsButtons == null) ? 0 : panelCooldownChanging, delegate { lbHolder.EnterScreen(); });

        activeLevelsButtons = lbHolder;
    }
    public void CloseAnyLevelsButtonsHolder(LevelsButtonsHolder dontCloseMe) {
        if (activeLevelsButtons) {
            if (activeLevelsButtons != dontCloseMe) {
                activeLevelsButtons.StopAllCoroutines();
                activeLevelsButtons.ExitScreen();
            }
        }
    }
}

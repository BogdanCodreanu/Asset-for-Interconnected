using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabasesManager : MonoBehaviour {
    private static SceneInfo currentSceneInfo;
    private static StageInfo currentStage;

    private void Awake() {
        DatabaseSceneInfo databaseScene = Resources.Load("Database Scenes Info") as DatabaseSceneInfo;

        if (!databaseScene) {
            InfoUIPanelPopup.ButtonAction[] actions = new InfoUIPanelPopup.ButtonAction[1];
            actions[0] = new InfoUIPanelPopup.ButtonAction("Ok", delegate { SceneController.GoToMainMenu(); });
            InfoUI.PanelPopup("Database Error", "Unable to load Scenes database", actions, false);
        }

        //currentSceneInfo = GetCurrentSceneInfo(databaseScene);
        currentSceneInfo = SetCurrentSceneRef(databaseScene);
        if (currentSceneInfo == null) {
            InfoUIPanelPopup.ButtonAction[] actions = new InfoUIPanelPopup.ButtonAction[1];
            actions[0] = new InfoUIPanelPopup.ButtonAction("Ok", delegate { SceneController.GoToMainMenu(); });
            InfoUI.PanelPopup("Database Error", "Current scene loaded from database is null", actions, false);
        }
        else {
            currentStage = GetCurrentStageInfo(databaseScene);
        }
    }

    private StageInfo GetCurrentStageInfo(DatabaseSceneInfo db) {
        StageInfo outer = new StageInfo() {
            stageIndex = currentSceneInfo.stageIndex,
            stageName = db.stagesInfo[currentSceneInfo.stageIndex].stageName
        };
        return outer;
    }
    private SceneInfo GetCurrentSceneInfo(DatabaseSceneInfo db) {
        SceneInfo good = null;
        foreach (SceneInfo si in db.scenesInfo) {
            if (si.thisSceneName == SceneManager.GetActiveScene().name) {
                good = si;
                break;
            }
        }
        if (good == null)
            return good;
        SceneInfo outer = new SceneInfo(good.thisSceneName) {
            nextSceneName = good.nextSceneName,
            creationBoundsLimitCorner = good.creationBoundsLimitCorner,
            topRightCameraLimit = good.topRightCameraLimit,
            bottomLeftCameraLimit = good.bottomLeftCameraLimit
        };

        outer.avaliableMechs = new List<MechanicalPart>();
        if (good.avaliableMechs != null) {
            foreach (MechanicalPart m in good.avaliableMechs) {
                outer.avaliableMechs.Add(m);
            }
        }
        return outer;
    }
    private SceneInfo SetCurrentSceneRef(DatabaseSceneInfo db) {
        SceneInfo good = null;
        foreach (SceneInfo si in db.scenesInfo) {
            if (SceneManager.GetActiveScene().name == si.thisSceneName) {
                return si;
            }
        }
        return good;
    }

    public static bool IsValidScene() {
        return currentSceneInfo != null;
    }

    public static Vector3 GetCameraTopRightCorner() {
        if (currentSceneInfo != null) {
            return currentSceneInfo.topRightCameraLimit;
        }
        return Vector3.one * Mathf.Infinity;
    }
    public static Vector3 GetCameraBotLeftCorner() {
        if (currentSceneInfo != null) {
            return currentSceneInfo.bottomLeftCameraLimit;
        }
        return Vector3.one * Mathf.Infinity * -1;
    }
    public static Vector3 GetCreationBoundsCorner() {
        if (currentSceneInfo != null) {
            return currentSceneInfo.creationBoundsLimitCorner;
        }
        return Vector3.one * 10000f;
    }
    public static List<MechanicalPart> GetAvaliableMechs() {
        if (currentSceneInfo != null) {
            return currentSceneInfo.avaliableMechs;
        }
        return new List<MechanicalPart>();
    }
    public static string GetNextSceneName() {
        if (currentSceneInfo != null) {
            if (string.IsNullOrEmpty(currentSceneInfo.nextSceneName))
                return SceneController.MainMenuSceneName;
            return currentSceneInfo.nextSceneName;
        }
        return SceneController.MainMenuSceneName;
    }
    public static string GetSceneNickname() {
        if (!string.IsNullOrEmpty(currentSceneInfo.nickName))
            return currentSceneInfo.nickName;
        return currentSceneInfo.thisSceneName;
    }
    public static string GetStageName() {
        if (string.IsNullOrEmpty(currentStage.stageName))
            return "NULL STAGE";
        return currentStage.stageName;
    }
    public static string GetObjectiveText() {
        if (string.IsNullOrEmpty(currentSceneInfo.objectiveMissionText))
            return "";
        return currentSceneInfo.objectiveMissionText;
    }
}

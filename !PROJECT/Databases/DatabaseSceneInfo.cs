using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabaseSceneInfo : ScriptableObject {

    public List<SceneInfo> scenesInfo;
    public List<StageInfo> stagesInfo;
}

[System.Serializable]
public class SceneInfo {
    public SceneInfo(string name) {
        thisSceneName = name;
    }

    public string nickName;
    public string thisSceneName;
    public string nextSceneName;
    public int stageIndex;
    public Vector3 creationBoundsLimitCorner;
    public Vector3 topRightCameraLimit;
    public Vector3 bottomLeftCameraLimit;
    public List<MechanicalPart> avaliableMechs;
    public string objectiveMissionText;
}

[System.Serializable]
public class StageInfo {
    public int stageIndex;
    public string stageName;
}


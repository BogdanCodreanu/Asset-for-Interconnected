using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabaseSingleScene : ScriptableObject {
    public string thisSceneName;
    public string nickName;
    public string nextSceneName;
    public string objectiveMissionText;
    public int stageIndex;
    public DatabaseStages stages;
    public Vector3 creationBoundsLimitCorner;
    public Vector3 topRightCameraLimit;
    public Vector3 bottomLeftCameraLimit;
    public List<MechanicalPart> rewardedMechs;
}

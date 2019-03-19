using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabaseStages : ScriptableObject {
    public List<SingleStageInfo> stages;
}

[System.Serializable]
public struct SingleStageInfo {
    public string name;
}

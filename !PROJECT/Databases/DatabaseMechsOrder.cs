using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabaseMechsOrder : ScriptableObject {
    public List<MechanicalPriority> mechsOrder;
}

[System.Serializable]
public class MechanicalPriority {
    public MechanicalPriority(MechanicalPart mech) {
        this.mech = mech;
    }

    public MechanicalPart mech;
    public int priority;
}

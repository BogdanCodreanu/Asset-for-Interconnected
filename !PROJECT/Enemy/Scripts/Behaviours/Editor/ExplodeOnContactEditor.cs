using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExplodeOnContact))]
public class ExplodeOnContactEditor : Editor {

    protected virtual void OnSceneGUI() {
        ExplodeOnContact tar = (ExplodeOnContact)target;

        Handles.color = new Color(1f, .2f, 0f, .2f);
        Handles.DrawSolidDisc(tar.transform.position, Vector3.forward, tar.explodeRadius);
        Handles.color = Color.white;
    }
}

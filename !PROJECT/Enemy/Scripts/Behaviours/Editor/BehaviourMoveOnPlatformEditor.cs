using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BehaviourMoveOnPlatform))]
public class BehaviourMoveOnPlatformEditor : Editor {

    Vector3 newOnGroundPoint;
    Vector3 newInFrontPoint;
    Vector3 newLedgePoint;

    protected virtual void OnSceneGUI() {
        BehaviourMoveOnPlatform tar = (BehaviourMoveOnPlatform)target;

        newOnGroundPoint = tar.transform.TransformPoint(tar.onGroundPoint);
        EditorGUI.BeginChangeCheck();
        newOnGroundPoint = Handles.PositionHandle(newOnGroundPoint, Quaternion.identity);
        Handles.color = new Color(0, .6f, 1f, .5f);
        Handles.DrawSolidDisc(newOnGroundPoint, Vector3.forward, tar.detectionRadius);
        Handles.color = Color.white;
        
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(tar, "Changed On Ground Point");
            tar.onGroundPoint = newOnGroundPoint - tar.transform.position;
        }

        newInFrontPoint = tar.transform.TransformPoint(tar.inFrontPoint);
        EditorGUI.BeginChangeCheck();
        newInFrontPoint = Handles.PositionHandle(newInFrontPoint, Quaternion.identity);
        Handles.color = new Color(0, .6f, 1f, .5f);
        Handles.DrawSolidDisc(newInFrontPoint, Vector3.forward, tar.detectionRadius);
        Handles.color = Color.white;

        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(tar, "Changed In Front Point");
            tar.inFrontPoint = newInFrontPoint - tar.transform.position;
        }

        if (!tar.fallFromLedges) {
            newLedgePoint = tar.transform.TransformPoint(tar.inLedgePoint);
            EditorGUI.BeginChangeCheck();
            newLedgePoint = Handles.PositionHandle(newLedgePoint, Quaternion.identity);
            Handles.color = new Color(0, .6f, 1f, .5f);
            Handles.DrawSolidDisc(newLedgePoint, Vector3.forward, tar.detectionRadius);
            Handles.color = Color.white;

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(tar, "Changed On Ledge Point");
                tar.inLedgePoint = newLedgePoint - tar.transform.position;
            }
        }
    }
    
}

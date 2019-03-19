using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnviormentMovingPlatform))]
public class EnviormentMovingPlatformEditor : Editor {

    protected virtual void OnSceneGUI() {
        EnviormentMovingPlatform tar = (EnviormentMovingPlatform)target;

        Handles.color = new Color(1, 1, 0, 0.3f);
        foreach (Vector3 v in tar.movingPositions) {
            Handles.DrawSolidDisc(v, Vector3.forward, 0.3f);
        }

        for (int i = 0; i < tar.movingPositions.Length - 1; i++) {
            Handles.DrawLine(tar.movingPositions[i], tar.movingPositions[i + 1]);

        }
        if (tar.circular) {
            Handles.DrawLine(tar.movingPositions[tar.movingPositions.Length - 1], tar.movingPositions[0]);
        }
    }

    public override void OnInspectorGUI() {
        EnviormentMovingPlatform tar = (EnviormentMovingPlatform)target;
        base.OnInspectorGUI();
        for (int i = 0; i < tar.movingPositions.Length; i++) {
            if (GUILayout.Button("Reset position to " + i + " point")) {
                tar.transform.position = tar.movingPositions[i];
            }
        }
    }
}
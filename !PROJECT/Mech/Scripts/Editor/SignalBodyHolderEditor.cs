using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SignalBodyHolder))]
public class SignalBodyHolderEditor : Editor {
    Vector3 newSpawningPos;

    private void OnEnable() {
        newSpawningPos = (target as SignalBodyHolder).transform.position;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        SignalBodyHolder tar = target as SignalBodyHolder;
        if (tar.holderRenderer == null) {
            EditorGUILayout.BeginHorizontal();
            newSpawningPos = EditorGUILayout.Vector3Field("Spawning Point", newSpawningPos);
            if (GUILayout.Button("Spawn")) {
                SpawnHolder();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    protected virtual void OnSceneGUI() {
        SignalBodyHolder tar = target as SignalBodyHolder;
        if (tar.holderRenderer == null) {

            newSpawningPos = Handles.PositionHandle(newSpawningPos, Quaternion.identity);
            Handles.color = new Color(0, .5f, 1f, .3f);

            if (tar.usedFor == SignalBodyHolder.InputOrOutput.Output)
                Handles.CubeHandleCap(0, newSpawningPos, Quaternion.identity, SignalBodyHolder.bodyHolderSize, EventType.Repaint);
            if (tar.usedFor == SignalBodyHolder.InputOrOutput.Input)
                Handles.SphereHandleCap(0, newSpawningPos, Quaternion.identity, SignalBodyHolder.bodyHolderSize, EventType.Repaint);

            Handles.color = Color.white;
        }
    }

    private void SpawnHolder() {
        SignalBodyHolder tar = target as SignalBodyHolder;
        MechanicalOperational mech = tar.GetComponent<MechanicalOperational>();
        GameObject spawn = new GameObject("Signal Holder -" + tar.usedFor + "- " + mech.mechName);
        spawn.transform.position = newSpawningPos;
        spawn.transform.localScale = Vector3.one * SignalBodyHolder.bodyHolderSize;
        spawn.transform.parent = tar.transform;
        SpriteRenderer sp = spawn.AddComponent<SpriteRenderer>();
        sp.sortingLayerName = "Wire renderer Holders";
        tar.holderRenderer = sp;
        Selection.activeGameObject = spawn;
    }
}

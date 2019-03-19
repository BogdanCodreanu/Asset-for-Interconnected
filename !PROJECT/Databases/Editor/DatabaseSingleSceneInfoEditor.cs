using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DatabaseSingleScene))]
public class DatabaseSingleSceneInfoEditor : Editor {
    DatabaseSingleScene scene;
    Scene currentScene;

    private void OnEnable() {
        scene = target as DatabaseSingleScene;
        currentScene = SceneManager.GetActiveScene();

        if (string.IsNullOrEmpty(scene.thisSceneName)) {
            scene.thisSceneName = currentScene.name;
        }
        serializedObject.ApplyModifiedProperties();

        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }
    void OnDisable() {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        //GUI.color = Color.cyan;
        EditorGUILayout.BeginHorizontal("box");
        GUI.color = Color.white;

        if (scene.stages != null) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stage", GUILayout.Width(60));
            scene.stageIndex = EditorGUILayout.IntSlider(scene.stageIndex, 0, scene.stages.stages.Count - 1);
            EditorGUILayout.LabelField("- " + scene.stages.stages[scene.stageIndex].name, GUILayout.Width(100f));
            EditorGUILayout.EndHorizontal();

        }

        EditorGUILayout.EndVertical();

        if (scene.stages == null) {
            EditorGUILayout.HelpBox("Assign Stages database !", MessageType.Error);
        }
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
        }
    }


    Vector3 creationBoundsCorner;
    Vector3 topRightCameraCorner;
    Vector3 bottomLeftCameraCorner;
    public void OnSceneGUI(SceneView sceneview) {
        EditorGUI.BeginChangeCheck();
        creationBoundsCorner = Handles.PositionHandle(creationBoundsCorner, Quaternion.identity);
        creationBoundsCorner.z = 0;
        Handles.Label(creationBoundsCorner + Vector3.one, "Creation Bounds Corner", "box");
        Handles.color = new Color(1, 0, 0, 0.6f);
        Handles.DrawWireCube(Vector3.zero, new Vector3(creationBoundsCorner.x * 2, creationBoundsCorner.y * 2, 1));

        if (0 >= creationBoundsCorner.y || 0 >= creationBoundsCorner.x) {
            Handles.color = new Color(1, 0, 0, 1f);
            Handles.Label(Vector3.zero, "Wrong placed corners", "box");
        }

        topRightCameraCorner = Handles.PositionHandle(topRightCameraCorner, Quaternion.identity);
        topRightCameraCorner.z = 0;
        bottomLeftCameraCorner = Handles.PositionHandle(bottomLeftCameraCorner, Quaternion.identity);
        bottomLeftCameraCorner.z = 0;
        Handles.color = new Color(1, 1, 0, 0.6f);
        GUI.backgroundColor = new Color(1, 1, 1, 1);
        GUI.contentColor = Color.yellow;
        Handles.Label(topRightCameraCorner + Vector3.one, "Camera Limits", "box");
        Handles.DrawWireCube((topRightCameraCorner + bottomLeftCameraCorner) * 0.5f, new Vector3(topRightCameraCorner.x - bottomLeftCameraCorner.x, topRightCameraCorner.y - bottomLeftCameraCorner.y, 1));
        if (bottomLeftCameraCorner.y > topRightCameraCorner.y || bottomLeftCameraCorner.x > topRightCameraCorner.x) {
            Handles.color = new Color(1, 0, 0, 1f);
            Handles.Label((topRightCameraCorner + bottomLeftCameraCorner) * 0.5f, "Wrong placed corners", "box");
        }

        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
            scene.bottomLeftCameraLimit = bottomLeftCameraCorner;
            scene.topRightCameraLimit = topRightCameraCorner;
            scene.creationBoundsLimitCorner = creationBoundsCorner;
        }

    }
}

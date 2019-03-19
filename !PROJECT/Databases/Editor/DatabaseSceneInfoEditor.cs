using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DatabaseSceneInfo))]
public class DatabaseSceneInfoEditor : Editor {

    DatabaseSceneInfo db;
    Vector3 creationBoundsCorner;
    Vector3 topRightCameraCorner;
    Vector3 bottomLeftCameraCorner;
    SceneInfo currentSceneInfo;

    List<MechanicalPart> copiedList = null;

    SceneInfo cutScene;
    SiInfos cutSiInfo;

    SceneInfo deletedScene;
    SiInfos deletedInfo;

    bool showingStagesInfos;

    private class SiInfos {
        public SiInfos(SceneInfo f) { father = f; }
        public SceneInfo father;
        public bool openedMechPartsPanel = false;
        public MechanicalPart newAdded = null;
        public bool openedPanel = false;
    }
    List<SiInfos> siInfos;

    private void OnEnable() {
        db = target as DatabaseSceneInfo;
        creationBoundsCorner = Vector3.zero;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        bool openCurrentScene = false;
        FindCurrentScene();
        if (currentSceneInfo != null) {
            creationBoundsCorner = currentSceneInfo.creationBoundsLimitCorner;
            topRightCameraCorner = currentSceneInfo.topRightCameraLimit;
            bottomLeftCameraCorner = currentSceneInfo.bottomLeftCameraLimit;
            openCurrentScene = true;
        }
        siInfos = new List<SiInfos>();
        foreach (SceneInfo si in db.scenesInfo) {
            siInfos.Add(new SiInfos(si));
            if (openCurrentScene) {
                if (si == currentSceneInfo) {
                    siInfos[siInfos.Count - 1].openedPanel = true;
                    openCurrentScene = false;
                }
            }
        }
        copiedList = null;
        cutScene = null;
        cutSiInfo = null;
        deletedScene = null;
        deletedInfo = null;

        showingStagesInfos = false;
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;

            if (cutScene != null) {
                db.scenesInfo.Insert(0, cutScene);
                siInfos.Insert(0, cutSiInfo);
                cutScene = null;
                cutSiInfo = null;
            }
        }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        DisplayStages();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Minimize All", GUILayout.Width(100))) {
            foreach (SiInfos s in siInfos)
                s.openedPanel = false;
        }
        if (GUILayout.Button("Maximize All", GUILayout.Width(100))) {
            foreach (SiInfos s in siInfos)
                s.openedPanel = true;
        }
        if (deletedScene != null) {
            GUI.color = Color.red;
            if (GUILayout.Button("Undo Deletion", GUILayout.Width(200))) {
                db.scenesInfo.Insert(0, deletedScene);
                siInfos.Insert(0, deletedInfo);
                deletedInfo = null;
                deletedScene = null;
                FindCurrentScene();
            }
            GUI.color = Color.white;
        }
        EditorGUILayout.EndHorizontal();

        if (currentSceneInfo == null) {
            EditorGUILayout.HelpBox("No scene info for current scene", MessageType.Warning);
        }

        if (db.scenesInfo != null) {
            int i = 0;

            if (cutScene != null) {
                DisplayPasteFromCut(i);
            }

            foreach (SceneInfo si in db.scenesInfo) {
                if (!DisplayOneSceneInfo(si, siInfos[i])) {
                    break;
                }
                i++;

                if (cutScene == null) {
                    EditorGUILayout.Space();
                }
                else {
                    if (DisplayPasteFromCut(i))
                        break;
                }
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Item")) {
                db.scenesInfo.Add(new SceneInfo(SceneManager.GetActiveScene().name));
                siInfos.Add(new SiInfos(db.scenesInfo[db.scenesInfo.Count - 1]));
                if (currentSceneInfo == null) {
                    FindCurrentScene();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        else {

            EditorGUILayout.HelpBox("List is null", MessageType.Info);
            if (GUILayout.Button("Create new List")) {
                db.scenesInfo = new List<SceneInfo>();

            }
        }
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
        }
    }

    private void FindCurrentScene() {
        currentSceneInfo = null;
        foreach (SceneInfo si in db.scenesInfo) {
            if (si.thisSceneName == SceneManager.GetActiveScene().name) {
                currentSceneInfo = si;
                creationBoundsCorner = si.creationBoundsLimitCorner;
                topRightCameraCorner = si.topRightCameraLimit;
                bottomLeftCameraCorner = si.bottomLeftCameraLimit;
                break;
            }
        }
    }

    private bool DisplayPasteFromCut(int index) {
        GUI.color = new Color(1, .4f, 0, 1f);
        if (GUILayout.Button("Paste " + cutScene.thisSceneName + " here")) {
            db.scenesInfo.Insert(index, cutScene);
            siInfos.Insert(index, cutSiInfo);
            cutScene = null;
            cutSiInfo = null;
            GUI.color = Color.white;
            return true;
        }
        GUI.color = Color.white;
        return false;
    }
    private bool DisplaySceneHeader(SceneInfo si, SiInfos info) {
        GUI.color = Color.cyan;
        EditorGUILayout.BeginHorizontal("box");
        GUI.color = Color.red;
        if (GUILayout.Button("X", GUILayout.Width(20))) {
            deletedScene = si;
            deletedInfo = info;
            db.scenesInfo.Remove(si);
            siInfos.Remove(info);
            FindCurrentScene();
            return false;
        }
        GUI.color = new Color(1, .4f, 0, 1f);
        if (cutScene == null) {
            if (GUILayout.Button("Cut", GUILayout.Width(30))) {
                cutSiInfo = info;
                cutScene = si;
                db.scenesInfo.Remove(si);
                siInfos.Remove(info);
                return false;
            }
        }
        else {
            EditorGUILayout.LabelField("Cut", GUILayout.Width(30));
        }
        GUI.color = Color.white;

        if (GUILayout.Button((info.openedPanel) ? "_" : "↨", GUILayout.Width(40))) {
            info.openedPanel = !info.openedPanel;
        }

        si.thisSceneName = EditorGUILayout.TextField("Current Scene Name", si.thisSceneName);
        if (db.stagesInfo != null) {
            if (si.stageIndex < db.stagesInfo.Count && si.stageIndex >= 0) {
                GUI.color = new Color(0, .2f, 1, .6f);
                EditorGUILayout.BeginHorizontal("box");
                GUI.color = Color.white;
                EditorGUILayout.LabelField("- " + db.stagesInfo[si.stageIndex].stageName, GUILayout.Width(100f));
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        return true;
    }
    private bool DisplayOneSceneInfo(SceneInfo si, SiInfos info) {
        if (si == currentSceneInfo)
            GUI.color = Color.yellow;
        else
            GUI.color = new Color(.5f, .5f, 0, 1f);
        EditorGUILayout.BeginVertical("box");
        GUI.color = Color.white;

        if (!DisplaySceneHeader(si, info))
            return false;

        if (info.openedPanel) {

            si.nickName = EditorGUILayout.TextField("Scene Nickname", si.nickName);
            si.nextSceneName = EditorGUILayout.TextField("Next Scene Name", si.nextSceneName);
            si.objectiveMissionText = EditorGUILayout.TextField("Objective Mission Text", si.objectiveMissionText);
            EditorGUILayout.BeginHorizontal();
            if (db.stagesInfo != null) {
                if (si.stageIndex < db.stagesInfo.Count && si.stageIndex >= 0) {
                    EditorGUILayout.LabelField("Stage: " + db.stagesInfo[si.stageIndex].stageName, GUILayout.Width(150f));
                }
                else {
                    EditorGUILayout.HelpBox("Stage index too big or too small", MessageType.Error);
                }
                si.stageIndex = EditorGUILayout.IntSlider(si.stageIndex, 0, db.stagesInfo.Count - 1);
                if (si.stageIndex < 0)
                    si.stageIndex = 0;
                if (si.stageIndex >= db.stagesInfo.Count)
                    si.stageIndex = db.stagesInfo.Count - 1;
            }
            else {
                EditorGUILayout.HelpBox("No stages List", MessageType.Error);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            si.creationBoundsLimitCorner = EditorGUILayout.Vector3Field("Creation Bounds limit corner", si.creationBoundsLimitCorner);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Camera Limits", GUILayout.Width(100));
            si.bottomLeftCameraLimit = EditorGUILayout.Vector3Field("", si.bottomLeftCameraLimit, GUILayout.Width(200));
            si.topRightCameraLimit = EditorGUILayout.Vector3Field("", si.topRightCameraLimit, GUILayout.Width(200));
            if (EditorGUI.EndChangeCheck()) {
                creationBoundsCorner = si.creationBoundsLimitCorner;
                topRightCameraCorner = si.topRightCameraLimit;
                bottomLeftCameraCorner = si.bottomLeftCameraLimit;
            }

            EditorGUILayout.EndHorizontal();

            DisplayAvaliableMechs(si, info);

        }
        EditorGUILayout.EndVertical();
        return true;
    }
    private void DisplayAvaliableMechs(SceneInfo si, SiInfos info) {
        GUI.color = new Color(.8f, 0, .9f, .7f);
        EditorGUILayout.BeginVertical("box");
        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Mech Panel." + ((si.avaliableMechs != null) ? "Mechs: " + si.avaliableMechs.Count : "")))
            info.openedMechPartsPanel = !info.openedMechPartsPanel;
        if (GUILayout.Button("Copy", GUILayout.Width(60)))  // copy
            copiedList = si.avaliableMechs;
        if (copiedList != null) {
            if (GUILayout.Button("Paste", GUILayout.Width(60))) {  // paste
                si.avaliableMechs = new List<MechanicalPart>();
                foreach (MechanicalPart m in copiedList)
                    si.avaliableMechs.Add(m);
            }
        }
        else {
            EditorGUILayout.LabelField("Paste", GUILayout.Width(60));
        }
        EditorGUILayout.EndHorizontal();

        if (info.openedMechPartsPanel) {
            info.newAdded = EditorGUILayout.ObjectField("Add New Mech", info.newAdded, typeof(MechanicalPart), false, GUILayout.Height(60)) as MechanicalPart;
            if (info.newAdded) {
                if (si.avaliableMechs == null)
                    si.avaliableMechs = new List<MechanicalPart>();

                si.avaliableMechs.Add(info.newAdded);
                info.newAdded = null;
            }

            if (si.avaliableMechs != null) {
                for (int i = 0; i < si.avaliableMechs.Count; i++) {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20))) {
                        si.avaliableMechs.Remove(si.avaliableMechs[i]);
                        break;
                    }
                    si.avaliableMechs[i] = EditorGUILayout.ObjectField("Mech[" + i + "]", si.avaliableMechs[i], typeof(MechanicalPart), false) as MechanicalPart;
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DisplayStages() {
        GUI.color = new Color(0, 0, 1, .3f);
        EditorGUILayout.BeginVertical("box");
        GUI.color = Color.white;
        if (db.stagesInfo == null) {
            if (GUILayout.Button("Create stages List")) {
                db.stagesInfo = new List<StageInfo>();
            }
        }
        else {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button((showingStagesInfos) ? "_" : "↨", GUILayout.Width(60))) {
                showingStagesInfos = !showingStagesInfos;
            }
            EditorGUILayout.LabelField("Stages");
            EditorGUILayout.EndHorizontal();
            if (showingStagesInfos) {
                GUI.color = new Color(0, 1, 1, .3f);
                EditorGUILayout.BeginVertical("box");
                GUI.color = Color.white;
                int stageIndex = 0;
                foreach (StageInfo stage in db.stagesInfo) {
                    EditorGUILayout.BeginHorizontal();
                    if (stageIndex == db.stagesInfo.Count - 1) {
                        GUI.color = Color.red;
                        if (GUILayout.Button("X", GUILayout.Width(20))) {
                            db.stagesInfo.Remove(stage);
                            break;
                        }
                    }
                    GUI.color = Color.white;
                    stage.stageIndex = stageIndex;
                    stage.stageName = EditorGUILayout.TextField("Stage index " + stage.stageIndex + ": ", stage.stageName);
                    EditorGUILayout.EndHorizontal();
                    stageIndex++;
                }
                if (GUILayout.Button("Create new Stage")) {
                    db.stagesInfo.Add(new StageInfo { stageIndex = db.stagesInfo.Count, stageName = "NEW STAGE" });
                }

                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndVertical();
    }

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
            if (currentSceneInfo != null) {
                EditorUtility.SetDirty(target);
                currentSceneInfo.bottomLeftCameraLimit = bottomLeftCameraCorner;
                currentSceneInfo.topRightCameraLimit = topRightCameraCorner;
                currentSceneInfo.creationBoundsLimitCorner = creationBoundsCorner;
            }
        }

    }
}

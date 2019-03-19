using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tutorial4MechsFrame))]
public class Tutorial4MechsFrameEditor : Editor {
    Camera spawnedCamera;
    Tutorial4MechsFrame frame;

    private void Awake() {
        frame = target as Tutorial4MechsFrame;
    }

    private void OnDestroy() {
        if (spawnedCamera)
            Debug.LogWarning("dont forget to destroy spawned Camera from tutorial: " + spawnedCamera.gameObject);
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (!spawnedCamera) {
            if (GUILayout.Button("Create test Camera")) {
                SpawnCamera();
            }
        } else {
            //EditorGUI.DrawPreviewTexture(new Rect(10, 400, 100, 100), spawnedCamera.targetTexture);
            MoveCamera();
        }
    }

    void SpawnCamera() {
        spawnedCamera = Instantiate(frame.cameraForMechsTutorials.gameObject, frame.transform.position, Quaternion.identity).GetComponent<Camera>();
        MoveCamera();
    }

    void MoveCamera() {
        spawnedCamera.transform.position = new Vector3(spawnedCamera.transform.position.x, spawnedCamera.transform.position.y, -frame.cameraDistance);
    }

}

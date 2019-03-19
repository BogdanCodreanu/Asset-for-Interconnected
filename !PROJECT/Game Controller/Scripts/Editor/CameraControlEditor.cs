//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(CameraControl))]
//public class CameraControlEditor : Editor {

//    protected virtual void OnSceneGUI() {
//        CameraControl example = (CameraControl)target;
        
//        EditorGUI.BeginChangeCheck();
//        Vector3 newTopRightCorner = Handles.PositionHandle(example.limitTopRight, Quaternion.identity);
//        if (EditorGUI.EndChangeCheck()) {
//            Undo.RecordObject(example, "Change Top Right Corner Bounds");
//            example.limitTopRight = newTopRightCorner;
//        }

//        EditorGUI.BeginChangeCheck();
//        Vector3 newBottomLeftCorner = Handles.PositionHandle(example.limitBottomLeft, Quaternion.identity);
//        if (EditorGUI.EndChangeCheck()) {
//            Undo.RecordObject(example, "Change Bottom Left Corner Bounds");
//            example.limitBottomLeft = newBottomLeftCorner;
//        }
//        Handles.color = new Color(0, 1, 0.3f, 0.6f);
//        Handles.DrawWireCube((newTopRightCorner + newBottomLeftCorner) * 0.5f, new Vector3(newTopRightCorner.x - newBottomLeftCorner.x, newTopRightCorner.y - newBottomLeftCorner.y, 1));

//        if (newBottomLeftCorner.y > newTopRightCorner.y || newBottomLeftCorner.x > newTopRightCorner.x) {
//            Handles.color = new Color(1, 0, 0, 1f);
//            Handles.Label((newTopRightCorner + newBottomLeftCorner) * 0.5f, "Wrong placed corners", "box");
//        }

//    }

//    //public override void OnInspectorGUI() {
//    //    base.OnInspectorGUI();
//    //    CreationBounds example = (CreationBounds)target;
//    //    EditorGUI.BeginChangeCheck();
//    //    float newTester = EditorGUILayout.FloatField("here", example.tester);
//    //    if (EditorGUI.EndChangeCheck()) {
//    //        Undo.RecordObject(example, "changed float");
//    //        example.tester = newTester;
//    //    }
//    //}
//}

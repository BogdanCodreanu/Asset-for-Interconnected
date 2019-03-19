//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(CreationBounds))]
//public class CreationBoundsEditor : Editor {

//    protected virtual void OnSceneGUI() {
//        CreationBounds example = (CreationBounds)target;
        
//        EditorGUI.BeginChangeCheck();
//        Vector3 newTopRightCorner = Handles.PositionHandle(example.topRightCorner, Quaternion.identity);
//        if (EditorGUI.EndChangeCheck()) {
//            Undo.RecordObject(example, "Change Top Right Corner Bounds");
//            example.topRightCorner = newTopRightCorner;
//            example.bottomLeftCorner = -newTopRightCorner;
//        }

//        //EditorGUI.BeginChangeCheck();
//        //Vector3 newBottomLeftCorner = Handles.PositionHandle(example.bottomLeftCorner, Quaternion.identity);
//        //if (EditorGUI.EndChangeCheck()) {
//        //    Undo.RecordObject(example, "Change Bottom Left Corner Bounds");
//        //    example.bottomLeftCorner = newBottomLeftCorner;
//        //}
//        //Handles.color = new Color(1, 0, 0, 0.6f);
//        //Handles.DrawWireCube((newTopRightCorner + newBottomLeftCorner) * 0.5f, new Vector3(newTopRightCorner.x - newBottomLeftCorner.x, newTopRightCorner.y - newBottomLeftCorner.y, 1));

//        //if (newBottomLeftCorner.y > newTopRightCorner.y || newBottomLeftCorner.x > newTopRightCorner.x) {
//        //    Handles.color = new Color(1, 0, 0, 1f);
//        //    Handles.Label((newTopRightCorner + newBottomLeftCorner) * 0.5f, "Wrong placed corners", "box");
//        //}

//        Handles.color = new Color(1, 0, 0, 0.6f);
//        Handles.DrawWireCube(Vector3.zero, new Vector3(newTopRightCorner.x * 2, newTopRightCorner.y * 2, 1));

//        if (0 >= newTopRightCorner.y || 0 >= newTopRightCorner.x) {
//            Handles.color = new Color(1, 0, 0, 1f);
//            Handles.Label(Vector3.zero, "Wrong placed corners", "box");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DatabaseStages))]
public class DatabaseStagesEditor : Editor {
    ReorderableList list;
    //DatabaseStages stages;


    private void OnEnable() {
        //stages = target as DatabaseStages;
        InitializeList();
    }

    private void InitializeList() {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("stages"), true, true, true, true);

        list.drawElementCallback =
         (Rect rect, int index, bool isActive, bool isFocused) => {
             var element = list.serializedProperty.GetArrayElementAtIndex(index);
             rect.y += 2;
             EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width - 150, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("name"), GUIContent.none);
             EditorGUI.LabelField(new Rect(rect.x + rect.width - 120, rect.y, 150, EditorGUIUtility.singleLineHeight), "Index: " + index);
         };

        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Stages");
        };

    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}

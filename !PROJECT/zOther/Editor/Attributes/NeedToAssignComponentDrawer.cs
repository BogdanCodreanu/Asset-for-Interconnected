using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(NeedToAssignComponentAttribute))]
public class NeedToAssignComponentDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property.objectReferenceValue == null) {
            GUI.color = new Color(1, .3f, .3f, 1f);
        }
        EditorGUI.PropertyField(position, property, label);
        GUI.color = Color.white;
    }
}

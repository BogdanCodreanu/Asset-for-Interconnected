using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PersonalGUI : Editor {

    //DatabaseMechsOrder db;
    //MechanicalPriority cutMech;
    //MechanicalPriority deletedMech;
    //MechanicalPart newMech;

    //private void OnEnable() {
    //    db = target as DatabaseMechsOrder;
    //    cutMech = null;
    //    deletedMech = null;
    //    newMech = null;
    //}

    //public override void OnInspectorGUI() {
    //    base.OnInspectorGUI();
    //    EditorGUI.BeginChangeCheck();
    //    //GUI.color = Color.cyan;

    //    //if (GUILayout.Button("Calculate Priorities")) {
    //    //    CalculatePriorities();
    //    //}
    //    //GUI.color = Color.white;

    //    if (deletedMech != null) {
    //        GUI.color = Color.red;
    //        if (GUILayout.Button("Undo Deletion", GUILayout.Width(200))) {
    //            db.mechsOrder.Insert(0, deletedMech);
    //            deletedMech = null;
    //        }
    //        GUI.color = Color.white;
    //    }

    //    DisplayList(new Color(1, 1, 0, .4f));
    //    //if (GUILayout.Button("Add New element")) {
    //    //    db.mechsOrder.Add(new MechanicalPriority());
    //    //}

    //    GUI.color = Color.cyan;
    //    newMech = EditorGUILayout.ObjectField("New Mechanical Part", newMech, typeof(MechanicalPart), false, GUILayout.Height(30f)) as MechanicalPart;
    //    GUI.color = Color.white;
    //    if (newMech != null) {
    //        db.mechsOrder.Add(new MechanicalPriority(newMech));
    //        newMech = null;
    //    }

    //    if (EditorGUI.EndChangeCheck()) {
    //        Debug.Log("Set dirty");
    //        CalculatePriorities();
    //        EditorUtility.SetDirty(target);
    //    }
    //}

    //private void CalculatePriorities() {
    //    for (int i = 0; i < db.mechsOrder.Count; i++) {
    //        db.mechsOrder[i].priority = (db.mechsOrder.Count - i);
    //    }
    //}

    //private void DisplayList(Color itemColor) {
    //    int pasteIndex = 0;
    //    PasteButton(pasteIndex);
    //    pasteIndex++;
    //    foreach (MechanicalPriority mp in db.mechsOrder) {
    //        GUI.color = itemColor;
    //        if (mp == cutMech) {
    //            GUI.color = new Color(1, .4f, 0, 1f);
    //            pasteIndex--;
    //        }
    //        EditorGUILayout.BeginVertical("box");
    //        EditorGUILayout.BeginHorizontal();

    //        GUI.color = Color.red;
    //        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20))) {
    //            if (mp == cutMech) {
    //                cutMech = null;
    //            }
    //            deletedMech = mp;
    //            db.mechsOrder.Remove(mp);
    //            return;
    //        }
    //        if (cutMech == null) {
    //            GUI.color = new Color(1, .4f, 0, 1f);
    //            if (GUILayout.Button("Cut", GUILayout.Width(30), GUILayout.Height(20))) {
    //                cutMech = mp;
    //            }
    //        }
    //        else {
    //            GUI.color = Color.white;
    //            EditorGUILayout.LabelField("Cut", EditorStyles.toolbarButton, GUILayout.Width(30), GUILayout.Height(20));
    //        }
    //        GUI.color = Color.white;
    //        DisplayMechPriorityHeader(mp);
    //        EditorGUILayout.EndHorizontal();
    //        DisplayMechPriority(mp);
    //        EditorGUILayout.EndVertical();
    //        if (PasteButton(pasteIndex)) {
    //            return;
    //        }
    //        pasteIndex++;
    //    }
    //}

    //private bool PasteButton(int i) {
    //    if (cutMech == null) {
    //        GUI.color = Color.white;
    //        EditorGUILayout.LabelField("Paste", EditorStyles.toolbarButton, GUILayout.Width(100), GUILayout.Height(20));
    //    }
    //    else {
    //        GUI.color = new Color(1, .4f, 0, 1f);
    //        if (GUILayout.Button("Paste", GUILayout.Width(100), GUILayout.Height(20))) {
    //            db.mechsOrder.Remove(cutMech);
    //            if (i > db.mechsOrder.Count)
    //                db.mechsOrder.Add(cutMech);
    //            else
    //                db.mechsOrder.Insert(i, cutMech);

    //            cutMech = null;
    //            return true;
    //        }
    //    }
    //    GUI.color = Color.white;
    //    return false;
    //}

    //private void DisplayMechPriority(MechanicalPriority mechPriority) {
    //    mechPriority.mech = EditorGUILayout.ObjectField("Mech", mechPriority.mech, typeof(MechanicalPart), false) as MechanicalPart;
    //}
    //private void DisplayMechPriorityHeader(MechanicalPriority mechPr) {
    //    EditorGUILayout.BeginHorizontal();
    //    EditorGUILayout.LabelField((mechPr.mech) ? mechPr.mech.mechName : "NULL", EditorStyles.boldLabel);
    //    EditorGUILayout.LabelField(": " + mechPr.priority, EditorStyles.boldLabel);
    //    EditorGUILayout.EndHorizontal();
    //}
}

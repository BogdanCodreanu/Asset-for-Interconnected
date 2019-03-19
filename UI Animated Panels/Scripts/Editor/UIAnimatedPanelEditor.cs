using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Razziel.AnimatedPanels {
    [CustomEditor(typeof(UIAnimatedPanel))]
    [CanEditMultipleObjects]
    public class UIAnimatedPanelEditor : Editor {
        UIAnimatedPanel panel;
        SerializedProperty content, openingMode, toggledByButtons, initialHide, additionalFadingElements;

        private void OnEnable() {
            panel = target as UIAnimatedPanel;
            content = serializedObject.FindProperty("content");
            openingMode = serializedObject.FindProperty("openingMode");
            toggledByButtons = serializedObject.FindProperty("toggledByButtons");
            initialHide = serializedObject.FindProperty("initialHide");
            additionalFadingElements = serializedObject.FindProperty("additionalFadingElements");
        }

        public override void OnInspectorGUI() {
            //base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(content);
            EditorGUILayout.PropertyField(toggledByButtons, true);
            EditorGUILayout.PropertyField(openingMode);
            EditorGUILayout.PropertyField(additionalFadingElements, true);

            GUI.color = Color.green;
            panel.root = EditorGUILayout.Toggle("Root Panel", panel.root);
            if (panel.root)
                EditorGUILayout.PropertyField(initialHide);
            GUI.color = Color.white;

            if (!panel.root) {
                GUI.color = new Color(1, .5f, 0, 1f);
                panel.grabAnimationFromRoot = EditorGUILayout.Toggle("Grab Animation From Root", panel.grabAnimationFromRoot);
                GUI.color = Color.white;
                if (panel.grabAnimationFromRoot) {
                    panel.appearTime.grabFromRoot = true;
                    panel.customFadeAwayDuration = true;
                    panel.fadeTime.grabFromRoot = true;
                }
            }

            if (!panel.root) {
                EditorGUILayout.BeginVertical("box");
                GUI.color = Color.white;
                EditorGUILayout.LabelField("Panel Appear Speed", EditorStyles.boldLabel);

                if (panel.grabAnimationFromRoot)
                    GUI.color = new Color(1, .5f, 0, 1f);

                panel.appearTime.grabFromRoot = EditorGUILayout.Toggle("Grab from Root", panel.appearTime.grabFromRoot);
                if (!panel.appearTime.grabFromRoot) {
                    panel.appearTime.customTime = EditorGUILayout.FloatField("Custom Appear Time", panel.appearTime.customTime);
                }

                GUI.color = Color.white;
            }
            else {
                EditorGUILayout.BeginVertical("box");
                panel.appearTime.customTime = EditorGUILayout.FloatField("Animation Time", panel.appearTime.customTime);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");

            if (panel.grabAnimationFromRoot)
                GUI.color = new Color(1, .5f, 0, 1f);
            panel.customFadeAwayDuration = EditorGUILayout.Toggle("Custom Fade Duration", panel.customFadeAwayDuration);
            GUI.color = Color.white;

            if (panel.customFadeAwayDuration) {
                if (!panel.root) {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField("Panel Fade Speed", EditorStyles.boldLabel);

                    if (panel.grabAnimationFromRoot)
                        GUI.color = new Color(1, .5f, 0, 1f);

                    panel.fadeTime.grabFromRoot = EditorGUILayout.Toggle("Grab from Root", panel.fadeTime.grabFromRoot);
                    if (!panel.fadeTime.grabFromRoot) {
                        panel.fadeTime.customTime = EditorGUILayout.FloatField("Custom Fade Time", panel.fadeTime.customTime);
                    }

                    GUI.color = Color.white;
                }
                else {
                    EditorGUILayout.BeginVertical("box");
                    panel.fadeTime.customTime = EditorGUILayout.FloatField("Fade Time", panel.fadeTime.customTime);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
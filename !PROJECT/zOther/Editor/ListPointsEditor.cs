using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ListPointsEditor {
    private float pointRadiusOnScene = .2f;
    private List<Vector3> linePoints;
    private Vector3 mousePos;
    private float alignError = .5f;

    public ListPointsEditor() {
        linePoints = new List<Vector3>();
        selectedPointIndex = -1;
    }

    public void SetInitialPositions(Vector3[] initialPoints, bool isLocalPosition = false, Transform parent = null) {
        linePoints = new List<Vector3>();
        for (int i = 0; i < initialPoints.Length; i++) {
            linePoints.Add(initialPoints[i]);
        }

        if (isLocalPosition) {
            if (!parent) {
                Debug.LogError("Parent Transform not passed");
                return;
            }
            for (int i = 0; i < linePoints.Count; i++) {
                linePoints[i] += parent.position;
            }
        }
    }
    //public ListPointsEditor(List<Vector3> initialPoints) {
    //}

    public List<Vector3> GetPoints() {
        return linePoints;
    }

    public void InspectorGUIControlPoints() {
        EditorGUILayout.HelpBox("Current points: " + linePoints.Count, MessageType.None);
        if (linePoints.Count < 2) {
            GUI.color = new Color(1, .2f, 0, 1);
            if (GUILayout.Button("Create 2 Points", GUILayout.Height(30f))) {
                int i = 0;
                while (linePoints.Count < 2) {
                    linePoints.Add(Vector3.zero * i);
                    i++;
                }
            }
            GUI.color = Color.white;
        } else {
            alignError = EditorGUILayout.FloatField("Error used for Align", alignError);
            if (GUILayout.Button("Align Points", GUILayout.Height(30f))) {
                AlignPoints(alignError);
            }
        }
    }

    private void SetMousePos() {
        mousePos = Camera.current.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x, (Camera.current.pixelHeight - Event.current.mousePosition.y)));
        pointRadiusOnScene =  Camera.current.transform.position.z * -.02f;
        mousePos = new Vector3(mousePos.x, mousePos.y);
    }

    private int selectedPointIndex;
    //private bool shiftPosition;
    //private int closestShiftPoint;
    int closestPointToMouse;
    int deletingIndex;

    public void ControlSceneGUI() {
        SetMousePos();
        if (linePoints.Count >= 2) {
            int hoveringIndex = -1;
            for (int i = 0; i < linePoints.Count; i++) {
                if ((linePoints[i] - mousePos).magnitude <= pointRadiusOnScene && deletingIndex != i) {
                    Handles.color = new Color(1, 1, 0, 0.5f);
                    hoveringIndex = i;
                }
                else {
                    Handles.color = new Color(1, 1, 0, 0.2f);
                }
                if (deletingIndex == i)
                    Handles.color = new Color(1, 0, 0, 0.7f);

                Handles.DrawSolidDisc(linePoints[i], Vector3.forward, pointRadiusOnScene);
                Handles.color = new Color(1, 1, 0, 0.2f);
                if (i != linePoints.Count - 1) {
                    Handles.DrawLine(linePoints[i], linePoints[i + 1]);
                }
            }
            Handles.color = Color.white;

            InsertingPointLogic();
            DeletingPointLogic();

            if ((Event.current.type == EventType.MouseDown) && Event.current.button == 0) {
                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                if (hoveringIndex != -1) {
                    selectedPointIndex = hoveringIndex;
                }
                if (selectedPointIndex == -1) {
                    if (Event.current.shift) {
                        linePoints.Insert(closestPointToMouse, mousePos);
                    }
                    if (Event.current.control) {
                        linePoints.RemoveAt(deletingIndex);
                    }

                    Event.current.Use();
                }
            }

            if ((Event.current.type == EventType.MouseDrag) && Event.current.button == 0) {
                if (selectedPointIndex != -1) {
                    linePoints[selectedPointIndex] = mousePos;
                }
                Event.current.Use();
            }

            if ((Event.current.type == EventType.MouseUp) && Event.current.button == 0) {
                GUIUtility.hotControl = 0;
                selectedPointIndex = -1;
                Event.current.Use();
            }
        }
    }
    private void InsertingPointLogic() {
        if (Event.current.shift) {
            closestPointToMouse = 0;
            float minDistance = float.MaxValue;

            for (int i = 0; i < linePoints.Count; i++) {
                if ((mousePos - linePoints[i]).magnitude < minDistance) {
                    minDistance = (mousePos - linePoints[i]).magnitude;
                    closestPointToMouse = i;
                }
            }
            if (closestPointToMouse == 0)
                closestPointToMouse = 1;

            Handles.color = new Color(0, 1, 0, 0.2f);
            Handles.DrawSolidDisc(mousePos, Vector3.forward, pointRadiusOnScene);
            Handles.DrawLine(linePoints[closestPointToMouse], mousePos);
            Handles.DrawLine(linePoints[closestPointToMouse - 1], mousePos);
        }
    }

    private void DeletingPointLogic() {
        deletingIndex = -1;
        if (Event.current.control) {
            deletingIndex = 0;
            float minDistance = float.MaxValue;

            for (int i = 0; i < linePoints.Count; i++) {
                if ((mousePos - linePoints[i]).magnitude < minDistance) {
                    minDistance = (mousePos - linePoints[i]).magnitude;
                    deletingIndex = i;
                }
            }
        }
    }

    private void AlignPoints(float error) {
        for (int i = 1; i < linePoints.Count; i++) {
            for (int j = 0; j < i; j++) {
                if (Mathf.Abs(linePoints[i].x - linePoints[j].x) < error)
                    linePoints[i] = new Vector3(linePoints[j].x, linePoints[i].y);

                if (Mathf.Abs(linePoints[i].y - linePoints[j].y) < error)
                    linePoints[i] = new Vector3(linePoints[i].x, linePoints[j].y);
            }
        }
        if (Mathf.Abs(linePoints[0].x - linePoints[linePoints.Count - 1].x) < error)
            linePoints[0] = new Vector3(linePoints[linePoints.Count - 1].x, linePoints[0].y);

        if (Mathf.Abs(linePoints[0].y - linePoints[linePoints.Count - 1].y) < error)
            linePoints[0] = new Vector3(linePoints[0].x, linePoints[linePoints.Count - 1].y);
    }
}



//if (Event.current.shift) {
//    if (!shiftPosition) {
//        shiftPosition = true;
//        closestShiftPoint = -1;
//        if (selectedPointIndex == linePoints.Count - 1 && selectedPointIndex != 0)
//            closestShiftPoint = linePoints.Count - 2;
//        if (selectedPointIndex == 0 && linePoints.Count > 1)
//            closestShiftPoint = 1;

//        if (closestShiftPoint != -1 && linePoints.Count > 2) {
//            if ((linePoints[selectedPointIndex] - linePoints[selectedPointIndex - 1]).magnitude
//                > (linePoints[selectedPointIndex] - linePoints[selectedPointIndex + 1]).magnitude) {
//                closestShiftPoint = selectedPointIndex + 1;
//            } else {
//                closestShiftPoint = selectedPointIndex - 1;
//            }
//        }
//    }
//} else {
//    if (shiftPosition) {
//        shiftPosition = false;
//    }
//}
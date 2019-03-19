using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformLineRenderer))]
public class PlatformLineRendererEditor : Editor {
    PlatformLineRenderer platform;
    LineRenderer rend;
    EdgeCollider2D coll;

    //List<Vector3> linePoints;

    const float pointRadiusOnScene = .2f;
    Vector3 mousePos;
    ListPointsEditor linePoints;

    private void Awake() {
        platform = target as PlatformLineRenderer;
        rend = platform.GetComponent<LineRenderer>();
        coll = platform.GetComponent<EdgeCollider2D>();
        ColliderPointsFromLineRend();

        GetLinePoints();
    }

    private void OnDestroy() {
        ColliderPointsFromLineRend();
    }

    private void GetLinePoints() {
        //linePoints = new List<Vector3>();
        Vector3[] points = new Vector3[rend.positionCount];
        rend.GetPositions(points);

        linePoints = new ListPointsEditor();
        linePoints.SetInitialPositions(points, true, platform.transform);
        //for (int i = 0; i < points.Length; i++) {
        //    linePoints.Add(points[i]);
        //}
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        linePoints.InspectorGUIControlPoints();
        if (GUILayout.Button("Loop: " + (rend.loop ? "ON" : "OFF"))) {
            rend.loop = !rend.loop;
            ColliderPointsFromLineRend();
        }
    }

    void ColliderPointsFromLineRend() {
        if (rend.loop) {
            Vector3[] points = new Vector3[rend.positionCount];
            rend.GetPositions(points);
            Vector2[] points2D = new Vector2[points.Length + 1];

            for (int i = 0; i < points.Length; i++) {
                points2D[i] = new Vector2(points[i].x, points[i].y);
            }
            points2D[points2D.Length - 1] = points2D[0];
            coll.points = points2D;
        }
        else {
            Vector3[] points = new Vector3[rend.positionCount];
            rend.GetPositions(points);
            Vector2[] points2D = new Vector2[points.Length];

            for (int i = 0; i < points2D.Length; i++) {
                points2D[i] = new Vector2(points[i].x, points[i].y);
            }

            coll.points = points2D;
        }
    }

    void SetPointsToMe() {
        Vector3[] pts = linePoints.GetPoints().ToArray();
        for (int i = 0; i < pts.Length; i++) {
            pts[i] -= platform.transform.position;
        }
        rend.positionCount = pts.Length;
        rend.SetPositions(pts);
    }

    void SetMousePos() {
        mousePos = Camera.current.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x, (Camera.current.pixelHeight - Event.current.mousePosition.y)));

        mousePos = new Vector3(mousePos.x, mousePos.y);
    }
    protected virtual void OnSceneGUI() {
        SetMousePos();
        linePoints.ControlSceneGUI();
        SetPointsToMe();

        Handles.color = new Color(0, 0, 1, .7f);
        Handles.DrawSolidDisc(mousePos, Vector3.forward, .05f);
    }
}

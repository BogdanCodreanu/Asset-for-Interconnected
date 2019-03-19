using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRendererLine : MonoBehaviour {
    private LineRenderer line;
    private int nrOFPoints = 40;

    private void Awake() {
        line = GetComponent<LineRenderer>();
        line.positionCount = nrOFPoints;
        line.SetPosition(1, Vector3.zero);
        line.useWorldSpace = false;
        line.loop = true;
    }

    public void InitLineRendVars(Material mat, Color col, float width, string sortingLayer, int sortingOrder, int customNumberOFPoints = 40) {
        nrOFPoints = customNumberOFPoints;
        line.positionCount = customNumberOFPoints;

        line.startWidth = line.endWidth = width;
        line.sortingLayerName = sortingLayer;
        line.sortingOrder = sortingOrder;
        line.material = mat;
        line.startColor = line.endColor = col;
    }

    public void SetPoints(float width) {
        float oneAngle = 360f / (nrOFPoints * 1f) * Mathf.Deg2Rad;
        for (int i = 0; i < nrOFPoints; i++) {
            line.SetPosition(i, new Vector3(Mathf.Cos(oneAngle * i) * width, Mathf.Sin(oneAngle * i) * width));
        }
    }

    private void OnDestroy() {
        Destroy(gameObject);
    }
}

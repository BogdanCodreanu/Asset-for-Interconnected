using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireRendererLine : MonoBehaviour {
    public LineRenderer line;
    private int nrOFPoints = 40;
    public int gravityPower = 2;
    public float gravityDrag = 1;
    private AnimationCurve gravityDragCurve;

    private float gravPercent;
    private Vector3 linedPos;

    private void Awake() {
        line = GetComponent<LineRenderer>();
        line.positionCount = nrOFPoints;
        line.SetPosition(1, Vector3.zero);
        line.useWorldSpace = true;
        line.loop = false;
        line.numCornerVertices = 5;
        line.numCapVertices = 5;
        line.generateLightingData = true;
        line.textureMode = LineTextureMode.Tile;
    }

    public void InitLineRendVars(AnimationCurve gravityCurve, Material mat, Color col, float width, string sortingLayer, int sortingOrder, int customNumberOFPoints = 40, float gravityDrag = 1, int gravityPower = 2) {
        nrOFPoints = customNumberOFPoints;
        line.positionCount = customNumberOFPoints;

        this.gravityPower = gravityPower;
        this.gravityDrag = gravityDrag;
        line.startWidth = line.endWidth = width;
        line.sortingLayerName = sortingLayer;
        line.sortingOrder = sortingOrder;
        line.material = mat;
        line.startColor = line.endColor = col;

        gravityDragCurve = gravityCurve;
    }

    public void SetWireLight(bool isTurnedOn) {
        line.material.SetFloat("_MovingLight", (isTurnedOn) ? 1 : 0);
    }

    public void SetPoints(Vector3 from, Vector3 to) {

        for (int i = 0; i < nrOFPoints; i++) {
            linedPos = Vector3.Lerp(from, to, (float)i / (nrOFPoints - 1));
            //gravPercent = GravityFunc((linedPos - from).magnitude / (to - from).magnitude);
            gravPercent = gravityDragCurve.Evaluate((linedPos - from).magnitude / (to - from).magnitude);

            line.SetPosition(i, Vector3.Lerp(linedPos, linedPos + Vector3.down * gravityDrag, gravPercent));
        }
    }

    private void OnDestroy() {
        Destroy(gameObject);
    }

    private float GravityFunc(float x) {
        return 1 - Mathf.Pow(x * 2 - 1, gravityPower);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsGameEffects : MonoBehaviour {

    public Material greenWireMaterial;
    public enum WireMaterialType { GreenWire };
    public AnimationCurve wireGravityCurve;
    private static AnimationCurve wireGravityDragCurve;

    private static Material greenWireMat;

    private void Awake() {
        greenWireMat = greenWireMaterial;
        wireGravityDragCurve = wireGravityCurve;
    }

    public static LineRenderer CreateNewLine(Vector3 from, Vector3 to, Material mat, Color col, float width = 0.5f, int roundCap = 0, string sortingLayer = "Game Effects", int sortingOrder = 0) {
        GameObject createdLine = new GameObject("Created Line Rend");
        LineRenderer lineRend = createdLine.AddComponent<LineRenderer>();
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, from);
        lineRend.SetPosition(1, to);
        lineRend.startWidth = lineRend.endWidth = width;
        lineRend.sortingLayerName = sortingLayer;
        lineRend.sortingOrder = sortingOrder;
        lineRend.material = mat;
        lineRend.startColor = lineRend.endColor = col;
        lineRend.numCapVertices = roundCap;

        return lineRend;
    }

        public static CircleRendererLine CreateNewCircle(Vector3 point, float size, Material mat, Color col, float width = 0.1f, string sortingLayer = "Game Effects", int sortingOrder = 0) {
        GameObject createdObj = new GameObject("Created Circle Rend");
        createdObj.transform.position = point;
        CircleRendererLine circleRend = createdObj.AddComponent<CircleRendererLine>();
        circleRend.InitLineRendVars(mat, col, width, sortingLayer, sortingOrder);
        circleRend.SetPoints(size);
        return circleRend;
    }

    public static WireRendererLine CreateNewWire(Vector3 from, Vector3 to, WireMaterialType wireMaterial, Color col, float gravityDrag = 1, int gravityPower = 2, float width = 0.1f, int roundCap = 0,
            string sortingLayer = "Wire renderer", int sortingOrder = 0) {

        GameObject createdObj = new GameObject("Created Wire Rend");
        WireRendererLine wireRend = createdObj.AddComponent<WireRendererLine>();
        wireRend.InitLineRendVars(wireGravityDragCurve,(wireMaterial == WireMaterialType.GreenWire) ? greenWireMat : greenWireMat,
            col, width, sortingLayer, sortingOrder, 40, gravityDrag, gravityPower);
        wireRend.SetPoints(from, to);

        return wireRend;
    }
}

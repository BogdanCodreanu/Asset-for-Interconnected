using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionFuncs : MonoBehaviour {
    private static RaycastHit2D[] inMouseHits;
    private static MechanicalPart mechPartAux;
    private static Transform currentMechTransform;
    private static bool canISelectCollider;

    public enum DetermineColliderLogic { MostImportantMech, LeastImportantMech }

    private static SpriteRenderer selectionRend;
    private static GameObject spawnedSelection;


    public static Collider2D DetermineCollider(Vector3 worldMousePos, DetermineColliderLogic logicUsed, MechanicalPart dontSelectThis = null,
        bool lookForGizmos = false, bool dontSelectChildren = false, bool onlyOutputtingSignalConnections = false, Transform[] selectOnlyThese = null) {
        inMouseHits = Physics2D.RaycastAll(worldMousePos, Vector2.zero, 0.01f);

        if (lookForGizmos) {
            for (int i = 0; i < inMouseHits.Length; i++) {
                if (inMouseHits[i].collider.CompareTag("Gizmos"))
                    return inMouseHits[i].collider;
            }
        }

        int max = -1;
        int maxI = -1;
        int min = -1;
        int minI = -1;

        if (dontSelectThis)
            currentMechTransform = dontSelectThis.transform;
        else
            currentMechTransform = null;

        if (dontSelectChildren && !dontSelectThis)
            Debug.LogError("Trying to not select children, but it's not a given an object transform");

        for (int i = 0; i < inMouseHits.Length; i++) {
            if (inMouseHits[i].transform != currentMechTransform) {
                mechPartAux = inMouseHits[i].collider.GetComponent<MechanicalPart>();

                if (mechPartAux && !inMouseHits[i].collider.CompareTag("Gizmos")) {

                    canISelectCollider = true;

                    if (dontSelectChildren)
                        if (dontSelectThis)
                            if (inMouseHits[i].transform.IsChildOf(dontSelectThis.transform))
                                canISelectCollider = false;

                    if (onlyOutputtingSignalConnections)
                        if (!(mechPartAux is MechanicalOperational))
                            canISelectCollider = false;

                    if (onlyOutputtingSignalConnections && mechPartAux is MechanicalOperational) {
                        if (!(mechPartAux as MechanicalOperational).outputsSignal)
                            canISelectCollider = false;
                    }

                    if (selectOnlyThese != null)
                        if (!EnchantedHoverSoloObjects(inMouseHits[i].collider, selectOnlyThese))
                            canISelectCollider = false;


                    if (canISelectCollider) {
                        if (mechPartAux.selectionPriority > max) {
                            max = mechPartAux.selectionPriority;
                            maxI = i;
                        }
                        if (mechPartAux.selectionPriority < min || min == -1) {
                            min = mechPartAux.selectionPriority;
                            minI = i;
                        }
                    }
                }
            }
        }

        switch (logicUsed) {
            case DetermineColliderLogic.MostImportantMech:
                if (maxI != -1)
                    return inMouseHits[maxI].collider;
                else
                    return null;

            case DetermineColliderLogic.LeastImportantMech:
                if (minI != -1)
                    return inMouseHits[minI].collider;
                else
                    return null;

            default: return null;
        }
    }


    private static bool EnchantedHoverSoloObjects(Collider2D mouseCol, Transform[] onlyThese) {
        foreach (Transform t in onlyThese) {
            if (t) {
                if (t == mouseCol.transform)
                    return true;
            }
        }
        return false;
    }

    public static SelectionHover CreateSelectionOver(Collider2D coll, Material selectionMaterial, Color color,
        string sortintgLayerName = "Selection", int sortingOrder = 100) {
        spawnedSelection = new GameObject("" + sortintgLayerName + color);
        SelectionHover sel = spawnedSelection.AddComponent<SelectionHover>();
        sel.Init(coll.transform);

        selectionRend = spawnedSelection.AddComponent<SpriteRenderer>();
        selectionRend.material = selectionMaterial;
        selectionRend.sortingLayerName = sortintgLayerName;
        selectionRend.sortingOrder = sortingOrder;
        selectionRend.sprite = coll.GetComponent<SpriteRenderer>().sprite;
        selectionRend.material.SetColor("_Color", color);

        return sel;
    }
}

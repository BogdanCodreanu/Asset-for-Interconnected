using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionShortcut : MonoBehaviour {
    public float doublePressDuration = 0.5f;
    private bool counting;
    private float counter;
    private SpawningParts spawningParts;

    private MechanicalPart currentMech;
    private MechanicalPart[] childrenHovered;
    private SelectionHover[] hoveringSelections;
    public Color hoveringColor;
    public Material selectionMaterial;

    private void Awake() {
        spawningParts = GetComponent<SpawningParts>();
    }

    private void Update() {
        if (Input.GetButtonDown("Delete")) {
            if (spawningParts.GetSelectedObject() && GameController.GetGameState() == GameController.GameState.CREATING) {
                if (!spawningParts.GetSelectedObject().mainCube) {
                    if (counting) {
                        SecondPress();
                    }
                    else {
                        FirstPress();
                    }
                }
            }
        }

        if (counting) {
            counter += Time.deltaTime;
            if (counter >= doublePressDuration) {
                LostPress();
                counting = false;
            }

            if (Input.GetMouseButtonDown(0)) {
                LostPress();
            }
        }
    }

    private void LostPress() {
        counting = false;

        for (int i = 0; i < hoveringSelections.Length; i++) {
            if (hoveringSelections[i])
                Destroy(hoveringSelections[i]);
        }
    }

    private void FirstPress() {
        counter = 0f;
        counting = true;

        currentMech = spawningParts.GetSelectedObject();
        if (currentMech) {

            childrenHovered = currentMech.GetComponentsInChildren<MechanicalPart>();
            hoveringSelections = new SelectionHover[childrenHovered.Length];

            for (int i = 0; i < childrenHovered.Length; i++) {
                hoveringSelections[i] = SelectionFuncs.CreateSelectionOver(childrenHovered[i].ownCollider, selectionMaterial,
                    hoveringColor, "Selection", 90);
            }
        }
    }

    private void SecondPress() {
        counting = false;

        SelectingParts.Deselect();
        for (int i = 0; i < hoveringSelections.Length; i++) {
            if (hoveringSelections[i])
                Destroy(hoveringSelections[i]);
        }

        SpawningParts.Erase(childrenHovered);
        UndoMachine.Record();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteButtonHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public MechanicalUI mechUI;
    private MechanicalPart[] childrenHovered;
    private SelectionHover[] hoveringSelections;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (button.interactable) {
            childrenHovered = mechUI.currentMech.GetComponentsInChildren<MechanicalPart>();
            hoveringSelections = new SelectionHover[childrenHovered.Length];

            for (int i = 0; i < childrenHovered.Length; i++) {
                hoveringSelections[i] = SelectionFuncs.CreateSelectionOver(childrenHovered[i].ownCollider, mechUI.selectionMaterial,
                    mechUI.deletionColor, "Selection", 90);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (button.interactable) {
            for (int i = 0; i < hoveringSelections.Length; i++) {
                if (hoveringSelections[i])
                    Destroy(hoveringSelections[i]);
            }
        }
    }

    public void DeleteMech() {
        if (button.interactable) {
            for (int i = 0; i < hoveringSelections.Length; i++) {
                if (hoveringSelections[i])
                    Destroy(hoveringSelections[i]);
            }
            SpawningParts.Erase(childrenHovered);
            SelectingParts.Deselect();
            UndoMachine.Record();
        }
    }
}

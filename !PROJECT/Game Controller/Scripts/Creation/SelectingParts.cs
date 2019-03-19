using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectingParts : MonoBehaviour {
    private Vector3 mouseWorldPos;
    private Collider2D mouseCollider;

    private static Collider2D currentSelectedCollider;
    private static MechanicalPart currentMech;

    public Material selectionMaterial;
    public static Material SelectionMaterial;
    public Color mouseOverColor;
    public Color selectedColor;
    public Color selectingGizmosColor;
    public Color playingOverColor;
    public Color playingOverColorSelected;
    public Color limitBoundsColorError;
    private static SelectionHover hoveringSelection;
    private static SelectionHover selectedObjectSelection;

    private static GizmosControl currentGizmos;

    private bool isMouseOverUI;
    public static bool mouseOverSpawningPanel;

    private bool needToCreateHover;

    private CameraControl cameraControl;
    private static SpawningParts spawningParts;

    private bool disabledHoverViaEnchantedSelection;
    private SelectionHover[] enchantedSelections;
    private Transform[] enchantedOnlyThese;
    private bool enchantedHovering;
    public Color enchantedSelectionColor;

    public static bool draggingSomething;
    private static bool dontCloseSpawningPanelOnFirstDrag;

    private ObjectDuplication objectDuplication;

    private GizmosControl hoveredGizmos;

    private void SetStaticsToNull() {
        currentSelectedCollider = null;
        currentMech = null;
        hoveringSelection = null;
        selectedObjectSelection = null;
        currentGizmos = null;
        mouseOverSpawningPanel = false;
        draggingSomething = false;
        dontCloseSpawningPanelOnFirstDrag = false;
    }
    private void Awake() {
        SelectionMaterial = selectionMaterial;
        SetStaticsToNull();
        spawningParts = GetComponent<SpawningParts>();
        objectDuplication = GetComponent<ObjectDuplication>();
        cameraControl = GameController.cameraControl;
    }

    private void MouseOverPanels() {
        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();  // mouse over UI
    }

    void Update() {
        //if (Input.GetKeyDown(KeyCode.Z)) {
        //    if (currentMech)
        //        currentMech.SpawnTutorial();
        //}

        if (!disabledHoverViaEnchantedSelection && !GameController.pausedInput) {  // daca nu e enchanted selection e logica de baza.

            MouseOverPanels();
            mouseWorldPos = GlobalMousePosition.mouseWorldPos;

            if (!isMouseOverUI) {  // if i'm on world position
                if (!Input.GetMouseButton(0)) {
                    if (enchantedHovering) {
                        mouseCollider = SelectionFuncs.DetermineCollider(mouseWorldPos,
                            SelectionFuncs.DetermineColliderLogic.MostImportantMech, null, true, false, false, enchantedOnlyThese);
                    }
                    else {
                        mouseCollider = SelectionFuncs.DetermineCollider(mouseWorldPos,
                            SelectionFuncs.DetermineColliderLogic.MostImportantMech, null, true);  // i get the collider on my mouse
                    }
                }
            }
            else {
                mouseCollider = null;
            }


            if (mouseCollider) {  // if i have somethign on mouse
                if (hoveringSelection) {  // if i had a hover selection
                    if (hoveringSelection.followedObject != mouseCollider.transform) {  // that is not the hoverSelection over the object on mouse
                        DestroyHoverSelection();  // kill it
                        //if (hoveredGizmos) {
                        //    hoveredGizmos = null;
                        //}
                    }
                }

                needToCreateHover = false;
                if (!hoveringSelection)
                    needToCreateHover = true;

                if (currentSelectedCollider)
                    if (mouseCollider.transform == currentSelectedCollider.transform)
                        needToCreateHover = false;

                //hoveredGizmos = null;
                if (needToCreateHover) {  // if i need to create hover
                    HoveringSelection();
                }

                //if (hoveredGizmos && !wasHoveringOverGizmos) {
                //    wasHoveringOverGizmos = true;
                //    hoveredGizmos.MouseHoverEnter();
                //}
            }

            if (!mouseCollider) {  // if im not on anything with my mouse
                if (hoveringSelection && !Input.GetMouseButton(0)) {  // and if i had a hovering selection
                    DestroyHoverSelection();  // i destroy it
                }
            }

            if (!mouseOverSpawningPanel) {  // i can select or deselect i'm not over the spawning panel
                if (Input.GetMouseButtonDown(0)) {  // if i press click
                    if (mouseCollider) {  // and if i had a current object in position
                        if (mouseCollider.CompareTag("Gizmos")) {  // if it is a gizmos
                            GizmosFirstClick(mouseCollider, mouseWorldPos, true);  // its a first click on gizmos
                        }
                        else {
                            SelectObject(mouseCollider);  // i select the object
                        }
                    }
                    else {  // if i press on empty space
                        if (!isMouseOverUI && !GameController.pausedInput) {
                            Deselect(); // if i press click on something empty i deselect
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0)) {
                OnDragMouse(mouseWorldPos);
            }
            else {
                draggingSomething = false;
            }

            if (Input.GetMouseButtonUp(0)) {
                ReleaseMouse(mouseWorldPos);
            }
        }
    }

    private void DestroyHoverSelection() {
        Destroy(hoveringSelection);

        if (hoveredGizmos) {
            hoveredGizmos.MouseHoverExit();
            hoveredGizmos = null;
        }

        if (enchantedHovering)
            ColorEnchantedSelection(null);
    }

    private void HoveringSelection() {
        if (GameController.GetGameState() == GameController.GameState.CREATING) {
            if (mouseCollider.CompareTag("Gizmos")) {
                hoveringSelection = SelectionFuncs.CreateSelectionOver(mouseCollider, selectionMaterial, selectingGizmosColor,
                "Gizmos Selection");
                hoveredGizmos = mouseCollider.GetComponent<GizmosControl>();
                hoveredGizmos.MouseHoverEnter();
            }
            else {
                hoveringSelection = SelectionFuncs.CreateSelectionOver(mouseCollider, selectionMaterial, mouseOverColor);
                if (enchantedHovering) {
                    ColorEnchantedSelection(mouseCollider);
                }
            }
        }
        else {
            hoveringSelection = SelectionFuncs.CreateSelectionOver(mouseCollider, selectionMaterial, playingOverColor);
        }
    }

    private void SelectObject(Collider2D coll) {
        if (currentSelectedCollider)
            Deselect();
        currentMech = coll.GetComponent<MechanicalPart>();
        SelectObject(currentMech);
    }
    public void SelectObject(MechanicalPart m) {
        if (currentSelectedCollider)
            Deselect();

        currentMech = m;
        currentSelectedCollider = m.ownCollider;

        if (hoveringSelection)
            Destroy(hoveringSelection);

        if (GameController.GetGameState() == GameController.GameState.CREATING) {
            selectedObjectSelection = SelectionFuncs.CreateSelectionOver(currentSelectedCollider, selectionMaterial, selectedColor,
                "Selection", 0);
            currentMech.SpawnGizmos(cameraControl, selectionMaterial, limitBoundsColorError);
            currentMech.SpawnUI(true);
        }
        else {
            selectedObjectSelection = SelectionFuncs.CreateSelectionOver(currentSelectedCollider, selectionMaterial, playingOverColorSelected,
                "Selection", 0);
            currentMech.SpawnUI(false);
        }

    }

    private void OnDragMouse(Vector3 mouseWorldPos) {
        if (currentGizmos) {
            GizmosDrag(mouseWorldPos);
            draggingSomething = true;
        }
    }

    public static void Deselect() {
        if (currentMech) {
            currentMech.KillGizmos();
            currentMech.KillUI();
        }
        Destroy(selectedObjectSelection);

        currentSelectedCollider = null;
        currentGizmos = null;
        currentMech = null;


        if (!mouseOverSpawningPanel && !dontCloseSpawningPanelOnFirstDrag) {
            spawningParts.CloseSpawningPanel();
        }
    }

    private void ReleaseMouse(Vector3 mouseWorldPos) {

        if (currentGizmos) {
            currentGizmos.ReleaseClick(mouseWorldPos);
        }

        currentGizmos = null;
        dontCloseSpawningPanelOnFirstDrag = false;
    }

    private void GizmosFirstClick(Collider2D coll, Vector3 mousePos, bool canDuplicate = false) {
        currentGizmos = coll.GetComponent<GizmosControl>();
        GizmosFirstClick(currentGizmos, mousePos, canDuplicate);
    }
    public void GizmosFirstClick(GizmosControl gizmosControl, Vector3 mousePos, bool canDuplicate = false) {
        if (Input.GetKey(KeyCode.LeftShift) && canDuplicate && gizmosControl.movementGizmosAndCanDuplicate) {
            DuplicateByGizmosCheck(gizmosControl, mousePos);
            return;
        }
        currentGizmos = gizmosControl;
        currentGizmos.FirstClick(mousePos);
    }

    private void GizmosDrag(Vector3 mousePos) {
        currentGizmos.OnDrag(mousePos);
        if (GameSettings.showHandlesDescriptions)
            currentGizmos.ShowDescription();
    }

    public void CreatedNewMechPartAndNowIAmDraggingIt(MechanicalPart mech, Vector3 mousePos) {
        SelectObject(mech.ownCollider);
        //mech.GetMovingGizmos().SetFirstPlacement();
        GizmosFirstClick(mech.GetMovingGizmos(), mousePos);
        mech.GetMovingGizmos().SetObjectOnPointByGizmos(mousePos);

        hoveringSelection = SelectionFuncs.CreateSelectionOver(mech.GetMovingGizmos().GetComponent<Collider2D>(),
            selectionMaterial, selectingGizmosColor, "Gizmos Selection");
        dontCloseSpawningPanelOnFirstDrag = true;

        mech.AddMechToGameControllerList();
    }

    public MechanicalPart GetSelectedObject() {
        return currentMech;
    }

    private void DuplicateByGizmosCheck(GizmosControl gizmosControl, Vector3 mousePos) {
        objectDuplication.FirstIntentToDuplicate(gizmosControl, mousePos);
        Deselect();
    }

    public void DisableViaEnchantedSelection() {
        Deselect();
        if (hoveringSelection)
            Destroy(hoveringSelection);
        disabledHoverViaEnchantedSelection = true;
    }

    public void EnableViaEnchantedSelection() {
        disabledHoverViaEnchantedSelection = false;
    }

    public void EnableEnchantedHovering(SelectionHover[] selections, Transform[] onlyThese) {
        enchantedSelections = selections;
        enchantedOnlyThese = onlyThese;
        enchantedHovering = true;
    }

    public void DisableEnchantedHovering() {
        enchantedHovering = false;
    }

    private void ColorEnchantedSelection(Collider2D mouseCol) {
        foreach (SelectionHover s in enchantedSelections) {
            if (s) {
                if (mouseCol) {
                    if (s.followedObject == mouseCol.transform) {
                        s.ChangeColor(enchantedSelectionColor, 150);
                    }
                    else {
                        s.ResetColor();
                    }
                }
                else {
                    s.ResetColor();
                }
            }
        }
    }

    private bool EnchantedHoverSoloObjects(Collider2D mouseCol) {
        foreach (SelectionHover s in enchantedSelections) {
            if (s) {
                if (s.followedObject == mouseCol.transform)
                    return true;
            }
        }
        return false;
    }

    public void ReselectAfterOneFrame() {
        StartCoroutine(ReselectAfterFrame());
    }

    private IEnumerator ReselectAfterFrame() {
        MechanicalPart currentSelection = currentMech;
        Deselect();
        yield return new WaitForEndOfFrame();
        if (currentSelection)
            SelectObject(currentSelection);
    }
}

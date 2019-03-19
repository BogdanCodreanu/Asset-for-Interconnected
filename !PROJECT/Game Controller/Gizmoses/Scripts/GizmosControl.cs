using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GizmosControl : MonoBehaviour {

    public string gizmosName;
    [HideInInspector] public float cameraScaling = 1;
    public float cameraReductionSize = 1f;
    private CameraControl cameraControl;

    // movement gizmos bool se seteaza din script, exemplu la connection gizmos la fel si canDuplicate
    [HideInInspector] public bool movementGizmosAndCanDuplicate;
    [HideInInspector] public int gizmosesSpawnArrayIndex;

    [HideInInspector] public bool dragging;
    [HideInInspector] public Vector3 initialMechPos;
    [HideInInspector] public Quaternion initialMechRot;
    [HideInInspector] public Vector3 grabDiff;
    [HideInInspector] public MechanicalPart currentMech;


    public string descriptionOnHoverTitle;
    public ItemDescriptionPanel itemDescriptionPrefab;
    protected ItemDescriptionPanel itemDescriptionSpawn;
    private ItemDescriptionPanel itemDescriptionHoverSpawn;
    private float itemDescriptionCounter;
    private bool itemDescriptionCounting;
    private float itemDescriptionCounterDuration = .5f;
    
    public enum GizmosShowInChildrenType { None, Special, Signals }
    public GizmosShowInChildrenType showInChildren;

    private bool isValidForMech;
    public bool IsValidForMech {
        get {
            return isValidForMech;
        }
    }
    private MechanicalPart[] allChildrenMechsForValidation;

    public void Initialialize(MechanicalPart modifyingObject, CameraControl cameraCtrl, int arrayIndex) {
        cameraControl = cameraCtrl;
        cameraScaling = cameraControl.GetCameraScalingGizmos();
        transform.localScale = Vector3.one * cameraScaling * cameraReductionSize;

        currentMech = modifyingObject;
        gizmosesSpawnArrayIndex = arrayIndex;
        Init(modifyingObject);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .7f);

        RefreshValidationForMech();
    }
    public void RefreshValidationForMech() {
        isValidForMech = AllowValidMechOnTheCurrentState();
    }

    protected virtual bool AllowValidMechOnTheCurrentState() {
        return true;
    }

    public virtual void Init(MechanicalPart modifyingObject) { }

    public abstract void OnDrag(Vector3 mousePosition);
    public abstract void ShowDescription();

    public void FirstClick(Vector3 mousePosition) {
        dragging = true;
        grabDiff = transform.position - mousePosition;
        initialMechPos = currentMech.transform.position;
        initialMechRot = currentMech.transform.rotation;

        allChildrenMechsForValidation = currentMech.GetComponentsInChildren<MechanicalPart>();


        if (GameSettings.showHandlesDescriptions) {
            if (itemDescriptionHoverSpawn)
                itemDescriptionHoverSpawn.KillWithFade();
            itemDescriptionSpawn = Instantiate(itemDescriptionPrefab.gameObject, Input.mousePosition, Quaternion.identity, GameController.mainCanvasStatic.transform).GetComponent<ItemDescriptionPanel>();
            itemDescriptionSpawn.Init();
        }

        OnFirstClick(mousePosition);

        itemDescriptionCounting = false;
    }
    
    public void ReleaseClick(Vector3 mousePosition) {
        dragging = false;

        if (itemDescriptionSpawn)
            itemDescriptionSpawn.KillWithFade();

        OnReleaseClick(mousePosition);

        if (/*SuccessCondition() &&*/ ShouldRecordUndoOnSuccessPlacement()) {
                UndoMachine.Record();
        }
    }

    public abstract bool ShouldRecordUndoOnSuccessPlacement();

    public void OnKill() {
        if (itemDescriptionSpawn)
            itemDescriptionSpawn.KillWithFade();
        if (itemDescriptionHoverSpawn)
            itemDescriptionHoverSpawn.KillWithFade();

       // KillBoundLimitHovers();
        OnKilledGizmos();
    }
    private void Update() {
        if (cameraControl)
            cameraScaling = cameraControl.GetCameraScalingGizmos();
        transform.localScale = Vector3.one * cameraScaling * cameraReductionSize;

        if (dragging) {
            foreach (MechanicalPart m in allChildrenMechsForValidation) {
                m.CheckMyValidationAsChild(currentMech);
            }
            currentMech.CheckValidationFromAllSpawnedGizmoses();
        }

        if (itemDescriptionCounting) {
            itemDescriptionCounter += Time.deltaTime;
            if (itemDescriptionCounter >= itemDescriptionCounterDuration) {
                itemDescriptionCounting = true;
                SpawnHoverItemDescription();
            }
        }

        OnUpdate();
    }


    public virtual void OnFirstClick(Vector3 mousePosition) { }
    public virtual void OnReleaseClick(Vector3 mousePosition) { }
    public virtual void OnKilledGizmos() { }
    public virtual void OnUpdate() { }
    public virtual void SetObjectOnPointByGizmos(Vector3 mousePosition) { }

    //private void CheckForMechAndChildrenValidation() {
    //    if (currentMech.DoesThisTouchBounds() || !ShouldTheMechBeValidOnThisMove()) {
    //        currentMech.AddGizmosInvalidReason(this);
    //    }
    //    else {
    //        currentMech.RemoveGizmosInvalidReason(this);
    //    }
    //    //for (int i = 0; i < childrenForBounds.Length; i++) {
    //    //    if (childrenForBounds[i].DoesThisTouchBounds() || !ShouldTheMechBeValidOnThisMove()) {
    //    //        childrenForBounds[i].AddGizmosInvalidReason(this);
    //    //        //someoneTouchesBounds = true;
    //    //    } else {
    //    //        childrenForBounds[i].RemoveGizmosInvalidReason(this);
    //    //    }
    //    //}
    //}


    private void OnDestroy() {
        OnKill();
        Destroy(gameObject);
    }

    public void MouseHoverEnter() {
        if (!string.IsNullOrEmpty(descriptionOnHoverTitle) && GameSettings.showHandlesDescriptions) {
            itemDescriptionCounting = true;
            itemDescriptionCounter = 0;
        }
    }

    public void MouseHoverExit() {
        itemDescriptionCounting = false;
        if (itemDescriptionHoverSpawn)
            itemDescriptionHoverSpawn.KillWithFade();
    }

    private void SpawnHoverItemDescription() {
        itemDescriptionHoverSpawn = Instantiate(itemDescriptionPrefab.gameObject, Input.mousePosition, Quaternion.identity, GameController.mainCanvasStatic.transform).GetComponent<ItemDescriptionPanel>();
        itemDescriptionHoverSpawn.SetText(descriptionOnHoverTitle);
        itemDescriptionCounting = false;
        itemDescriptionHoverSpawn.Init();
    }
}

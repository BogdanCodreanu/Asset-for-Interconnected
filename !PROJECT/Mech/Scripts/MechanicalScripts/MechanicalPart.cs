using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanicalPart : MonoBehaviour {

    public string mechName;
    [TextArea]
    public string description;
    public bool mainCube;
    public int selectionPriority;
    public bool alive;

    [Header("Gizmos Control")]
    public GizmosControl[] gizmoses;
    private GizmosControl[] gizmosesSpawns = new GizmosControl[1];
    private List<GizmosControl> childrenGizmoses;
    private GizmosControl movementGizmos;
    public float GizmosRotationAngle;

    private MechanicalUI spawnedUI;

    [Header("Need To Assign Fields")]
    [NeedToAssignComponent] public Transform handleTransform;
    public LayerMask creationBoundsLayer;
    [NeedToAssignComponent] public MechanicalUI mechUI;

    [Header("Mechanical Settings")]
    protected bool started2 = false;

    [HideInInspector]
    public Collider2D ownCollider;
    [HideInInspector]
    public Rigidbody2D ownRb;
    [HideInInspector] public SpriteRenderer ownRend;

    //private SelectionHover limitBoundsHover;
    protected bool enabledState;

    private Transform savedParent;
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    [HideInInspector] public Rigidbody2D savedJointRb;
    [HideInInspector] public MechanicalPart connectedToMech;
    [HideInInspector] public Vector2 savedJointAnchor;
    [HideInInspector] public AnchoredJoint2D ownAnchoredJoint;

    private enum MechConnectionType { NONE, FixedJoint, HingeJoint }
    private MechConnectionType connectionType = MechConnectionType.NONE;

    [NeedToAssignComponent] public Tutorial4Mechs myTutorial;
    private Tutorial4Mechs spawnTutorial;

    public int indexInSavingGroup;

    private bool scriptDestroyed;
    public bool ScriptDestroyed {
        get {
            return scriptDestroyed;
        }
    }

    private bool isValidMech = true;
    public bool IsValidMech {
        get {
            return isValidMech;
        }
    }

    public virtual void Awake() {
        alive = true;
        scriptDestroyed = false;
        ownCollider = GetComponent<Collider2D>();
        ownRb = GetComponent<Rigidbody2D>();
        ownRend = GetComponent<SpriteRenderer>();
        ownAnchoredJoint = GetComponent<AnchoredJoint2D>();
        ownRend.sortingOrder = selectionPriority * 100;

        if (ownAnchoredJoint is FixedJoint2D) {
            connectionType = MechConnectionType.FixedJoint;
        } else if (ownAnchoredJoint is HingeJoint2D) {
            connectionType = MechConnectionType.HingeJoint;
        }

        childrenGizmoses = new List<GizmosControl>();
        Start2();
    }

    public virtual void Start2() {
        started2 = true;
    }

    public void Update() {
        UpdateMoreLogic();
        OnUpdate();
    }
    public virtual void UpdateMoreLogic() { }
    public virtual void OnUpdate() { }
    public virtual void KillAllSpawns() { }

    /// <summary>
    /// DONT OVERRIDE THIS UNLESS CHANGED LOGIC
    /// </summary>
    public void SetEnabledState(bool value) {
        if (!started2)
            Start2();
        enabledState = value;

        ownRb.StopAllForces();

        if (value) {
            alive = true;
            SaveLifeProperties();
            
            RecreateFixedJoint();

            if (isValidMech) {
                ownCollider.enabled = true;
                ownRb.isKinematic = false;
            } else {
                gameObject.SetActive(false);
            }
        }
        if (!value) {
            AssignLifeProperties();

            KillAllSpawns();

            ownCollider.enabled = true;
            ownRb.isKinematic = true;
            gameObject.SetActive(true);
        }

        SetEnabledStateMoreLogic(value);

        ChangedEnabledState(value);
    }

    public virtual void SetEnabledStateMoreLogic(bool value) { }

    /// <summary>
    /// Change on final partts.
    /// </summary>
    public virtual void ChangedEnabledState(bool value) { }
    
    public void SpawnGizmos(CameraControl cameraControl, Material selectionMaterial, Color boundsLimitColor) {
        gizmosesSpawns = new GizmosControl[gizmoses.Length];
        for (int i = 0; i < gizmoses.Length; i++) {
            if (gizmoses[i]) {
                gizmosesSpawns[i] = Instantiate(gizmoses[i].gameObject, transform.position, Quaternion.identity).GetComponent<GizmosControl>();
                gizmosesSpawns[i].Initialialize(this, cameraControl, i);
            }
        }
        SpawnChildrenGizmoses(cameraControl);
    }

    // killing al the gizmos
    public void KillGizmos() {
        if (gizmosesSpawns != null) {
            for (int i = 0; i < gizmosesSpawns.Length; i++) {
                if (gizmosesSpawns[i]) {
                    Destroy(gizmosesSpawns[i]);
                }
            }
        }
        foreach (GizmosControl gizmos in childrenGizmoses) {
            Destroy(gizmos);
        }
        childrenGizmoses.Clear();
        gizmosesSpawns = null;
    }

    public GizmosControl SelectIndexedGizmos(int index) {
        if (gizmosesSpawns[index])
            return gizmosesSpawns[index];
        Debug.LogError("gizmos at index " + index + " not existing");
        return null;
    }

    private void SpawnChildrenGizmoses(CameraControl cameraControl) {
        if (GameProgression.showSpecialHandleChildren) {

            childrenGizmoses = new List<GizmosControl>();
            MechanicalPart[] children = GetComponentsInChildren<MechanicalPart>();
            GizmosControl spawn;
            foreach (MechanicalPart mech in children) {
                if (mech != this) {

                    if (mech.transform.GetChildDepth(transform) <= GameProgression.showSpecialHandleChildrenDepth) {

                        for (int i = 0; i < mech.gizmoses.Length; i++) {
                            if (GameProgression.showSpecialHandleChildren && mech.gizmoses[i].showInChildren == GizmosControl.GizmosShowInChildrenType.Special) {
                                spawn = Instantiate(mech.gizmoses[i].gameObject, mech.transform.position, Quaternion.identity).GetComponent<GizmosControl>();
                                spawn.Initialialize(mech, cameraControl, -1);
                                childrenGizmoses.Add(spawn);
                            }
                        }
                    }
                }
            }
        }
    }
    
    public void SpawnUI(bool interactibleUI) {
        if (!mainCube) {
            if (mechUI) {
                spawnedUI = Instantiate(mechUI.gameObject).GetComponent<MechanicalUI>();
                spawnedUI.Init(this, interactibleUI);
                AskForUIFields(spawnedUI);
            }
            else {
                Debug.LogError("No UI to spawn assigned to " + gameObject.name);
            }
        }
    }
    public virtual void AskForUIFields(MechanicalUI spawnUI) { }
    
    public void KillUI() {
        if (spawnedUI)
            Destroy(spawnedUI.gameObject);
    }

    public void SetMovingGizmosTo(GizmosControl movingGizmos) {
        movementGizmos = movingGizmos;
    }

    public GizmosControl GetMovingGizmos() {
        return movementGizmos;
    }

    public void AddMechToGameControllerList(bool withKids = false) {
        if (withKids) {
            MechanicalPart[] kids = GetComponentsInChildren<MechanicalPart>();
            foreach (MechanicalPart m in kids) {
                GameController.AddMechToList(m);
            }
        }
        else {
            GameController.AddMechToList(this);
        }
    }

    public void SetScriptAsDestroied() {
        scriptDestroyed = true;
    }
    
    protected virtual void OnDestroy() {
        scriptDestroyed = true;
        KillAllSpawns();
        KillGizmos();
        KillPart();
        KillUI();
        Destroy(gameObject);
    }

    private void OnDisable() {
        KillAllSpawns();
    }

    public bool DoesThisTouchBounds() {
        bool touchesBounds = ownCollider.IsTouchingLayers(creationBoundsLayer);
        if (touchesBounds)
            InvalidMechAlpha();
        if (!touchesBounds) {
            RefreshValidMechAlpha();
        }
        return touchesBounds;
    }

    public void CheckMyValidationAsChild(MechanicalPart parent) {
        if (parent != this) {
            if (DoesThisTouchBounds()) {
                SetPlacementValidation(false);
                return;
            }
            if (!RecursiveSearchForInvalidMech(connectedToMech)) {
                SetPlacementValidation(false);
                return;
            }
            SetPlacementValidation(true);
        }
    }

    public bool RecursiveSearchForInvalidMech(MechanicalPart parent) {
        if (!isValidMech)
            return false;
        if (!parent)
            return isValidMech;
        return parent.RecursiveSearchForInvalidMech(parent.connectedToMech);
    }

    public void CheckValidationFromAllSpawnedGizmoses() {
        if (DoesThisTouchBounds()) {
            SetPlacementValidation(false);
            return;
        }
        GizmosControl gSpawn;
        if (gizmosesSpawns == null)
            return;

        for (int i = 0; i < gizmosesSpawns.Length; i++) {
            if (gizmosesSpawns[i]) {
                gSpawn = gizmosesSpawns[i];

                gSpawn.RefreshValidationForMech();
                if (!gSpawn.IsValidForMech) {
                    SetPlacementValidation(false);
                    return;
                }
            }
        }
        SetPlacementValidation(true);
    }
    
    /// <summary>
    /// Directly tell to be valid or invalid
    /// </summary>
    public void SetPlacementValidation(bool value) {
        if (value && !isValidMech)
            RefreshValidMechAlpha();
        else if (!value && isValidMech)
            InvalidMechAlpha();
    }

    private void RefreshValidMechAlpha() {
        isValidMech = true;
        ownRend.color = Color.white;
    }
    private void InvalidMechAlpha() {
        isValidMech = false;
        ownRend.color = new Color(.5f, .5f, .5f, .6f);
        OnInvalidation();
    }

    protected virtual void OnInvalidation() {

    }

    public virtual void KillPart(bool killJointConnection = true, bool recursiveKillChildrenCondition = true) {
        if (enabledState) {
            alive = false;

            if (killJointConnection) {
                transform.parent = null;
                Destroy(ownAnchoredJoint);
            }

            KillAllSpawns();
            OnKilledPart();

            if (recursiveKillChildrenCondition) {
                MechanicalPart[] children = GetComponentsInChildren<MechanicalPart>();
                foreach (MechanicalPart m in children) {
                    if (m != this) {
                        m.KillPart(false, false);
                    }
                }
            }
        }
    }
    public virtual void OnKilledPart() { }
    
    /// <summary>
    /// When moved by a moving gizmos this function is called (needed for moving joints / anchors and such)
    /// </summary>
    /// <param name="mousePosition"></param>
    public void DragMove(Vector3 mousePosition) {
        transform.position = mousePosition;
        if (connectionType != MechConnectionType.NONE) {
            if (ownAnchoredJoint.connectedBody)
                ownAnchoredJoint.connectedAnchor = ownAnchoredJoint.connectedBody.transform.InverseTransformPoint(mousePosition);
        }
    }

    public void RecreateFixedJoint() {
        if (connectionType != MechConnectionType.NONE) {
            Rigidbody2D rb = ownAnchoredJoint.connectedBody;
            Vector2 anchor = ownAnchoredJoint.anchor;
            Destroy(ownAnchoredJoint);
            AddComponentNewJoint();
            ownAnchoredJoint.connectedBody = rb;
            ownAnchoredJoint.anchor = anchor;
        }
    }

    public void ConnectToRigidbody(MechanicalPart connectedToMech, Vector3 centerPos) {
        if (connectionType != MechConnectionType.NONE) {
            ownAnchoredJoint.connectedBody = connectedToMech.ownRb;
            ownAnchoredJoint.anchor = centerPos;
            this.connectedToMech = connectedToMech;
        }
    }
    public void DisconnectFromRigidBody() {
        if (connectionType != MechConnectionType.NONE) {
            Destroy(ownAnchoredJoint);
            AddComponentNewJoint();
            ownAnchoredJoint.connectedBody = null;
            ownAnchoredJoint.anchor = Vector2.zero;
            connectedToMech = null;
        }
    }

    private void AddComponentNewJoint() {
        if (connectionType == MechConnectionType.FixedJoint)
            ownAnchoredJoint = gameObject.AddComponent<FixedJoint2D>();
        else if (connectionType == MechConnectionType.HingeJoint)
            ownAnchoredJoint = gameObject.AddComponent<HingeJoint2D>();
    }

    public virtual void SaveLifeProperties() {
        savedParent = transform.parent;
        savedPosition = transform.localPosition;
        savedRotation = transform.localRotation;

        if (connectionType != MechConnectionType.NONE) {
            savedJointRb = ownAnchoredJoint.connectedBody;
            savedJointAnchor = ownAnchoredJoint.anchor;
        }

        SaveCustomProperties();
    }
    public virtual void SaveCustomProperties() { }

    public virtual void AssignLifeProperties() {
        transform.parent = savedParent;
        transform.localPosition = savedPosition;
        transform.localRotation = savedRotation;

        if (connectionType != MechConnectionType.NONE) {
            if (ownAnchoredJoint)
                Destroy(ownAnchoredJoint);
            AddComponentNewJoint();
            ownAnchoredJoint.connectedBody = savedJointRb;
            ownAnchoredJoint.anchor = savedJointAnchor;
        }


        AssignCustomProperties();
    }
    public virtual void AssignCustomProperties() { }

    public void SpawnTutorial() {
        spawnTutorial = Instantiate(myTutorial.gameObject, Vector3.zero, Quaternion.identity, null).GetComponent<Tutorial4Mechs>();
        spawnTutorial.Init(this);
    }


    public virtual string GenerateSavedString() {
        string outer = "";
        outer += transform.position.x + " " + transform.position.y + " " + transform.position.z + " ";
        outer += transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z + " " + transform.rotation.w + " ";
        outer += "parent" + ((transform.parent != null) ? transform.parent.GetComponent<MechanicalPart>().indexInSavingGroup.ToString() : "-1") + " ";

        return outer;
    }
    
    public virtual void AssignReadSavedString(string read, MechanicalPart[] allSpawns) {
        int auxInt;

        System.Text.RegularExpressions.MatchCollection allMatches = System.Text.RegularExpressions.Regex.Matches(read, @"-?\d+(\.\d*)?");
        if (allMatches.Count >= 7) {

            transform.position = new Vector3(float.Parse(allMatches[0].ToString()), float.Parse(allMatches[1].ToString()), float.Parse(allMatches[2].ToString()));
            transform.rotation = new Quaternion(float.Parse(allMatches[3].ToString()), float.Parse(allMatches[4].ToString()), float.Parse(allMatches[5].ToString()), float.Parse(allMatches[6].ToString()));
        } else {
            Debug.LogError("Given string couldn't be regexed correctly! Did not create at least 7 floats");
        }

        allMatches = System.Text.RegularExpressions.Regex.Matches(read, @"parent-?\d+");
        allMatches = System.Text.RegularExpressions.Regex.Matches(allMatches[0].ToString(), @"-?\d+");
        auxInt = int.Parse(allMatches[0].ToString());
        if (auxInt != -1) {
            transform.parent = allSpawns[auxInt].transform;
            
            ConnectToRigidbody(allSpawns[auxInt], handleTransform.transform.position - transform.position);
            RecreateFixedJoint();

        }

    }
    
}

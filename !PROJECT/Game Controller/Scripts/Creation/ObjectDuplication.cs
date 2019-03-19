using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplication : MonoBehaviour {

    private Vector3 firstMousePos;
    private SelectingParts selectingParts;
    private bool consideringDuplication;
    private GizmosControl selectedGizmos;
    private LineRenderer visualLine;
    private CircleRendererLine visualCircle;
    public Material lineRendererMaterial;
    public Color visualColor;

    private void Awake() {
        selectingParts = GetComponent<SelectingParts>();
    }

    public void FirstIntentToDuplicate(GizmosControl gizmosControl, Vector3 mousePos) {
        firstMousePos = mousePos;
        consideringDuplication = true;
        selectedGizmos = gizmosControl;
        visualLine = VisualsGameEffects.CreateNewLine(mousePos, GlobalMousePosition.mouseWorldPos, lineRendererMaterial, visualColor, 0.05f, 1);
        visualCircle = VisualsGameEffects.CreateNewCircle(mousePos, .7f * GameController.cameraControl.GetCameraScalingGizmos(), lineRendererMaterial, visualColor, 0.05f);
    }

    private void Update() {
        if (consideringDuplication) {
            visualLine.SetPosition(1, GlobalMousePosition.mouseWorldPos);
            visualCircle.SetPoints(.7f * GameController.cameraControl.GetCameraScalingGizmos());
            if ((GlobalMousePosition.mouseWorldPos - firstMousePos).magnitude >= .7f * GameController.cameraControl.GetCameraScalingGizmos()) {
                consideringDuplication = false;
                DuplicateByGizmos(selectedGizmos, GlobalMousePosition.mouseWorldPos);
            }
            if (Input.GetMouseButtonUp(0)) {
                consideringDuplication = false;
                AbortDuplication();
            }
        }
    }

    private void DuplicateByGizmos(GizmosControl gizmosControl, Vector3 mousePos) {
        MechanicalPart mech = Instantiate(gizmosControl.currentMech.gameObject, mousePos,
            gizmosControl.currentMech.transform.rotation, gizmosControl.currentMech.transform.parent).GetComponent<MechanicalPart>();
        int myGizmosIndex = gizmosControl.gizmosesSpawnArrayIndex;

        selectingParts.SelectObject(mech);
        //mech.SelectIndexedGizmos(myGizmosIndex).SetFirstPlacement();
        selectingParts.GizmosFirstClick(mech.SelectIndexedGizmos(myGizmosIndex), mousePos);
        mech.AddMechToGameControllerList(true);
        Destroy(visualLine.gameObject);
        Destroy(visualCircle);
    }

    private void AbortDuplication() {
        selectingParts.SelectObject(selectedGizmos.currentMech);
        Destroy(visualLine.gameObject);
        Destroy(visualCircle);
    }
}

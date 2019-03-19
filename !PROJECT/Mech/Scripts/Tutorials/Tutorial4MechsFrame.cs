using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4MechsFrame : MonoBehaviour {
    [Header("Drag mechs as children of this object to frame them")]
    public string subtitle;
    [TextArea]
    public string description;

    public Camera cameraForMechsTutorials;

    public float cameraDistance = 10;
    private Camera spawnCamera;

    public bool resetMechs;
    public float resetMechsEvery = 3f;
    private float resetMechsCounter;
    MechanicalPart[] toggledMechs;
    UnitControl[] toggledUnits;

    public void SpawnObjects(Vector3 forgottenPosition) {
        transform.position = forgottenPosition;
        spawnCamera = Instantiate(cameraForMechsTutorials.gameObject, transform).GetComponent<Camera>();
        spawnCamera.transform.localPosition = new Vector3(0, 0, -cameraDistance);
        spawnCamera.fieldOfView = GameController.cameraControl.cam.fieldOfView;
        spawnCamera.targetTexture = new RenderTexture(spawnCamera.pixelWidth, spawnCamera.pixelHeight, 16) {
            filterMode = FilterMode.Bilinear
        };

        ToggleMechs(true);
    }

    private void Update() {
        if (resetMechs) {
            resetMechsCounter += Time.deltaTime;
            if (resetMechsCounter >= resetMechsEvery) {
                resetMechsCounter = 0;
                ResetMechs();
            }
        }
    }

    public RenderTexture GetRenderTextureCamera() {
        return spawnCamera.targetTexture;
    }

    private void ToggleMechs(bool value) {
        if (toggledMechs == null)
            toggledMechs = GetComponentsInChildren<MechanicalPart>();

        foreach (MechanicalPart mech in toggledMechs) {
            mech.SetEnabledState(value);
        }

        if (toggledUnits == null)
            toggledUnits = GetComponentsInChildren<UnitControl>();

        foreach (UnitControl unit in toggledUnits) {
            unit.SetEnabledState(value);
        }
    }

    private void ResetMechs() {
        ToggleMechs(false);
        ToggleMechs(true);
    }

    private void OnDestroy() {
        Destroy(spawnCamera);
    }
}

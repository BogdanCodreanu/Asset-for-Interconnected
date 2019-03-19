using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4Mechs : MonoBehaviour {
    public Tutorial4MechsCanvas canvasPrefab;
    private Tutorial4MechsCanvas spawnCanvas;

    public Tutorial4MechsFrame[] mechTutorialFrames;

    public void Init(MechanicalPart mech) {
        spawnCanvas = Instantiate(canvasPrefab.gameObject, transform).GetComponent<Tutorial4MechsCanvas>();
        spawnCanvas.Init(mech.mechName, this);
        spawnCanvas.SpawnFramesAndMakeUIExplanations(mechTutorialFrames);
    }

    private void OnDestroy() {
        Destroy(gameObject);
    }
}

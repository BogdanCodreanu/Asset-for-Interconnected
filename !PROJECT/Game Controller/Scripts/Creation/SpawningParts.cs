using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningParts : MonoBehaviour {
    private SelectingParts selectingParts;
    private static GameController gameController;
    private PanelSpawnerUI panelSpawnerUI;

    private List<MechanicalPart> spawns;

    private void Awake() {
        selectingParts = GetComponent<SelectingParts>();
        gameController = GetComponent<GameController>();

        spawns = new List<MechanicalPart>();
        if (DatabasesManager.IsValidScene()) {
            List<MechanicalPart> dataBaseMechs = DatabasesManager.GetAvaliableMechs();
            foreach (MechanicalPart m in dataBaseMechs) {
                if (m)
                    spawns.Add(m);
            }

            panelSpawnerUI = GameController.mainCanvasStatic.panelSpawnerUI;

            if (panelSpawnerUI) {
                panelSpawnerUI.SetSpawningParts(this);
                panelSpawnerUI.CreateGrid(spawns);
            }
        }
    }
    
    public void SpawnMech(int i) {
        Vector2 mouseWorldPos;
        MechanicalPart currentMech;
        mouseWorldPos = GlobalMousePosition.mouseWorldPos;
        currentMech = Instantiate(spawns[i].gameObject, mouseWorldPos, Quaternion.identity).GetComponent<MechanicalPart>();
        currentMech.ConnectToRigidbody(GameController.mainCube, Vector3.zero);  // not sure why, but the creation bounds wont work otherwise
        currentMech.SaveLifeProperties();

        selectingParts.CreatedNewMechPartAndNowIAmDraggingIt(currentMech, mouseWorldPos);
        currentMech.SetEnabledState(false);
    }

    public MechanicalPart GetSelectedObject() {  // pentru deletion shortcut
        return selectingParts.GetSelectedObject();
    }

    public static void Erase(MechanicalPart[] killedMechs) {
        foreach (MechanicalPart m in killedMechs) {
            m.transform.parent = null;
        }
        foreach (MechanicalPart m in killedMechs) {
            Erase(m);
        }
    }

    public static void Erase(MechanicalPart killedMech) {
        gameController.Erase(killedMech);
    }

    public void CloseSpawningPanel() {
        if (panelSpawnerUI)
            panelSpawnerUI.ExitScreen();
    }

    public void AddSpawnToGrid(MechanicalPart newSpawn) {
        spawns.Add(newSpawn);
        panelSpawnerUI.DestroyCurrentGrid();
        panelSpawnerUI.CreateGrid(spawns);
    }
}

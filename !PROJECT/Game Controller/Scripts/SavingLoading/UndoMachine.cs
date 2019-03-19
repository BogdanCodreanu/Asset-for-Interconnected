using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoMachine : MonoBehaviour {
    private const int historyLimit = 30;
    private static List<SavedMechanical> historyList = new List<SavedMechanical>();
    private static int currentInt;
    public static int currentIndex;

    private void Awake() {
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift)) {
            Undo();
        }

        if ((Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl))) {
            Redo();
        }
    }

    public static void Record() {
        if (historyList.Count > historyLimit) {
            historyList.RemoveAt(0);
        }
        SavedMechanical addedMech = SaveMachine.SaveAndGet(false);
        if (currentIndex != historyList.Count - 1) {
            int nrOfElementsToDestroy = historyList.Count - currentIndex - 1;
            for (int i = 0; i < nrOfElementsToDestroy; i++) {
                historyList.RemoveAt(historyList.Count - 1);
            }
        }
        historyList.Add(addedMech);
        currentIndex = historyList.Count - 1;
    }

    public static void Undo() {
        if (currentIndex > 0) {
            currentIndex--;
            SaveMachine.LoadAndSet(historyList[currentIndex]);
        }
    }

    public static void Redo() {
        if (currentIndex < historyList.Count - 1) {
            currentIndex++;
            SaveMachine.LoadAndSet(historyList[currentIndex]);
        }
    }
}

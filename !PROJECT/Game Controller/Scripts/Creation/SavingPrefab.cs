using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavingPrefab : MonoBehaviour {

    [HideInInspector] public MechanicalPart mainCube;

    private static GameObject testGameobj;


    private void Update() {
        //if (Input.GetKeyDown(KeyCode.S)) {
        //    SaveMachine();
        //}
    }

    public void SaveMachine() {
        string json = JsonUtility.ToJson(mainCube, true);
        File.WriteAllText(Application.dataPath + "/mainCube.txt", json);
        Debug.Log("written");
    }
}

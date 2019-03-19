using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveMachine : MonoBehaviour {
    public static string directoryName;
    private static Texture2D texture;
    public Camera screenshootingCameraPrefab;
    private static Camera screenshootingCameraPrefb;

    private void Awake() {
        directoryName = Application.dataPath + "/Saved Mechs";
        screenshootingCameraPrefb = screenshootingCameraPrefab;
    }

    /// <summary>
    /// Saves the current Machine and returns it.
    /// </summary>
    /// <param name="takeScreenshot">Should it also capture the screenshot? Costs performance</param>
    /// <returns></returns>
    public static SavedMechanical SaveAndGet(bool takeScreenshot) {
        DatabaseMechs allMechsDatabase = Resources.Load("Mechs", typeof(DatabaseMechs)) as DatabaseMechs;
        List<string> mechsDatabaseNames = new List<string>();
        foreach (MechanicalPart mech in allMechsDatabase.allMechs) {
            mechsDatabaseNames.Add(mech.mechName);
        }

        //List<MechanicalPart> parts = GameController.mechs;
        //SavedMechanical savedMachine = new SavedMechanical(parts.Count);
        MechanicalPart[] parts = GameController.mainCube.GetComponentsInChildren<MechanicalPart>();
        SavedMechanical savedMachine = new SavedMechanical(parts.Length);
        //int i;
        //foreach (MechanicalPart m in parts) {
        //    savedMachine.savedParts[i] = new SavedMechPart(mechsDatabaseNames.IndexOf(m.mechName), i);
        //    m.indexInSavingGroup = i;
        //    i++;
        //}
        for (int i = 0; i < parts.Length; i++) {
            savedMachine.savedParts[i] = new SavedMechPart(mechsDatabaseNames.IndexOf(parts[i].mechName), i);
            parts[i].indexInSavingGroup = i;
        }

        //i = 0;
        //foreach (MechanicalPart m in parts) {
        //    savedMachine.savedParts[i].mechData = m.GenerateSavedString();
        //    i++;
        //}
        for (int i = 0; i < savedMachine.savedParts.Length; i++) {
            savedMachine.savedParts[i].mechData = parts[i].GenerateSavedString();
            //Debug.Log("Gerated: " + savedMachine.savedParts[i].GetDataAndDecrypt());
        }

        if (takeScreenshot) {
            savedMachine.screenshot = ScreenshotBytes(savedMachine);
        }

        return savedMachine;
    }

    private static byte[] ScreenshotBytes(SavedMechanical mech) {
        //MechanicalPart[] allMechs = SpawnSavedMech(mech);
        //MechanicalPart spawnedMainCube = null;
        //for (int i = 0; i < allMechs.Length; i++) {
        //    if (allMechs[i].mainCube) {
        //        spawnedMainCube = allMechs[i];
        //        break;
        //    }
        //}
        //spawnedMainCube.transform.position = new Vector3(5000, 6000);
        Camera spawnCamera = Instantiate(screenshootingCameraPrefb.gameObject,
            new Vector3(GameController.mainCube.transform.position.x, GameController.mainCube.transform.position.y, -10), Quaternion.identity, null).GetComponent<Camera>();

        spawnCamera.targetTexture = new RenderTexture(200, 200, 16) {
            filterMode = FilterMode.Bilinear
        };
        spawnCamera.aspect = 1;
        spawnCamera.orthographicSize = 2.5f;
        spawnCamera.Render();
        texture = new Texture2D(spawnCamera.targetTexture.width, spawnCamera.targetTexture.height, TextureFormat.RGB24, false);
        mech.ssHeight = spawnCamera.targetTexture.height; mech.ssWidth = spawnCamera.targetTexture.width;
        RenderTexture.active = spawnCamera.targetTexture;
        texture.ReadPixels(new Rect(0, 0, spawnCamera.targetTexture.width, spawnCamera.targetTexture.height), 0, 0);
        texture.Apply();
        spawnCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(spawnCamera.gameObject);
       // Destroy(spawnedMainCube.gameObject);
        return texture.GetRawTextureData();
    }

    public static void SaveToFile(string filename, SavedMechanical savedMachine) {
        string path = directoryName + "/" + filename;
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

        FileStream file = File.Create(path);
        bf.Serialize(file, savedMachine);
        file.Close();
    }

    //private static void EncryptFile(string pathToFile) {
    //    char[] chars = File.ReadAllText(pathToFile).ToCharArray();
    //    int aux;
    //    for (int i = 0; i < chars.Length; i++) {
    //        aux = i % 19;
    //        chars[i] = (char)(chars[i] + 1);
    //    }
    //    File.WriteAllText(pathToFile, new string(chars));

    //    //byte[] allBytes = File.ReadAllBytes(pathToFile);
    //    //byte aux;

    //    //for (int i = 0; i < allBytes.Length; i++) {
    //    //    aux = (byte)(i % 19);
    //    //    allBytes[i] = (byte)(allBytes[i] + (byte)(1));
    //    //}
    //    //File.WriteAllBytes(pathToFile, allBytes);
    //}

    //private static void DecryptFile(string pathToFile) {
    //    char[] chars = File.ReadAllText(pathToFile).ToCharArray();
    //    int aux;
    //    for (int i = 0; i < chars.Length; i++) {
    //        aux = i % 19;
    //        chars[i] = (char)(chars[i] - 1);
    //    }
    //    File.WriteAllText(pathToFile, new string(chars));
    //}

    public static SavedMechanical LoadFromFile(string filename) {
        string path = directoryName + "/" + filename;
        if (File.Exists(path)) {
            FileStream file = File.Open(path, FileMode.Open);
            SavedMechanical savedMachine = null;

            try {
                BinaryFormatter bf = new BinaryFormatter();
                savedMachine = bf.Deserialize(file) as SavedMechanical;
            } catch {
                return null;
            } finally {
                file.Close();
            }
            return savedMachine;
        }
        return null;
    }

    public static bool LoadAndSet(SavedMechanical savedMachine) {
        if (savedMachine != null) {
            GameController.DestroyExistingMechs();

            SpawnSavedMech(savedMachine);
            GameController.ResetMechList();
            return true;
        }
        else {
            Debug.LogError("saved machine era null");
            return false;
        }
    }

    private static MechanicalPart[] SpawnSavedMech(SavedMechanical savedMachine) {
        DatabaseMechs allMechsDatabase = Resources.Load("Mechs", typeof(DatabaseMechs)) as DatabaseMechs;
        MechanicalPart[] spawnMechs = new MechanicalPart[savedMachine.savedParts.Length];
        for (int i = 0; i < savedMachine.savedParts.Length; i++) {
            spawnMechs[i] = Instantiate(allMechsDatabase.allMechs[savedMachine.savedParts[i].mechIndexFromDatabase].gameObject, Vector3.zero, Quaternion.identity).GetComponent<MechanicalPart>();
        }
        for (int i = 0; i < savedMachine.savedParts.Length; i++) {
            spawnMechs[i].AssignReadSavedString(savedMachine.savedParts[i].mechData, spawnMechs);
        }
        return spawnMechs;
    }
}

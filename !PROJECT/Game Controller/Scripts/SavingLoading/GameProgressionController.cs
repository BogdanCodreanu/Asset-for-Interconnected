using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameProgressionController : MonoBehaviour {
    private const string filename = "Save file.txt";

    private void Awake() {
        if (GameProgression.loaded)
            SaveToFile();
        else
            LoadFile();
    }

    private void OnDestroy() {
        SaveToFile();
    }

    private static void LoadFile() {

        GameProgression.loaded = true;
        string path = Application.persistentDataPath + "/" + filename;
        if (File.Exists(path)) {
            string fileText = string.Empty;
            string jsonText = string.Empty;
            GameProgressionFile fileData;
            try {
                fileText = File.ReadAllText(Application.persistentDataPath + "/" + filename);
                jsonText = DecryptString(fileText);
                fileData = JsonUtility.FromJson<GameProgressionFile>(jsonText);
                Debug.Log("loaded");
            } catch {
                Debug.LogError("unable to read save files. using default");
                DefaultSettings();
                return;
            }
            GameProgression.AssignFileData(fileData);
        } else {
            Debug.Log("file wasn't existing");
            DefaultSettings();
        }
    }

    private static void DefaultSettings() {
        GameProgression.enteredInGameForFirstTime = true;
    }

    /// <summary>
    /// Saves the game progression to file.
    /// </summary>
    public static void SaveToFile() {
        Debug.Log("saved Data");
        string path = Application.persistentDataPath + "/" + filename;
        string fileText = string.Empty;
        string jsonText = string.Empty;
        GameProgressionFile fileData = GameProgression.GetFileData();
        jsonText = JsonUtility.ToJson(fileData, false);
        fileText = EncryptString(jsonText);
        File.WriteAllText(path, fileText);
    }
    
    private static string DecryptString(string s) {
        char[] chars = s.ToCharArray();
        //for (int i = 0; i < chars.Length; i++) {
        //    chars[i] = (char)(chars[i] - (i % 19 + i % 7 * i % 9 + i / 3));
        //}
        return new string(chars);
    }

    private static string EncryptString(string s) {
        char[] chars = s.ToCharArray();
        //for (int i = 0; i < chars.Length; i++) {
        //    chars[i] = (char)(chars[i] + (i % 19 + i % 7 * i % 9 + i / 3));
        //}
        return new string(chars);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsHistorySaver : MonoBehaviour {
    public struct SavedTip {
        public string time;
        public string message;
    }
    public TMP_Text savedText;

    public static List<SavedTip> savedTips;
    private static TMP_Text textStatic;
    private bool awaken;

    public void Awake() {
        if (!awaken) {
            awaken = true;
            textStatic = savedText;
            if (savedTips == null) {
                savedTips = new List<SavedTip>();
            }
            RefreshText();
        }
    }


    public static void AddTip(string message) {
        savedTips.Add(new SavedTip { message = message, time = "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second  + "]"});
        RefreshText();
    }

    private static void RefreshText() {
        textStatic.text = "";
        foreach (SavedTip tip in savedTips) {
            textStatic.text = "\t" + tip.time + " " + tip.message + "\n" + textStatic.text;
        }
    }
}

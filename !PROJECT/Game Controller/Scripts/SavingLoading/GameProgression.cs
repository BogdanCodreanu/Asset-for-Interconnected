using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgression  {
    public static bool loaded = false;

    public static bool showSpecialHandleChildren;
    public static int showSpecialHandleChildrenDepth;

    public static bool enteredInGameForFirstTime;

    public static void AssignFileData(GameProgressionFile file) {
        showSpecialHandleChildren = file.showSpecialHandleChildren;
        showSpecialHandleChildrenDepth = file.showSpecialHandleChildrenDepth;
        enteredInGameForFirstTime = file.enteredInGameForFirstTime;
    }

    public static GameProgressionFile GetFileData() {
        GameProgressionFile file = new GameProgressionFile() {
            enteredInGameForFirstTime = enteredInGameForFirstTime,
            showSpecialHandleChildren = showSpecialHandleChildren,
            showSpecialHandleChildrenDepth = showSpecialHandleChildrenDepth
        };
        return file;
    }
}
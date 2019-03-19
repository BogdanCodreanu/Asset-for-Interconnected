using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour {
    [HideInInspector] public GameController gameController;
    public HeaderUI headerUI;
    public PanelSpawnerUI panelSpawnerUI;
    [HideInInspector] public Canvas canvas;
    public UIFadeController panelSpawnerOpenerFader;

    private void Awake() {
        gameController = FindObjectOfType<GameController>();
        headerUI.InitGameController(gameController);
        canvas = GetComponent<Canvas>();
    }
}

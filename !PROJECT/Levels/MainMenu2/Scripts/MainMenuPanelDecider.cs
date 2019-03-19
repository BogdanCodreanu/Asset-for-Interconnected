using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Razziel.AnimatedPanels;

public class MainMenuPanelDecider : MonoBehaviour {
    public float appearingTime = .5f;

    public UIFadeController[] mainMenuButtons;
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;
    public SettingManager settingManager;
    public StagesLevelsButtons stagesLevelsButtons;

    public UIAnimatedPanel othersPanel;

    private void Awake () {
        // back buttons
        settingManager.backButton.onClick.AddListener(delegate { AppearMenu(appearingTime); });
        stagesLevelsButtons.backButton.onClick.AddListener(delegate { AppearMenu(appearingTime); });

        SetButtonsListeners();

        othersPanel.Initialize();
        othersPanel.controller.openingClosingPanel.SetActionBeforeAppear(delegate { FadeMenu(appearingTime); });
        othersPanel.controller.openingClosingPanel.SetActionBeforeFade(delegate { AppearMenu(appearingTime); });
    }
    private void Start() {
        AppearMenu(2f);
    }

    private void AppearMenu(float time) {
        SetButtonsListeners();
        foreach (UIFadeController butt in mainMenuButtons) {
            butt.Appear(time);
        }
    }
    private void FadeMenu(float time) {
        RemoveButtonsListeners();
        foreach (UIFadeController butt in mainMenuButtons) {
            butt.Disappear(time);
        }
    }

    private void SetButtonsListeners() {
        playButton.onClick.AddListener(delegate { PlayButton(); FadeMenu(appearingTime); });
        optionsButton.onClick.AddListener(delegate { OptionButton(); FadeMenu(appearingTime); });
        exitButton.onClick.AddListener(delegate { ExitButton(); FadeMenu(appearingTime); });
    }
    private void RemoveButtonsListeners() {
        playButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void PlayButton() {
        stagesLevelsButtons.EnterScreen();
    }

    private void OptionButton() {
        settingManager.EnterScreen();
    }
    private void ExitButton() {
        FadeMenu(.6f);
        this.ExecuteFunctionWithDelay(.6f, delegate { Application.Quit(); });
    }
}

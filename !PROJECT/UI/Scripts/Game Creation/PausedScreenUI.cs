using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Razziel.AnimatedPanels;

public class PausedScreenUI : MonoBehaviour {
    public Button resumeButton;
    public Button tipsHistoryButton;
    public Button settingsButton;
    public Button mainMenuButton;

    public SettingManager settingsPanelPrefab;
    
    public UIAnimatedPanel animatedMainPanel;
    public UIAnimatedPanel animatedTipsHistory;

    public TipsHistorySaver tipsHistorySaver;

    private void AssignButtonListeners() {
        resumeButton.onClick.AddListener(delegate { ToggleScreen(); });
        mainMenuButton.onClick.AddListener(delegate { SceneController.GoToMainMenu(); GameController.SetTimeScale(1); });
        settingsButton.onClick.AddListener(delegate { SettingsPanel(); });

        tipsHistoryButton.onClick.AddListener(delegate {
            animatedTipsHistory.GetActionThatTogglesPanel()();
        });
    }
    private void RemoveButtonListeners() {
        resumeButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        tipsHistoryButton.onClick.RemoveAllListeners();
    }
    private void Awake() {
        tipsHistorySaver.Awake();
        animatedMainPanel.Initialize();
        animatedMainPanel.controller.openingClosingPanel.SetActionAfterAppear(delegate { AssignButtonListeners(); });
        animatedMainPanel.controller.openingClosingPanel.SetActionBeforeFade(delegate { RemoveButtonListeners(); });

        animatedMainPanel.controller.openingClosingPanel.SetActionBeforeAppear(delegate { GameController.AddPause(this); });
        animatedMainPanel.controller.openingClosingPanel.SetActionAfterFade(delegate { GameController.RemovePause(this); });
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            ToggleScreen();
        }
    }
    
    public void ToggleScreen() {
        animatedMainPanel.GetActionThatTogglesPanel()();
    }

    private void SettingsPanel() {
        ToggleScreen();
        SettingManager spawn = Instantiate(settingsPanelPrefab.gameObject, GameController.mainCanvasStatic.transform).GetComponent<SettingManager>();
        spawn.EnterScreen();
        settingsButton.interactable = false;
        spawn.SetActionOnFinishExittingScreen(delegate { settingsButton.interactable = true; Destroy(spawn.gameObject); });
    }
}

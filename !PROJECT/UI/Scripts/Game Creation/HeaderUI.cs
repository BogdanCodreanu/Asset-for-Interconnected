using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Razziel.AnimatedPanels;

public class HeaderUI : MonoBehaviour {
    private GameController gameController;
    public Button playButton;
    public Button stopButton;
    public Button nextLevelButton;

    private Animator playButtonAnim;
    private Animator stopButtonAnim;
    private Animator anim;

    public Toggle displayHandlesChildrenSpecial;
    public Toggle displayHandlesChildrenSignal;
    public Slider displayHandlesChildrenDepth;
    public UIAnimatedPanel childrenDepthAnimatedPanel;

    public ObjectiveSliderProgression progressionSlider;

    public Toggle cameraFocusToggle;

    public Button undoButton;
    public Button redoButton;

    private void Awake() {
        anim = GetComponent<Animator>();
        playButtonAnim = playButton.GetComponent<Animator>();
        stopButtonAnim = stopButton.GetComponent<Animator>();
        if (DatabasesManager.IsValidScene())
            nextLevelButton.onClick.AddListener(delegate { GameController.OnClickedNextLevel(); SceneController.LoadSceneWithLoadingScreen(DatabasesManager.GetNextSceneName()); });
        ShowPlayButton();
        
        if (GameProgression.loaded) {
            displayHandlesChildrenSpecial.isOn = GameProgression.showSpecialHandleChildren;
            displayHandlesChildrenDepth.value = GameProgression.showSpecialHandleChildrenDepth;
            childrenDepthAnimatedPanel.Toggle(displayHandlesChildrenSpecial.isOn);
        }

        displayHandlesChildrenSpecial.onValueChanged.AddListener(delegate {
            GameProgression.showSpecialHandleChildren = displayHandlesChildrenSpecial.isOn; GameController.selectingParts.ReselectAfterOneFrame(); childrenDepthAnimatedPanel.Toggle(displayHandlesChildrenSpecial.isOn); });
        displayHandlesChildrenDepth.onValueChanged.AddListener(delegate { GameProgression.showSpecialHandleChildrenDepth = (int)displayHandlesChildrenDepth.value; GameController.selectingParts.ReselectAfterOneFrame(); });

        cameraFocusToggle.onValueChanged.AddListener(delegate { GameController.cameraControl.Focus(); });
        undoButton.onClick.AddListener( delegate { UndoMachine.Undo(); });
        redoButton.onClick.AddListener(delegate { UndoMachine.Redo(); });
    }

    private void Update() {
        if (Input.GetButtonDown("Toggle GameState") && !GameController.pausedInput) {
            gameController.ToggleGameState();
        }
        cameraFocusToggle.isOn = GameController.cameraControl.focusing;
    }

    public void InitGameController(GameController gameCont) {
        gameController = gameCont;
        gameController.SetHeaderUIButtons(this);
        playButton.onClick.AddListener(delegate { gameController.PlayGame(); });
        stopButton.onClick.AddListener(delegate { gameController.StopGame(); });
    }

    public void ShowStopButton() {
        stopButton.transform.SetSiblingIndex(1);
        playButtonAnim.SetBool("OnScreen", false);
        stopButtonAnim.SetBool("OnScreen", true);

        progressionSlider.SetObjectiveSliderSmooth(0);
    }

    public void ShowPlayButton() {
        playButton.transform.SetSiblingIndex(1);
        playButtonAnim.SetBool("OnScreen", true);
        stopButtonAnim.SetBool("OnScreen", false);

        progressionSlider.SetObjectiveSliderSmooth(0);
    }

    public void VictoryButtons() {
        anim.SetBool("Victory", true);
    }
}

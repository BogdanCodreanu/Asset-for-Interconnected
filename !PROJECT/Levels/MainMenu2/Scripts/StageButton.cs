using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageButton : MonoBehaviour {
    private LevelButton[] levelButtons;
    public TMP_Text label;
    private LevelsButtonsHolder levelsButtonsHolder;
    private Button button;
    private UnityAction buttonAction;
    public LevelsButtonsHolder levelsButtonsHolderPrefab;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void Init(StagesLevelsButtons stagesLevelsButtons, StageInfo info, List<SceneInfo> scenesWithThisStage,
            Transform holderForLevelButtons) {
        label.text = info.stageName;
        levelsButtonsHolder = Instantiate(levelsButtonsHolderPrefab.gameObject, holderForLevelButtons).GetComponent<LevelsButtonsHolder>();
        levelsButtonsHolder.name = info.stageName + " - Levels Buttons";
        buttonAction = delegate {
            stagesLevelsButtons.CloseAnyLevelsButtonsHolder(levelsButtonsHolder);
            stagesLevelsButtons.SetActiveLevelsButtons(levelsButtonsHolder);
        };
        EnableButtonAction();
        levelsButtonsHolder.Init(info, scenesWithThisStage);
    }

    public void EnableButtonAction() {
        button.onClick.AddListener(buttonAction);
    }
    public void DisableButtonAction() {
        button.onClick.RemoveAllListeners();
    }
}

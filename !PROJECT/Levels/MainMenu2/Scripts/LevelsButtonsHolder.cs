using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelsButtonsHolder : MonoBehaviour {
    private Animator anim;
    public TMP_Text title;
    public LevelButton levelButtonPrefab;
    public Transform buttonsHolder;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void Init(StageInfo stageInfo, List<SceneInfo> scenesWithThisStage) {
        title.text = stageInfo.stageName;
        LevelButton spawn;
        foreach (SceneInfo si in scenesWithThisStage) {
            spawn = Instantiate(levelButtonPrefab.gameObject, buttonsHolder).GetComponent<LevelButton>();
            spawn.Init(si);
        }
    }

    public void EnterScreen() {
        anim.SetBool("OnScreen", true);
    }
    public void ExitScreen() {
        anim.SetBool("OnScreen", false);
    }
}

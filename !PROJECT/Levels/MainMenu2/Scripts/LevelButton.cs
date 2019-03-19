using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public TMP_Text text;
    //private SceneInfo sceneInfo;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void Init(SceneInfo si) {
        //sceneInfo = si;
        text.text = (string.IsNullOrEmpty(si.nickName)) ? si.thisSceneName : si.nickName;
        button.onClick.AddListener(delegate { SceneController.LoadSceneWithLoadingScreen(si.thisSceneName); });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Razziel.AnimatedPanels;

public class Tutorial4MechsCanvas : MonoBehaviour {
    public TMP_Text title;
    public Button closeButton;

    public Transform explanationsHolder;
    public Tutorial4MechsUIExplanation explanationPrefab;

    private Tutorial4MechsFrame[] frameSpawns;
    private Tutorial4MechsUIExplanation[] explanationSpawns;

    private GameObject framesHolder;
    private Tutorial4Mechs mainTutorial;

    public RectTransform slidingTransformAnimated;
    public Image blackBackground;
    private OpeningClosingPanel openingClosingPanel;

    private void Awake() {
        GameController.AddPause(this);
        closeButton.onClick.AddListener(delegate { CloseScreen(); });

    }

    public void Init(string title, Tutorial4Mechs tut) {
        this.title.text = title;
        mainTutorial = tut;
    }

    public void SpawnFramesAndMakeUIExplanations(Tutorial4MechsFrame[] frames) {
        framesHolder = new GameObject("Tutorial Frames Holder For " + title.text);
        frameSpawns = new Tutorial4MechsFrame[frames.Length];
        explanationSpawns = new Tutorial4MechsUIExplanation[frames.Length];

        for (int i = 0; i < frames.Length; i++) {
            frameSpawns[i] = Instantiate(frames[i].gameObject, framesHolder.transform).GetComponent<Tutorial4MechsFrame>();
            frameSpawns[i].SpawnObjects(new Vector3(10000 + i * 20, 1000, 0));
            explanationSpawns[i] = Instantiate(explanationPrefab.gameObject, explanationsHolder).GetComponent<Tutorial4MechsUIExplanation>();
            explanationSpawns[i].Init(frameSpawns[i].subtitle, frameSpawns[i].description, frameSpawns[i].GetRenderTextureCamera());
        }

        openingClosingPanel = new OpeningClosingPanel(slidingTransformAnimated, this, .4f, OpeningClosingPanel.OpeningPanelMode.Vertical);
        openingClosingPanel.AddAditionalFadingElement(new OpeningClosingPanel.AdditionalFadingElement(blackBackground, .5f));
        openingClosingPanel.SetActionAfterFade(delegate { Destroy(gameObject); GameController.RemovePause(this); });
        openingClosingPanel.AppearFromZero();
    }

    private void CloseScreen() {
        openingClosingPanel.FadeToZero();
    }

    private void OnDestroy() {
        //foreach (Tutorial4MechsFrame frame in frameSpawns) {
        //    Destroy(frame);
        //}
        Destroy(framesHolder);
        Destroy(mainTutorial);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Razziel.AnimatedPanels;


public class InfoUIPanelPopup : MonoBehaviour {
    const float fadeInsideElementsTime = .4f;
    const float rectResizeTime = .6f;


    public class ButtonAction {
        public string name;
        public UnityAction action;

        public ButtonAction(string name, UnityAction action) {
            this.name = name;
            this.action = action;
        }
    };

    public Image panelImage;
    public Button closeButton;
    public Image blackBacground;

    private OpeningClosingPanel openingClosingPanel;

    public TMP_Text title;
    public TMP_Text description;

    private RectTransform rectTrans;
    private bool killed;

    public UIButtonController buttonPrefab;
    public GameObject buttonsHolderLayoutHorizontal;
    private List<UIButtonController> allButtons;

    private void Awake() {
        closeButton.onClick.AddListener(delegate { KillByButton(); });
        rectTrans = panelImage.GetComponent<RectTransform>();
        //ButtonAction[] tests = new ButtonAction[2];
        //tests[0] = new ButtonAction("but1", delegate { InfoUI.InformationalClosablePopup("FUCK", "should not ckicled"); });
        //tests[1] = new ButtonAction("Button 2", null);
        //Init("test", "asdfasdfasdf", true, tests, true);
    }

    public void Init(string title, string description, bool titleWithStyle, ButtonAction[] buttonOptions, bool hasCloseButton) {
        if (titleWithStyle)
            this.title.text = "<style=Game>" + title + "</style>";
        else
            this.title.text = title;
        this.description.text = description;

        closeButton.gameObject.SetActive(hasCloseButton);

        UIButtonController spawn;
        allButtons = new List<UIButtonController>();
        if (buttonOptions != null)
            for (int i = 0; i < buttonOptions.Length; i++) {
                spawn = Instantiate(buttonPrefab, buttonsHolderLayoutHorizontal.transform).GetComponent<UIButtonController>();
                spawn.textOnButton.text = buttonOptions[i].name;
                spawn.SetCurrentAction(buttonOptions[i].action + delegate { KillByButton(); RemoveListenersFromButtons(); });
                allButtons.Add(spawn);
            }
        
        openingClosingPanel = new OpeningClosingPanel(rectTrans, this, .3f, OpeningClosingPanel.OpeningPanelMode.Vertical);
        openingClosingPanel.SetActionBeforeAppear(delegate { GameController.AddPause(this); });
        openingClosingPanel.SetActionAfterAppear(delegate { ResumeListenersFromButtons(); });
        openingClosingPanel.SetActionAfterFade(delegate { GameController.RemovePause(this); Destroy(gameObject); });
        openingClosingPanel.AddAditionalFadingElement(new OpeningClosingPanel.AdditionalFadingElement(blackBacground, .5f));
        openingClosingPanel.AppearFromZero();
    }

    private void RemoveListenersFromButtons() {
        foreach (UIButtonController butt in allButtons) {
            butt.PauseListener();
        }
    }
    private void ResumeListenersFromButtons() {
        foreach (UIButtonController butt in allButtons) {
            butt.ResumeSavedListener();
        }
    }

    private void KillByButton() {
        if (!killed) {
            openingClosingPanel.FadeToZero();
            killed = true;
        }
    }
}

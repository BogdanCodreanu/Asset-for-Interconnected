using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoUI : MonoBehaviour {
    public GameObject ErrorTextPrefab;
    private static GameObject ErrorText;
    private static MonoBehaviour mono;

    //public VerticalLayoutGroup vertLayoutPopup;
    //private static VerticalLayoutGroup vertLPopup;

    public InfoUIInformationalPopup informationalPopup;
    public Transform informationalPopupHolder;
    private static InfoUIInformationalPopup informationalPop;
    private static Transform informationalPopHolder;
    
    public InfoUIInformationalText informationalTextMoving;
    public Transform informationalTexMovingtHolder;
    private static InfoUIInformationalText informationalText;
    private static Transform informationalTextHolder;

    public InfoUIPanelPopup panelPopupPrefab;
    public Transform panelHolder;
    private static InfoUIPanelPopup panelPopup;
    private static Transform panelsHolder;

    private void Awake() {
        ErrorText = ErrorTextPrefab;
        mono = this;
        //vertLPopup = vertLayoutPopup;
        informationalPop = informationalPopup;
        informationalPopHolder = informationalPopupHolder;

        informationalText = informationalTextMoving;
        informationalTextHolder = informationalTexMovingtHolder;

        panelPopup = panelPopupPrefab;
        panelsHolder = panelHolder;
    }

    public static void ErrorMoving(string text, float displayDuration) {
        Text message = Instantiate(ErrorText, GameController.mainCanvasStatic.transform).GetComponent<Text>();
        message.text = text;
        mono.StartCoroutine(FadeTextMoving(displayDuration, 1f, message, 20f, mono));
    }

    public static void InformationalMoving(string text, float timeOnScreen = 2, float delayToSpawn = 0) {
        mono.ExecuteFunctionWithDelay(delayToSpawn, delegate {
            InfoUIInformationalText spawn = Instantiate(informationalText, informationalTextHolder).GetComponent<InfoUIInformationalText>();

            spawn.Init(text, timeOnScreen);
        });
    }

    public static void InformationalClosablePopup(string title, string text, float selfKillTime = 10, bool titleWithStyle = true) {
        InfoUIInformationalPopup spawn = Instantiate(informationalPop, informationalPopHolder).GetComponent<InfoUIInformationalPopup>();

        spawn.Init(title, text, selfKillTime, titleWithStyle);
        //Canvas.ForceUpdateCanvases();
        //vertLPopup.CalculateLayoutInputHorizontal();
        //vertLPopup.CalculateLayoutInputVertical();
        //vertLPopup.SetLayoutHorizontal();
        //vertLPopup.SetLayoutVertical();
    }

    public static void PanelPopup(string title, string text, InfoUIPanelPopup.ButtonAction[] avaliableActions, bool hasCloseButton, bool titleWithStyle = true) {
        InfoUIPanelPopup spawn = Instantiate(panelPopup, panelsHolder).GetComponent<InfoUIPanelPopup>();

        spawn.Init(title, text, titleWithStyle, avaliableActions, hasCloseButton);
    }

    private static IEnumerator FadeTextMoving(float displayDuration, float fadeDuration, Text message, float speed, MonoBehaviour mono) {
        Coroutine moved = mono.StartCoroutine(MoveTextUpwards(speed, message.transform, displayDuration + fadeDuration));
        yield return new WaitForSecondsRealtime(displayDuration);

        float startTime = Time.time;
        while (Time.time - startTime <= fadeDuration) {
            message.color = new Color(message.color.r, message.color.g, message.color.b, 1 - (Time.time - startTime) / fadeDuration);
            yield return new WaitForEndOfFrame();
        }
        yield return moved;
        Destroy(message.gameObject);
    }

    private static IEnumerator MoveTextUpwards(float speed, Transform text, float duration) {
        float startTime = Time.time;

        while (Time.time - startTime <= duration) {
            text.position += new Vector3(0, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
    }
}

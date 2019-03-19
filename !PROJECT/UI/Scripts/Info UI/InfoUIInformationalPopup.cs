using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InfoUIInformationalPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    const float fadeInsideElementsTime = .17f;
    const float rectResizeTime = .2f;
    const float secondsToSelfFade = 4f;

    public TMP_Text title;
    public TMP_Text description;
    public Image[] fadingImages;
    public Button closeButton;

    public Image panelImage;
    private RectTransform rectTrans;
    private bool killed;

    // initial alphas
    float titleAlphaInitial;
    float descriptionAlphaInitial;
    float[] imagesAlphaInitial;
    float initialRectHeight;
    float initialAlphaPanel;

    private float selfKillTime;
    private bool mouseOver;

    public Image glowingImage;
    private bool glowing;

    private void Awake() {
        rectTrans = GetComponent<RectTransform>();
        SetInitials();
        SetTransparent();
        
        //Init("Test", "description", 10, true);
    }

    public void Init(string title, string description, float selfKillTime, bool titleWithStyle) {
        if (titleWithStyle)
            this.title.text = "<style=Game>" + title + "</style>";
        else
            this.title.text = title;
        this.description.text = description;

        this.selfKillTime = selfKillTime;
        if (selfKillTime < secondsToSelfFade + 1) {
            selfKillTime = secondsToSelfFade + 1;
            Debug.LogWarning("self kill time less then 5 seconds. wtf");
        }
        TipsHistorySaver.AddTip(description);
        StartCoroutine(AppearFromNothing());
    }

    private void SetInitials() {
        initialRectHeight = rectTrans.sizeDelta.y;
        initialAlphaPanel = panelImage.color.a;
        titleAlphaInitial = title.color.a;
        descriptionAlphaInitial = description.color.a;
        imagesAlphaInitial = new float[fadingImages.Length];
        for (int i = 0; i < imagesAlphaInitial.Length; i++) {
            imagesAlphaInitial[i] = fadingImages[i].color.a;
        }
    }
    private void SetTransparent() {
        rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, 0);
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 0);
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        description.color = new Color(description.color.r, description.color.g, description.color.b, 0);
        foreach (Image i in fadingImages) {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        }
        glowingImage.color = new Color(glowingImage.color.r, glowingImage.color.g, glowingImage.color.b, 0);
    }
    private void SetOpaque() {
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, initialAlphaPanel);
        title.color = new Color(title.color.r, title.color.g, title.color.b, titleAlphaInitial);
        description.color = new Color(description.color.r, description.color.g, description.color.b, descriptionAlphaInitial);
        for (int i = 0; i < imagesAlphaInitial.Length; i++) {
            fadingImages[i].color = new Color(fadingImages[i].color.r, fadingImages[i].color.g, fadingImages[i].color.b, imagesAlphaInitial[i]);
        }
    }

	void Start () {
        closeButton.onClick.AddListener(delegate { KillByButton(); });

	}

    private void KillByButton() {
        if (!killed) {
            StartCoroutine(FadeToNothingByButton());
            killed = true;
        }
    }

    private IEnumerator FadeToNothingByButton() {
        SetInitials();
        float startTime = Time.time;
        float lerper = 0;

        while (lerper < 1f) {
            lerper = (Time.time - startTime) / fadeInsideElementsTime;
            title.color = new Color(title.color.r, title.color.g, title.color.b, Mathf.Lerp(titleAlphaInitial, 0, lerper));
            description.color = new Color(description.color.r, description.color.g, description.color.b, Mathf.Lerp(descriptionAlphaInitial, 0, lerper));
            for (int i = 0; i < imagesAlphaInitial.Length; i++) {
                fadingImages[i].color = new Color(fadingImages[i].color.r, fadingImages[i].color.g, fadingImages[i].color.b, Mathf.Lerp(imagesAlphaInitial[i], 0, lerper));
            }
            yield return new WaitForEndOfFrame();
        }
        startTime = Time.time;
        lerper = 0;

        while (lerper < 1f) {
            lerper = (Time.time - startTime) / rectResizeTime;
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, Mathf.Lerp(initialRectHeight, 0, lerper));
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, Mathf.Lerp(initialAlphaPanel, 0, lerper));

            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private IEnumerator AppearFromNothing() {
        float lerper = 0;
        float startTime = Time.time;

        while (lerper < 1f) {
            lerper = (Time.time - startTime) / rectResizeTime;
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, Mathf.Lerp(initialRectHeight, 0, 1 - lerper));
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, Mathf.Lerp(initialAlphaPanel, 0, 1 - lerper));
            yield return new WaitForEndOfFrame();
        }
        startTime = Time.time;
        lerper = 0;

        while (lerper < 1f) {
            lerper = (Time.time - startTime) / fadeInsideElementsTime;
            title.color = new Color(title.color.r, title.color.g, title.color.b, Mathf.Lerp(titleAlphaInitial, 0, 1 - lerper));
            description.color = new Color(description.color.r, description.color.g, description.color.b, Mathf.Lerp(descriptionAlphaInitial, 0, 1 - lerper));
            for (int i = 0; i < imagesAlphaInitial.Length; i++) {
                fadingImages[i].color = new Color(fadingImages[i].color.r, fadingImages[i].color.g, fadingImages[i].color.b, Mathf.Lerp(imagesAlphaInitial[i], 0, 1 - lerper));
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(KillingSlowlyByFade());
        glowing = true;
        StartCoroutine(GlowEffect());
    }

    private IEnumerator KillingSlowlyByFade() {
        float lerper = 0;
        float waitTime = selfKillTime - secondsToSelfFade;
        float startTime = Time.time;
        while(lerper < 1) {
            lerper = (Time.time - startTime) / waitTime;
            yield return new WaitForEndOfFrame();
        }

        startTime = Time.time;
        lerper = 0;

        while (lerper < 1f && !killed) {
            if (mouseOver || glowing) {
                SetOpaque();
                lerper = 0;
                startTime = Time.time;
            }
            else {

                lerper = (Time.time - startTime) / secondsToSelfFade;
                title.color = new Color(title.color.r, title.color.g, title.color.b, Mathf.Lerp(titleAlphaInitial, 0, lerper));
                description.color = new Color(description.color.r, description.color.g, description.color.b, Mathf.Lerp(descriptionAlphaInitial, 0, lerper));
                for (int i = 0; i < imagesAlphaInitial.Length; i++) {
                    fadingImages[i].color = new Color(fadingImages[i].color.r, fadingImages[i].color.g, fadingImages[i].color.b, Mathf.Lerp(imagesAlphaInitial[i], 0, lerper));
                }

                panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, Mathf.Lerp(initialAlphaPanel, 0, lerper * .5f));
            }
            yield return new WaitForEndOfFrame();
        }

        startTime = Time.time;
        lerper = 0;
        Color panelColorNow = panelImage.color;

        while (lerper < 1f && !killed) {
            lerper = (Time.time - startTime) / rectResizeTime;
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, Mathf.Lerp(initialRectHeight, 0, lerper));
            panelImage.color = new Color(panelColorNow.r, panelColorNow.g, panelColorNow.b, Mathf.Lerp(panelColorNow.a, 0, lerper));

            yield return new WaitForEndOfFrame();
        }

    }

    private IEnumerator GlowEffect() {
        glowingImage.Fade(0, .4f, 1f);
        yield return new WaitForSeconds(1f);

        float sinuser = 0;

        while (glowing) {
            glowingImage.SetAlpha(Mathf.Sin(sinuser) * .2f + .4f);
            sinuser += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        glowingImage.Fade(glowingImage.color.a, 0, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
        glowing = false;
    }
    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
    }
}

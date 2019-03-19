using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanel : MonoBehaviour {
    private const float fadeTime = .15f;
    public TMP_Text mainText;
    public TMP_Text smallDescription;
    public TMP_Text shortCutText;

    [HideInInspector] public RectTransform rectTrans;
    private UIFadeController fadeController;

    private bool stoppedFollowing = false;

    private void Awake() {
        fadeController = GetComponent<UIFadeController>();
        rectTrans = GetComponent<RectTransform>();
        CheckPivotAndSize();
    }

    public void SetText(string text) {
        mainText.text = "<style=SmallMedium>" + text + "</style>";
    }
    public void SetShortcut(string shortcut) {
        shortCutText.text = "<style=SmallMedium>Shortcut <i>(<style=Game>" + shortcut.ToUpper() + "</style>)</i></style>";
    }
    public void SetSmallDescription(string text) {
        if (GameSettings.showSmallDescriptionOnHover)
            smallDescription.text = "<style=Small>" + text + "</style>";
    }

    public void OnDestroy() {
        Destroy(gameObject);
    }

    private void Update() {
        if (!stoppedFollowing) {
            transform.position = Input.mousePosition;
            CheckPivotAndSize();
        }
    }

    private void StopFollowingMouse() {
        stoppedFollowing = true;
    }

    /// <summary>
    /// Checks pivot and size, and also sets the fading controller to fade.
    /// </summary>
    public void Init() {
        CheckPivotAndSize();
        fadeController.SetToZeroAll();
        fadeController.Appear(fadeTime);
    }

    /// <summary>
    /// Destroies the object after it fades away. Also stops following mouse.
    /// </summary>
    public void KillWithFade() {
        StopFollowingMouse();
        Destroy(gameObject, fadeTime);
        fadeController.Disappear(fadeTime);
    }

    private void CheckPivotAndSize() {
        rectTrans.pivot = new Vector3(1, 1);
        if (rectTrans.anchoredPosition.y - rectTrans.sizeDelta.y <= 0) {
            rectTrans.pivot = new Vector2(rectTrans.pivot.x, 0);
        }
        if (rectTrans.anchoredPosition.x - rectTrans.sizeDelta.x <= 0) {
            rectTrans.pivot = new Vector2(0, rectTrans.pivot.y);
        }
    }
}

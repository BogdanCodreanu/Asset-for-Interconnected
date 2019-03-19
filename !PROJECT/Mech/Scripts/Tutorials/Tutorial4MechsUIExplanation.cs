using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial4MechsUIExplanation : MonoBehaviour {

    public TMP_Text subtitle;
    public TMP_Text description;
    public RawImage rawImageForCamera;
    public AspectRatioFitter rawImageAspectRatioFitter;

    private void Awake() {
        rawImageAspectRatioFitter.aspectRatio = (float)Screen.width / Screen.height;
    }

    public void Init(string subtitle, string description, RenderTexture cameraRenderTexture) {
        this.subtitle.text = subtitle;
        this.description.text = description;
        rawImageForCamera.texture = cameraRenderTexture;
    }
}

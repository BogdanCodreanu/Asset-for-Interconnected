using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SettingManager : MonoBehaviour {
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public TMP_Dropdown vSyncDropDown;
    public TMP_Dropdown textureQualityDropDown;
    public Toggle lowGraphicsToggle;

    public Button resetDefaultGameSettings;

    public Slider handlesScaleSlider;
    public Toggle handlesShowDescription;
    public Slider cameraScrollingSpeed;
    public Slider cameraSmoothness;
    public Toggle snapCameraOnStop;
    public Toggle snapCameraOnStart;
    public Toggle smallDescriptionHover;

    public Button backButton;

    private Resolution[] resolutions;
    private Settings4File settings4File;

    public Animator settingsPanelAnimator;

    private Action onFinishExittingScreenAction;

    private void Awake() {
        settings4File = new Settings4File();

        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });
        vSyncDropDown.onValueChanged.AddListener(delegate { OnVsyncChange(); });
        textureQualityDropDown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        lowGraphicsToggle.onValueChanged.AddListener(delegate { OnLowGraphics(); });

        resetDefaultGameSettings.onClick.AddListener(delegate { GameDefaultSettings(); });

        handlesScaleSlider.onValueChanged.AddListener(delegate { OnHandleScaleSlider(); });
        handlesShowDescription.onValueChanged.AddListener(delegate { OnHandleShowDescription(); });
        cameraScrollingSpeed.onValueChanged.AddListener(delegate { OnCameraScrollingSpeed(); });
        cameraSmoothness.onValueChanged.AddListener(delegate { OnCameraScrollSmoothness(); });
        snapCameraOnStop.onValueChanged.AddListener(delegate { OnSnapCameraToMainCube(); });
        snapCameraOnStart.onValueChanged.AddListener(delegate { OnSnapCameraToMainCubeOnStart(); });
        smallDescriptionHover.onValueChanged.AddListener(delegate { OnSmallDisplayHover(); });
        backButton.onClick.AddListener(delegate { ApplySettingsToFile(); ExitScreen(); });

        resolutions = Screen.resolutions;
        Array.Reverse(resolutions);

        foreach (Resolution res in resolutions) {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.ToString()));
        }
        
        LoadSettings();

        resolutionDropdown.RefreshShownValue();
        GameSettings.showHandlesDescriptions = true;
    }

    public void OnFullScreenToggle() {
        settings4File.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }
    public void OnResolutionChange() {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        settings4File.resolutionIndex = resolutionDropdown.value;
    }
    public void OnVsyncChange() {
        QualitySettings.vSyncCount = settings4File.vSync = vSyncDropDown.value;
    }
    public void OnTextureQualityChange() {
        QualitySettings.masterTextureLimit = settings4File.textureQuality = textureQualityDropDown.value;
    }
    public void OnHandleScaleSlider() {
        GameSettings.handlesScale = settings4File.handlesScale = Mathf.RoundToInt(handlesScaleSlider.value);
    }
    public void OnHandleShowDescription() {
        GameSettings.showHandlesDescriptions = settings4File.handlesShowDescription = handlesShowDescription.isOn;
    }
    public void OnCameraScrollingSpeed() {
        GameSettings.cameraScrollSpeed = settings4File.cameraScrollSpeed = Mathf.RoundToInt(cameraScrollingSpeed.value);
    }
    public void OnCameraScrollSmoothness() {
        GameSettings.cameraSmoothness = settings4File.cameraSmoothness = Mathf.RoundToInt(cameraSmoothness.value);
    }
    public void OnSnapCameraToMainCube() {
        GameSettings.snapToMainCubeOnStop = settings4File.snapCameraToMainCube = snapCameraOnStop.isOn;
    }
    public void OnSnapCameraToMainCubeOnStart() {
        GameSettings.snapToMainCubeOnStart = settings4File.snapCameraToMainCubeOnStart = snapCameraOnStart.isOn;
    }
    public void OnSmallDisplayHover() {
        GameSettings.showSmallDescriptionOnHover = settings4File.showSmallDescriptionOnHover = smallDescriptionHover.isOn;
    }
    public void OnLowGraphics() {
        settings4File.lowGraphics = lowGraphicsToggle.isOn;
        Shader.SetGlobalFloat("_LowGraphicsMode", (settings4File.lowGraphics) ? 1 : 0);
    }

    private void SaveSettings() {
        string jsonData = JsonUtility.ToJson(settings4File, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.txt", jsonData);
    }

    public void ApplySettingsToFile() {
        SaveSettings();
    }

    public void LoadSettings() {
        if (File.Exists(Application.persistentDataPath + "/gamesettings.txt")) {
            settings4File = JsonUtility.FromJson<Settings4File>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.txt"));


            fullscreenToggle.isOn = settings4File.fullscreen;
            Screen.fullScreen = settings4File.fullscreen;
            if (settings4File.resolutionIndex < resolutions.Length) {
                resolutionDropdown.value = settings4File.resolutionIndex;
            } else {
                resolutionDropdown.value = 0;
                settings4File.resolutionIndex = 0;
            }
            Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
            QualitySettings.vSyncCount = vSyncDropDown.value = settings4File.vSync;
            QualitySettings.masterTextureLimit = textureQualityDropDown.value = settings4File.textureQuality;
            lowGraphicsToggle.isOn = settings4File.lowGraphics; Shader.SetGlobalFloat("_LowGraphicsMode", (settings4File.lowGraphics) ? 1 : 0);

            GameSettings.handlesScale = settings4File.handlesScale; handlesScaleSlider.value = settings4File.handlesScale;
            GameSettings.showHandlesDescriptions = handlesShowDescription.isOn = settings4File.handlesShowDescription;
            GameSettings.cameraScrollSpeed = settings4File.cameraScrollSpeed; cameraScrollingSpeed.value = settings4File.cameraScrollSpeed;
            GameSettings.cameraSmoothness = settings4File.cameraSmoothness; cameraSmoothness.value = Mathf.RoundToInt(settings4File.cameraSmoothness);
            GameSettings.snapToMainCubeOnStop = snapCameraOnStop.isOn = settings4File.snapCameraToMainCube;
            GameSettings.snapToMainCubeOnStart = snapCameraOnStart.isOn = settings4File.snapCameraToMainCubeOnStart;
            GameSettings.showSmallDescriptionOnHover = smallDescriptionHover.isOn = settings4File.showSmallDescriptionOnHover;

        }
        else { // default settings
            VideoDefaultSettings();
            GameDefaultSettings();
            SaveSettings();
        }
        GameSettings.settingsLoaded = true;
    }

    private void VideoDefaultSettings() {
        Screen.SetResolution(resolutions[0].width, resolutions[0].height, Screen.fullScreen);
        resolutionDropdown.value = settings4File.resolutionIndex = 0;

        settings4File.fullscreen = Screen.fullScreen = fullscreenToggle.isOn = true;
        QualitySettings.vSyncCount = settings4File.vSync = vSyncDropDown.value = 1;
        QualitySettings.masterTextureLimit = settings4File.textureQuality = textureQualityDropDown.value = 0;
        lowGraphicsToggle.isOn = settings4File.lowGraphics = false; Shader.SetGlobalFloat("_LowGraphicsMode", 0);
    }

    private void GameDefaultSettings() {
        GameSettings.handlesScale = settings4File.handlesScale = 40; handlesScaleSlider.value = 40; // min = 15. max = 70
        GameSettings.showHandlesDescriptions = handlesShowDescription.isOn = settings4File.handlesShowDescription = true;
        GameSettings.cameraScrollSpeed = settings4File.cameraScrollSpeed = 60; cameraScrollingSpeed.value = 60; // min = 20. max = 200
        GameSettings.cameraSmoothness = settings4File.cameraSmoothness = 90; cameraSmoothness.value = 90;
        GameSettings.snapToMainCubeOnStop = snapCameraOnStop.isOn = settings4File.snapCameraToMainCube = true;
        GameSettings.snapToMainCubeOnStart = snapCameraOnStart.isOn = settings4File.snapCameraToMainCubeOnStart = true;
        GameSettings.showSmallDescriptionOnHover = smallDescriptionHover.isOn = settings4File.showSmallDescriptionOnHover = true;
    }

    public void ExitScreen() {
        settingsPanelAnimator.SetBool("OnScreen", false);
        StopAllCoroutines();
    }
    public void EnterScreen() {
        settingsPanelAnimator.SetBool("OnScreen", true);
        StopAllCoroutines();
    }

    public void SetActionOnFinishExittingScreen(Action action) {
        onFinishExittingScreenAction = action;
    }
    public void OnFinishedExittingScreen() {
        if (onFinishExittingScreenAction != null)
            onFinishExittingScreenAction();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beebyte.Obfuscator;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SavedMachineButton : MonoBehaviour {
    public TMP_Text title;
    public Button overwriteButton;
    public Button loadButton;
    public Button deleteButton;
    public RawImage screenshotImage;

    private SavingLoadingPanelController controller;
    public enum SavedMachineButtonMode { Saving, Loading };
    private string filename;

    private SavedMechanical savedMech;

    public void Init(SavedMachineButtonMode mode, string filename, SavingLoadingPanelController controller) {
        this.controller = controller;
        this.filename = filename;
        title.text = filename;
        if (mode == SavedMachineButtonMode.Loading) {
            overwriteButton.gameObject.SetActive(false);
            loadButton.onClick.AddListener(() => { OnLoadButton(); });
        } else {
            loadButton.gameObject.SetActive(false);
            overwriteButton.onClick.AddListener(() => { OnOverwriteButton(); });
        }
        deleteButton.onClick.AddListener(() => { OnDeleteButton(); });

        savedMech = SaveMachine.LoadFromFile(filename);
        if (savedMech != null) {
            MakeThumbnail();
        }
    }

    private void MakeThumbnail() {
        Texture2D screenshotTexture = new Texture2D(savedMech.ssWidth, savedMech.ssHeight, TextureFormat.RGB24, false);
        screenshotTexture.LoadRawTextureData(savedMech.screenshot);
        screenshotTexture.Apply();
        screenshotImage.texture = screenshotTexture;
    }

    [SkipRename]
    private void OnOverwriteButton() {
        InfoUIPanelPopup.ButtonAction[] actionsAvaliable = new InfoUIPanelPopup.ButtonAction[2];
        actionsAvaliable[0] = new InfoUIPanelPopup.ButtonAction("Yes", YesOverwrite);
        actionsAvaliable[1] = new InfoUIPanelPopup.ButtonAction("No", null);
        InfoUI.PanelPopup("Overwrite Action", "Are you sure you want to overwrite machine \"" + filename + "\"?", actionsAvaliable, false);
    }

    [SkipRename]
    private void OnLoadButton() {
        if (savedMech != null) {
            SaveMachine.LoadAndSet(savedMech);
            UndoMachine.Record();
            CloseEntirePanel();
        } else {
            InfoUIPanelPopup.ButtonAction[] actionsAvaliable = new InfoUIPanelPopup.ButtonAction[2];
            actionsAvaliable[0] = new InfoUIPanelPopup.ButtonAction("Yes", YesDelete);
            actionsAvaliable[1] = new InfoUIPanelPopup.ButtonAction("No", null);
            InfoUI.PanelPopup("Delete Action", "This machine is invalaid. Do you want to delete it?", actionsAvaliable, false);
        }
    }

    [SkipRename]
    private void OnDeleteButton() {
        InfoUIPanelPopup.ButtonAction[] actionsAvaliable = new InfoUIPanelPopup.ButtonAction[2];
        actionsAvaliable[0] = new InfoUIPanelPopup.ButtonAction("Yes", YesDelete);
        actionsAvaliable[1] = new InfoUIPanelPopup.ButtonAction("No", null);
        InfoUI.PanelPopup("Delete Action", "Are you sure you want to delete machine \"" + filename + "\"?", actionsAvaliable, false);
    }

    private void YesDelete() {
        Destroy(gameObject);
        File.Delete(SaveMachine.directoryName + "/" + filename);
    }

    private void YesOverwrite() {
        SaveMachine.SaveToFile(filename, SaveMachine.SaveAndGet(true));
        CloseEntirePanel();
    }

    private void CloseEntirePanel() {
        controller.OnBackButton();
    }
}

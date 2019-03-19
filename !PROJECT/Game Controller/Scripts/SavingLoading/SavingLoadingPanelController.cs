using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Beebyte.Obfuscator;
using TMPro;
using System.Text.RegularExpressions;
using Razziel.AnimatedPanels;

public class SavingLoadingPanelController : MonoBehaviour {
    public RectTransform holder;
    private OpeningClosingPanel openingClosingPanel;
    public Button saveEnterButton;
    public Button loadEnterButton;
    public Button backButton;

    public TMP_Text screenTitle;
    public GameObject enterInputSave;

    public Transform machineButtonsHolder;
    List<SavedMachineButton> machineButtonsSpawns;
    public SavedMachineButton machineButtonPrefab;

    public TMP_InputField inputField;
    public Button doneInputButton;

    private void Awake() {
        openingClosingPanel = new OpeningClosingPanel(holder, this, .3f, OpeningClosingPanel.OpeningPanelMode.FadeOnly);
        openingClosingPanel.SetActionBeforeAppear(() => { holder.gameObject.SetActive(true); });
        openingClosingPanel.SetActionAfterFade(() => { holder.gameObject.SetActive(false); AfterFade(); });

        saveEnterButton.onClick.AddListener(() => { openingClosingPanel.AppearFromZero(); EnterSaveMode(); GameController.AddPause(this); });
        loadEnterButton.onClick.AddListener(() => { openingClosingPanel.AppearFromZero(); EnterLoadMode(); GameController.AddPause(this); });
        backButton.onClick.AddListener(() => { OnBackButton(); });

        doneInputButton.onClick.AddListener(() => { OnDoneInputNewMachine(); });
    }

    [SkipRename]
    private void EnterSaveMode() {
        enterInputSave.gameObject.SetActive(true);
        screenTitle.text = "Save Machine";
        GenerateButtons(SavedMachineButton.SavedMachineButtonMode.Saving);
    }

    [SkipRename]
    private void EnterLoadMode() {
        enterInputSave.gameObject.SetActive(false);
        screenTitle.text = "Load Machine";
        GenerateButtons(SavedMachineButton.SavedMachineButtonMode.Loading);
    }

    [SkipRename]
    private void AfterFade() {
        if (machineButtonsSpawns != null)
            foreach (SavedMachineButton button in machineButtonsSpawns) {
                if (button)
                    Destroy(button.gameObject);
            }
    }

    [SkipRename]
    public void OnBackButton() {
        openingClosingPanel.FadeToZero();
        GameController.RemovePause(this);
    }

    [SkipRename]
    private void OnDoneInputNewMachine() {
        if (inputField.text.Length > 0) {
            SaveMachine.SaveToFile(inputField.text, SaveMachine.SaveAndGet(true));
            OnBackButton();
        }
    }

    private void GenerateButtons(SavedMachineButton.SavedMachineButtonMode mode) {
        if (Directory.Exists(SaveMachine.directoryName)) {
            if (machineButtonsSpawns != null) {
                machineButtonsSpawns.Clear();
            }
            machineButtonsSpawns = new List<SavedMachineButton>();
            SavedMachineButton spawn;
            string[] allFiles = Directory.GetFiles(SaveMachine.directoryName + "/");

            foreach (string filepath in allFiles) {
                string filename = Regex.Replace(filepath, SaveMachine.directoryName + "/(.+)", "$1");
                spawn = Instantiate(machineButtonPrefab.gameObject, machineButtonsHolder).GetComponent<SavedMachineButton>();
                spawn.Init(mode, filename, this);
                machineButtonsSpawns.Add(spawn);
            }
        }
    }

}

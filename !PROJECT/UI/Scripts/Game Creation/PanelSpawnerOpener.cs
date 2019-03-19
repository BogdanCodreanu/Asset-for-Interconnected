using UnityEngine;
using UnityEngine.UI;

public class PanelSpawnerOpener : MonoBehaviour {
    public PanelSpawnerUI spawningPanel;
    private bool ableToOpen;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { ClickedButton(); });
    }

    private void ClickedButton() {
        if (!GameController.pausedInput) {
            if (!spawningPanel.onScreen) {
                if (ableToOpen) {
                    spawningPanel.EnterScreen();
                }
            }
            else {
                spawningPanel.ExitScreen();
            }
        }
    }

    private void Update() {
        ableToOpen = !SelectingParts.draggingSomething;
    }
}

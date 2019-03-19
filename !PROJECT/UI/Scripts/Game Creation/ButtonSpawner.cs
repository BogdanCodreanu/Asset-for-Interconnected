using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSpawner : MonoBehaviour {
    private int prefabIndex;
    private SpawningParts spawningParts;
    public TMP_Text mechTitle;
    public Image image;
    public Button tutorialButton;

    private Button button;

    public void AssignJob(SpawningParts spawningPrts, int i, MechanicalPart mech) {
        button = GetComponent<Button>();
        prefabIndex = i;
        spawningParts = spawningPrts;
        mechTitle.text = mech.mechName;
        image.sprite = mech.GetComponent<SpriteRenderer>().sprite;
        if (mech.myTutorial)
            tutorialButton.onClick.AddListener(delegate { mech.SpawnTutorial(); });
        else
            Destroy(tutorialButton.gameObject);
    }

    public void SpawnObject() {
        if (button.interactable && GameController.GetGameState() == GameController.GameState.CREATING) {
            spawningParts.SpawnMech(prefabIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroTextMessage : MonoBehaviour {
    public GameObject introMessagesObjToTurnOn;
    public TMP_Text titleText;
    public TMP_Text stageText;
    public TMP_Text objectiveText;

    public float showDuration = 5f;
    public float fadeDuration = 4f;

    private void Awake() {
        List<TMP_Text> textsToFade = new List<TMP_Text>();
        introMessagesObjToTurnOn.SetActive(true);
        textsToFade.Add(titleText);
        textsToFade.Add(stageText);
        textsToFade.Add(objectiveText);
        AssignTexts();

        //foreach (TMP_Text text in textsToFade) {
        //    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        //}
        StartCoroutine(ShowTexts(textsToFade));
    }

    private void AssignTexts() {
        if (DatabasesManager.IsValidScene()) {
            titleText.text = DatabasesManager.GetSceneNickname();
            stageText.text = DatabasesManager.GetStageName();
            objectiveText.text = "<style=Game>" + DatabasesManager.GetObjectiveText() + "</style>";
        }
    }

    private IEnumerator ShowTexts(List<TMP_Text> textsToFade) {
        yield return new WaitForSeconds(showDuration);
        foreach (TMP_Text text in textsToFade) {
            text.Fade(1, 0, fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);
        introMessagesObjToTurnOn.SetActive(false);
        Destroy(gameObject);
    }
}

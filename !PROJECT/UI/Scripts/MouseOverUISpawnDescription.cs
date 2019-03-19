using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverUISpawnDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public ItemDescriptionPanel itemDescriptionPrefab;
    private ItemDescriptionPanel spawn;

    public float secondsUntilSpawn = 0;
    [TextArea]
    public string description;
    [TextArea]
    public string smallDescription;
    public string inputAxisNameShortcut;

    private bool counting;
    private float counter;

    private Transform canvasTransform;

    private void Awake() {
        if (GameController.mainCanvasStatic) {
            canvasTransform = GameController.mainCanvasStatic.transform;
        } else {
            canvasTransform = FindObjectOfType<Canvas>().transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        counter = 0;
        counting = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        counting = false;

        if (spawn) {
            spawn.KillWithFade();
        }
    }

    public void Update() {
        if (counting) {
            counter += Time.deltaTime;
            if (counter >= secondsUntilSpawn) {
                counting = false;
                SpawnDescriptionBox();
            }
        }
    }
    private void SpawnDescriptionBox() {
        bool makeSpwn = false;
        if (!string.IsNullOrEmpty(description))
            makeSpwn = true;

        if (GameSettings.showSmallDescriptionOnHover && !string.IsNullOrEmpty(smallDescription))
            makeSpwn = true;

        if (makeSpwn) {
            spawn = Instantiate(itemDescriptionPrefab.gameObject, Input.mousePosition, Quaternion.identity, canvasTransform).GetComponent<ItemDescriptionPanel>();
            spawn.Init();

            if (!string.IsNullOrEmpty(inputAxisNameShortcut)) {
                spawn.SetShortcut(inputAxisNameShortcut);
            }
            if (!string.IsNullOrEmpty(description)) {
                spawn.SetText(description);
            }
            if (!string.IsNullOrEmpty(smallDescription)) {
                spawn.SetSmallDescription(smallDescription);
            }
        }
        counting = false;
    }

    private void OnDestroy() {
        if (spawn)
            Destroy(spawn);
    }
}

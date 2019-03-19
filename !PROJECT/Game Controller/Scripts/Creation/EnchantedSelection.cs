using System.Collections;
using UnityEngine;

public class EnchantedSelection : MonoBehaviour {
    private SelectingParts selectingParts;
    private GameController gameController;
    public GameObject circleSelectionPrefab;
    private ScalingSpawnDie circleSpawn;

    private bool enchanted = false;
    //private float keyHoldingCounter;
    private bool circleFreeScaled;
    private Vector3 circleInitialSpawned;
    private float circleRadius;

    public Material selectionMaterial;
    public Color insideCircleColor;
    private Collider2D[] colsInCircle;
    private SelectionHover[] selectionsHover;
    private MechanicalPart mechAux;

    private Transform[] movedObjects;

    public float minimumRadius = 2f;
    private bool onCooldown;
    private bool willCreateCooldown;

    private void Start() {
        selectingParts = GetComponent<SelectingParts>();
        gameController = GetComponent<GameController>();
    }

    private void Update() {
        if (GameController.currentGameState == GameController.GameState.CREATING && !onCooldown) {
            if (Input.GetButtonDown("EnchantedSelect") && !GameController.pausedInput) {
                enchanted = !enchanted;
            }
            Logic();

            if (Input.GetMouseButtonDown(0) && enchanted)
                KillCircle();
        }
        
    }

    private void Logic() {
        if (Input.GetButtonDown("EnchantedSelect")) {
            SpawnCircle();
            // keyHoldingCounter = 0f;
            circleInitialSpawned = GlobalMousePosition.mouseWorldPos;
        }
        if (enchanted) {
            if (Input.GetButton("EnchantedSelect")) {
                // keyHoldingCounter += Time.deltaTime;
                // if (keyHoldingCounter >= 0) {
                circleFreeScaled = true;
                // }
                selectingParts.DisableViaEnchantedSelection();
            }

            if (circleFreeScaled) {
                circleRadius = (GlobalMousePosition.mouseWorldPos - circleInitialSpawned).magnitude;
                circleSpawn.transform.localScale = Vector3.one * circleRadius * 2f;
                CircleFreeScaledHovers();
            }

            if (Input.GetButtonUp("EnchantedSelect")) {
                circleFreeScaled = false;
                selectingParts.EnableViaEnchantedSelection();
                MoveObjects();
            }
        }

        if (!enchanted && Input.GetButtonDown("EnchantedSelect")) {
            KillCircle();
        }
    }

    private void SpawnCircle() {
        if (enchanted) {
            circleSpawn = Instantiate(circleSelectionPrefab, GlobalMousePosition.mouseWorldPos, Quaternion.identity, null).GetComponent<ScalingSpawnDie>();
            //circleSpawn.ScaleTo(Vector3.one, .1f);
        }
    }

    private void CircleFreeScaledHovers() {
        KillHovers();

        
        colsInCircle = Physics2D.OverlapCircleAll(circleInitialSpawned, circleRadius);
        selectionsHover = new SelectionHover[colsInCircle.Length];

        for (int i = 0; i < colsInCircle.Length; i++) {
            mechAux = colsInCircle[i].GetComponent<MechanicalPart>();

            if (mechAux) {
                if (!mechAux.mainCube)
                    selectionsHover[i] = SelectionFuncs.CreateSelectionOver(colsInCircle[i], selectionMaterial, insideCircleColor, "Enchanted Hover", -1);
            }
        }
    }

    private void KillHovers() {
        if (selectionsHover != null) {
            foreach (SelectionHover s in selectionsHover) {
                Destroy(s);
            }
        }
    }

    private void ResetCooldown() {
        onCooldown = false;
    }

    public void KillCircle() {
        KillHovers();
        gameController.AssignAllPartsTransforms();
        if (circleSpawn) {
            circleSpawn.Die(.1f);
            circleSpawn = null;
        }
        enchanted = false;
        selectingParts.DisableEnchantedHovering();
        SpriteRenderer rend;
        if (movedObjects != null) {
            foreach (Transform t in movedObjects) {
                rend = t.GetComponent<SpriteRenderer>();
                rend.sortingLayerName = "Mechanism";
            }
        }
        if (willCreateCooldown) {
            onCooldown = true;
            this.ExecuteFunctionWithDelay(.5f, ResetCooldown);
        }
    }

    private void MoveObjects() {
        if (selectionsHover != null) {
            int nrOfValidObjects = 0;
            int i = 0;
            foreach (SelectionHover s in selectionsHover) {
                if (s) {
                    nrOfValidObjects++;
                }
            }
            if (nrOfValidObjects < 1) {
                KillCircle();
                return;
            }

            movedObjects = new Transform[nrOfValidObjects];

            i = 0;
            foreach (SelectionHover s in selectionsHover) {
                if (s) {
                    s.StopFollowing();
                    movedObjects[i] = s.followedObject;
                    i++;
                }
            }
            gameController.RememberAllPartsTransforms();

            float angle = 360f / nrOfValidObjects * Mathf.Deg2Rad;
            
            Vector3 pushingAngle;
            SpriteRenderer rend;
            i = 0;
            if (circleRadius < minimumRadius) {
                circleRadius = minimumRadius;
            }

            foreach (Transform t in movedObjects) {
                if (t) {
                    pushingAngle = new Vector3(Mathf.Cos(angle * i), Mathf.Sin(angle * i));
                    StartCoroutine(MoveTowardsNicely(t, circleInitialSpawned + pushingAngle * circleRadius, .4f));
                    i++;

                    rend = t.GetComponent<SpriteRenderer>();
                    rend.sortingLayerName = "Mechanism Enchanted Selection";
                }
            }
            selectingParts.EnableEnchantedHovering(selectionsHover, movedObjects);

            willCreateCooldown = true;
        } else {
            KillCircle();
        }
    }

    private static IEnumerator MoveTowardsNicely(Transform mover, Vector3 towards, float duration) {
        float startTime = Time.time;
        Vector3 initialPos = mover.position;
        while (Time.time - startTime <= duration) {
            mover.transform.position = Vector3.Lerp(initialPos, towards, (Time.time - startTime) / duration);
            yield return new WaitForEndOfFrame();
        }
        mover.transform.position = towards;
    }
}

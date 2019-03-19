using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static List<MechanicalPart> mechs;
    public static List<UnitControl> units;
    public static List<EnviormentObject> envObjects;


    public enum GameState { CREATING, PLAYING };
    public static GameState currentGameState;

    public static GameState GetGameState() { return currentGameState; }
    public void SetGameState(GameState setTo) { currentGameState = setTo; }
    
    public float slowmotionTime = .1f;
    private bool isSlowmotion;

    //private static bool needToResetList;

    public static MechanicalPart mainCube;
    
    public static CameraControl cameraControl;

    private CreationBounds creationBounds;

    private RememberedTransform[] rememberedTransforms;
    private bool isRememberingTransforms;
    private static bool canPlayGameFromEnchantedSelectionComingBack;
    private static bool clickedPlayWhileNotBeingAble;
    
    private static SavingPrefab savingPrefab;

    //private static MainCubeRecoreder mainCubeRecorder;

    private static HeaderUI headerUI;
    public static MainCanvas mainCanvasStatic;

    public static bool notWorkinDueTutorial;
    public static GameController gameController;
    
    private static List<MonoBehaviour> pausedReasons;
    public static bool pausedInput;

    public static SelectingParts selectingParts;

    private static SavedMechanical betweenLevelsMachine;

    private void SetStatics() {
        mechs = new List<MechanicalPart>();
        units = new List<UnitControl>();
        envObjects = new List<EnviormentObject>();
    }

    private void Awake() {
        SetStatics();

        SetTimeScale(1);
        gameController = this;
        cameraControl = Camera.main.GetComponent<CameraControl>();

        mainCanvasStatic = FindObjectOfType<MainCanvas>();

        ResetMechList();
        ResetUnitList();
        ResetEnviormentObjects();

        creationBounds = GetComponent<CreationBounds>();
        canPlayGameFromEnchantedSelectionComingBack = true;
        
        savingPrefab = GetComponent<SavingPrefab>();
        savingPrefab.mainCube = mainCube;

        currentGameState = GameState.CREATING;

        if (mainCube.transform.position != Vector3.zero) {
            Debug.Log("Main cube moved to vector3.zero");
            mainCube.transform.position = Vector3.zero;
        }
        notWorkinDueTutorial = false;
        
        pausedReasons = new List<MonoBehaviour>();
        pausedInput = false;
        clickedPlayWhileNotBeingAble = false;

        selectingParts = GetComponent<SelectingParts>();
        
        if (betweenLevelsMachine != null) {
            SaveMachine.LoadAndSet(betweenLevelsMachine);
        }
        mainCube.transform.position = Vector3.zero;
        UndoMachine.Record();
    }

    private void Start() {
        creationBounds.SpawnBounds();
        foreach (MechanicalPart m in mechs) {
            if (m) {
                m.SaveLifeProperties();
            }
        }
    }

    private void Update() {
        TestPausing();
        if (Input.GetButtonDown("Slowmotion") && currentGameState == GameState.PLAYING) {
            SetTimeScale((isSlowmotion) ? 1 : slowmotionTime);
            isSlowmotion = !isSlowmotion;
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            string logger = mechs.Count + " mechs: ";
            foreach (MechanicalPart m in mechs) {
                logger += m.mechName + " ";
            }
            Debug.Log(logger);
        }
    }

    public static void DestroyExistingMechs() {
        mainCube = null;
        foreach (MechanicalPart m in mechs) {
            m.SetScriptAsDestroied();
            Destroy(m);
        }
    }

    public static void ResetMechList() {
        MechanicalPart[] allMechs = FindObjectsOfType<MechanicalPart>();
        mechs.Clear();
        for (int i = 0; i < allMechs.Length; ++i) {
            if (!allMechs[i].ScriptDestroyed) {
                mechs.Add(allMechs[i]);
                if (allMechs[i].mainCube) {
                    if (mainCube)
                        Debug.LogError("Multiple Main Cubes detected");
                    mainCube = allMechs[i];
                }
            }
        }
        if (!mainCube)
            Debug.LogError("No Main Cube Detected");
        
    }

    public static void ResetUnitList() {
        UnitControl[] allUnits = FindObjectsOfType<UnitControl>();
        units.Clear();
        for (int i = 0; i < allUnits.Length; ++i) {
            units.Add(allUnits[i]);
        }
    }

    public static void ResetEnviormentObjects() {
        EnviormentObject[] allObjs = FindObjectsOfType<EnviormentObject>();
        envObjects.Clear();
        for (int i = 0; i < allObjs.Length; ++i) {
            envObjects.Add(allObjs[i]);
        }
    }

    public static void AddMechToList(MechanicalPart mechPart) {
        if (canPlayGameFromEnchantedSelectionComingBack) {
            mechs.Add(mechPart);
        } else {
            SelectingParts.Deselect();
            Destroy(mechPart.gameObject);
        }
    }

    public void ToggleGameState() {
        if (currentGameState == GameState.PLAYING) {
            StopGame();
        }
        else {
            PlayGame();
        }
        SelectingParts.Deselect();
    }

    public static void SetTimeScale(float value) {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void SetHeaderUIButtons(HeaderUI h) {
        headerUI = h;
    }

    public void StopGame() {
        if (!notWorkinDueTutorial && !pausedInput && canPlayGameFromEnchantedSelectionComingBack) {
            if (currentGameState == GameState.PLAYING) {
                SetTimeScale(1);
                isSlowmotion = false;

                foreach (MechanicalPart m in mechs) {
                    if (m) {
                        m.SetEnabledState(false);
                    }
                }
                foreach (UnitControl u in units) {
                    if (u) {
                        u.SetEnabledState(false);
                    }
                }
                foreach (EnviormentObject e in envObjects) {
                    if (e) {
                        e.SetEnabledState(false);
                    }
                }

                creationBounds.SpawnBounds();

                headerUI.ShowPlayButton();
                selectingParts.ReselectAfterOneFrame();
            }

            SetGameState(GameState.CREATING);
            cameraControl.OnStopGame();
        }
    }

    public void PlayGame() {
        if (!notWorkinDueTutorial && !pausedInput && canPlayGameFromEnchantedSelectionComingBack) {
            if (currentGameState == GameState.PLAYING) {
                SetTimeScale(1);
            }

            if (currentGameState == GameState.CREATING) {
                //if (needToResetList)
                //    ResetMechList();
                //needToResetList = false;


                foreach (MechanicalPart m in mechs) {
                    if (m) {
                        m.SetEnabledState(true);
                    }
                }
                foreach (UnitControl u in units) {
                    if (u) {
                        u.SetEnabledState(true);
                    }
                }
                foreach (EnviormentObject e in envObjects) {
                    if (e) {
                        e.SetEnabledState(true);
                    }
                }

                cameraControl.OnStartGame();
                creationBounds.DeleteSpawns();

                PanelSpawnerUI.GameStarted();

                headerUI.ShowStopButton();
                selectingParts.ReselectAfterOneFrame();

                betweenLevelsMachine = SaveMachine.SaveAndGet(false);
            }
            SetGameState(GameState.PLAYING);
        }
        else {
            clickedPlayWhileNotBeingAble = true;
        }
    }

    public void Erase(MechanicalPart killedMech) {
        mechs.Remove(killedMech);
        Destroy(killedMech.gameObject);
    }

    public void RememberAllPartsTransforms() {
        rememberedTransforms = new RememberedTransform[mechs.Count];
        int i;
        for (i = 0; i < rememberedTransforms.Length; i++) {
            rememberedTransforms[i] = new RememberedTransform();
        }

        i = 0;
        foreach (MechanicalPart m in mechs) {
            rememberedTransforms[i].Remember(m.transform);
            i++;
        }
        isRememberingTransforms = true;
    }

    public void AssignAllPartsTransforms() {
        if (isRememberingTransforms) {
            StartCoroutine(AssignBackTransforms(mechs, rememberedTransforms, this, .4f, PlayGame));
            isRememberingTransforms = false;
        }
    }

    private static IEnumerator AssignBackTransforms(List<MechanicalPart> mechs, RememberedTransform[] rememberedTransforms, MonoBehaviour mono, float duration, Action playGameFunc) {
        int i = 0;
        canPlayGameFromEnchantedSelectionComingBack = false;
        foreach (MechanicalPart m in mechs) {
            rememberedTransforms[i].Assign(m.transform, mono, duration);
            i++;
        }
        i = 0;
        yield return new WaitForSeconds(duration);
        foreach (MechanicalPart m in mechs) {
            rememberedTransforms[i].AssignParents(m.transform);
            i++;
        }
        canPlayGameFromEnchantedSelectionComingBack = true;
        if (clickedPlayWhileNotBeingAble) {
            playGameFunc();
            clickedPlayWhileNotBeingAble = false;
        }
    }

    public static void Victory() {
        SetTimeScale(1f);
        headerUI.VictoryButtons();
    }

    public static void OnClickedNextLevel() { }

    public static void AddPause(MonoBehaviour mono) {
        if (pausedReasons == null)
            pausedReasons = new List<MonoBehaviour>();
        pausedReasons.Add(mono);
    }
    public static void RemovePause(MonoBehaviour mono) {
        pausedReasons.Remove(mono);
    }
    private void TestPausing() {
        pausedInput = pausedReasons.Count > 0;
    }
}

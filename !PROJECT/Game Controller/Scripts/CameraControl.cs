using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

    private Vector3 lastMousePos;
    private Vector3 currentMousePos;
    private Vector3 pos;

    [HideInInspector] public Camera cam;
    private float futureSize;

    private float cameraScalingGizmos;
    private Vector3 followDestination;
    private Transform focusedObject;
    private Toggle cameraFocusUIToggle;
    public bool focusing;
    
    private Vector2 limitTopRight = Vector3.one;
    private Vector2 limitBottomLeft = Vector3.zero;

    public float minCameraDist = -15;
    public float maxCameraDist = -60;

    private bool working;

    public const float showObjectiveDuration = 2f;
    public const float comeBackDuration = 2f;

    private void Awake() {
        if (DatabasesManager.IsValidScene()) {
            limitTopRight = DatabasesManager.GetCameraTopRightCorner();
            limitBottomLeft = DatabasesManager.GetCameraBotLeftCorner();
        }

        working = true;
        cameraFocusUIToggle = GameController.mainCanvasStatic.headerUI.cameraFocusToggle;
        cam = GetComponent<Camera>();
    }

    void Start() {

        if (cam.orthographic)
            futureSize = cam.orthographicSize;
        else
            futureSize = transform.position.z;
        lastMousePos = Input.mousePosition;
    }

    private void CheckLimits() {
        if (transform.position.x > limitTopRight.x) {
            transform.position = new Vector3(limitTopRight.x, transform.position.y, transform.position.z);
            ToggleFocus(false);
        }
        if (transform.position.y > limitTopRight.y) {
            transform.position = new Vector3(transform.position.x, limitTopRight.y, transform.position.z);
            ToggleFocus(false);
        }
        if (transform.position.x < limitBottomLeft.x) {
            transform.position = new Vector3(limitBottomLeft.x, transform.position.y, transform.position.z);
            ToggleFocus(false);
        }
        if (transform.position.y < limitBottomLeft.y) {
            transform.position = new Vector3(transform.position.x, limitBottomLeft.y, transform.position.z);
            ToggleFocus(false);
        }
    }

    private void FixedUpdate() {
        if (working) {
            FollowingLogic();

            if (movingByQuickFocus && focusedObject) {
                if ((transform.position - new Vector3(focusedObject.position.x, focusedObject.position.y, transform.position.z)).magnitude > .3f && focusing && !Input.GetMouseButtonDown(1)) {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(focusedObject.position.x, focusedObject.position.y, transform.position.z), .2f);
                } else {
                    movingByQuickFocus = false;
                }
            }
        }
    }

    void Update() {
        if (working && !GameController.pausedInput) {
            if (Input.GetButtonDown("CameraFocus")) {
                Focus();
                if (!focusing)
                    Focus();
            }

            if (Input.GetMouseButtonDown(1)) {
                lastMousePos = Input.mousePosition;
                if (focusing) {
                    ToggleFocus(false);
                }
            }

            if (Input.GetMouseButton(1)) {
                currentMousePos = Input.mousePosition;

                pos = -GlobalMousePosition.MouseToWorldConversion(currentMousePos) + GlobalMousePosition.MouseToWorldConversion(lastMousePos);
                transform.position += new Vector3(pos.x, pos.y, 0);

                lastMousePos = Input.mousePosition;
                CheckLimits();
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject()) {
                futureSize += Input.GetAxis("Mouse ScrollWheel") * GameSettings.cameraScrollSpeed;
                //if (futureSize < minSize)
                //    futureSize = minSize;
                //if (futureSize > maxSize)
                //    futureSize = maxSize;
                if (futureSize > minCameraDist)
                    futureSize = minCameraDist;
                if (futureSize < maxCameraDist)
                    futureSize = maxCameraDist;
            }
            if (cam.orthographic) {
                if (cam.orthographicSize != futureSize) {
                    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, futureSize, 1 - (GameSettings.cameraSmoothness * 0.01f));
                    if (Mathf.Abs(cam.orthographicSize - futureSize) < .01f)
                        cam.orthographicSize = futureSize;
                }
                //  not implemented for ortographic. future size is set wrong.
                cameraScalingGizmos = (GlobalMousePosition.MouseToWorldConversion(new Vector3(Screen.width, Screen.height)) - GlobalMousePosition.MouseToWorldConversion(Vector3.zero)).magnitude * GameSettings.handlesScale * .001f;
            }
            else {
                //if (cam.fieldOfView != futureSize) {
                //    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, futureSize, .1f);
                //    if (Mathf.Abs(cam.fieldOfView - futureSize) < .01f)
                //        cam.fieldOfView = futureSize;
                //}
                if (transform.position.z != futureSize) {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, futureSize), 1 - (GameSettings.cameraSmoothness * 0.01f));
                    if (Mathf.Abs(transform.position.z - futureSize) < .01f)
                        transform.position = new Vector3(transform.position.x, transform.position.y, futureSize);
                }

                cameraScalingGizmos = (GlobalMousePosition.MouseToWorldConversion(new Vector3(Screen.width, Screen.height)) - GlobalMousePosition.MouseToWorldConversion(Vector3.zero)).magnitude * GameSettings.handlesScale * .001f;
            }
        }
    }

    public float GetCameraScalingGizmos() {
        return cameraScalingGizmos;
    }

    private void FollowingLogic() {
        if (focusedObject && GameController.GetGameState() == GameController.GameState.PLAYING && focusing && !movingByQuickFocus) {
            followDestination = new Vector3(focusedObject.position.x, focusedObject.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, followDestination, .3f);
            CheckLimits();
        }
    }

    public void Focus() {
        if (!GameController.pausedInput) {
            MechanicalPart mech = GameController.selectingParts.GetSelectedObject();
            if (mech)
                focusedObject = mech.transform;
            else
                focusedObject = GameController.mainCube.transform;
            ToggleFocus(!focusing);
        }
    }

    private void ToggleFocus(bool value) {
        cameraFocusUIToggle.isOn = value;
        focusing = value;

        if (value) {
            if (GameController.GetGameState() == GameController.GameState.CREATING) {
                //StartCoroutine(MoveCameraByFocus());
                movingByQuickFocus = true;
            }
        } else { 
            focusedObject = null;
        }
    }

    public void OnStartGame() {
        ToggleFocus(false);
        if (GameSettings.snapToMainCubeOnStart) {
            Focus();
        }
    }

    public void OnStopGame() {
        ToggleFocus(false);
        if (GameSettings.snapToMainCubeOnStop) {
            Focus();
        }
    }

    private bool movingByQuickFocus;

    private IEnumerator MoveCameraByFocus() {
        yield return new WaitForEndOfFrame();
        //movingByQuickFocus = true;
        //float moveTime = .3f;
        //float startTime = Time.time;
        //Vector3 dest = new Vector3(focusedObject.position.x, focusedObject.position.y, transform.position.z);
        ////Coroutine mover = transform.MoveToDuring(this, new Vector3(focusedObject.position.x, focusedObject.position.y, transform.position.z), moveTime);
        ////while (Time.time - startTime < moveTime) {
        ////    if (!focusing || Input.GetMouseButtonDown(1))
        ////        StopCoroutine(mover);
        ////    yield return new WaitForEndOfFrame();
        ////}
        //while ((transform.position - dest).magnitude > .3f && focusing && !Input.GetMouseButtonDown(1)) {
        //   // transform.position = Vector3.Lerp(transform.position, dest, .05f);
        //    dest = new Vector3(focusedObject.position.x, focusedObject.position.y, transform.position.z);
        //    yield return new WaitForEndOfFrame();
        //}
        //movingByQuickFocus = false;
    }


    public void ChangeLimits(Vector3 newTop, Vector3 newBot) {
        limitBottomLeft = newBot;
        limitTopRight = newTop;
    }

    public void DisableThinking() {
        working = false;
    }
    public void EnableThinking() {
        working = true;
        futureSize = transform.position.z;
    }

    public void BeginShowingTheObjective(MonoBehaviour pausingMono, Vector3 objPos) {
        StartCoroutine(ShowObjective(pausingMono, objPos));
    }

    private IEnumerator ShowObjective(MonoBehaviour pausingMono, Vector3 objPos) {
        GameController.AddPause(pausingMono);
        GameController.mainCanvasStatic.panelSpawnerOpenerFader.Disappear(0);
        Vector3 initialPos = transform.position;
        Vector3 dest = new Vector3(objPos.x, objPos.y, transform.position.z);
        transform.position = dest;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSecondsOrInput(showObjectiveDuration);
        yield return new WaitForSeconds(0.2f);

        Coroutine movingCoroutine = transform.MoveToDuring(this, initialPos, comeBackDuration);

        yield return new WaitForSecondsOrInput(comeBackDuration);
        if (movingCoroutine != null) {
            StopCoroutine(movingCoroutine);
        }
        transform.position = initialPos;

        GameController.RemovePause(pausingMono);
        GameController.mainCanvasStatic.panelSpawnerOpenerFader.Appear(.3f);
    }
}

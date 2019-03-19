using UnityEngine;

public class GlobalMousePosition : MonoBehaviour {
    public static Vector3 mouseWorldPos;
    private static Ray cameraRay;
    private static Plane xy;
    public float generatePlaneAtDistanceEverytime = 0f;

    private void Awake() {
        ResetPlane();
    }

    void Update () {
        //mouseWorldPos = MouseToWorldConversion(Input.mousePosition);
        if (Camera.main.orthographic) {
            mouseWorldPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        }
        else {
            cameraRay = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

            float rayDistance;
            if (xy.Raycast(cameraRay, out rayDistance)) {
                mouseWorldPos = cameraRay.GetPoint(rayDistance);
            }
        }

        if (generatePlaneAtDistanceEverytime > 0f) {
            MakeAnotherPlane(transform.position + transform.TransformDirection(0, 0, 1) * generatePlaneAtDistanceEverytime,
                transform.TransformPoint(Vector3.right) + transform.TransformDirection(0, 0, 1) * generatePlaneAtDistanceEverytime,
                transform.TransformPoint(Vector3.up) + transform.TransformDirection(0, 0, 1) * generatePlaneAtDistanceEverytime);
        }
    }

    public static Vector3 MouseToWorldConversion(Vector3 inputMousePosition) {
        Vector3 mousePosReturn;

        if (Camera.main.orthographic) {
            mousePosReturn = new Vector3(Camera.main.ScreenToWorldPoint(inputMousePosition).x, Camera.main.ScreenToWorldPoint(inputMousePosition).y);
        }
        else {
            cameraRay = Camera.main.ScreenPointToRay(new Vector3(inputMousePosition.x, inputMousePosition.y));

            float rayDistance;
            if (xy.Raycast(cameraRay, out rayDistance)) {
                mousePosReturn = cameraRay.GetPoint(rayDistance);
            }
            else {
                Debug.LogError("Could not determine mouse world pos");
                return Vector3.zero;
            }
        }
        return mousePosReturn;
    }

    public void MakeAnotherPlane(Vector3 one, Vector3 two, Vector3 three) {
        xy.Set3Points(one, two, three);
    }

    private void ResetPlane() {
        xy.Set3Points(Vector3.right, Vector3.up, Vector3.left);
    }

    //private void OnDrawGizmos() {
    //    Gizmos.DrawSphere(mouseWorldPos, 0.2f);
    //    Gizmos.color = new Color(1, 0, 0, 1f);
    //    Gizmos.DrawSphere(transform.position + transform.TransformDirection(0, 0, 1) * generatePlaneAtDistanceEverytime, .5f);
    //    Gizmos.color = new Color(0, 0, 1, 1f);
    //    Gizmos.DrawSphere(transform.TransformPoint(Vector3.right * 2) + transform.TransformDirection(0, 0, 1) * generatePlaneAtDistanceEverytime, .5f);
    //    Gizmos.color = new Color(0, 1, 0, 1f);
    //    Gizmos.DrawSphere(transform.TransformPoint(Vector3.up * 2) + transform.TransformDirection(0, 0, 1) * generatePlaneAtDistanceEverytime, .5f);
    //}
}

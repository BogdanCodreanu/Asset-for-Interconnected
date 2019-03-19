//using System.IO;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEngine.SceneManagement;

//[CustomEditor(typeof(FeedbackReader))]
//public class FeedbackReaderEditor : Editor {

//    private float timeSee;
//    private string readingPath = "Feedback/";
//    private int frameShown;
//    private float frameShownFloat;
//    private bool playing;
//    private List<MainCubeRecordedSnap> framesRead;

//    private double startedAnimationTime;
//    private double animationCounter;
//    private int startedIndexAnimation;

//    private void Awake() {
//        readingPath = "Feedback/" + SceneManager.GetActiveScene().name;
//    }

//    public override void OnInspectorGUI() {
//        readingPath = EditorGUILayout.TextField("Reading Path", readingPath);
//        EditorGUILayout.HelpBox("Reading from Assets/" + readingPath, MessageType.None);
//        if (GUILayout.Button("Read File")) {
//            ReadLists();
//        }
//        if (framesRead != null) {
//            EditorGUILayout.HelpBox("Frames Read ! Number of frames: " + framesRead.Count, MessageType.None);
//            if (framesRead.Count > 0) {
//                EditorGUILayout.HelpBox("Total duration: " + framesRead[framesRead.Count - 1].timeTaken, MessageType.None);

//                if (!playing) {
//                    EditorGUILayout.LabelField("Frame Highlighted");
//                    frameShownFloat = EditorGUILayout.Slider(frameShownFloat, 0, framesRead.Count - 2);
//                    frameShown = Mathf.FloorToInt(frameShownFloat);
//                }

//                if (!playing) {
//                    if (GUILayout.Button("Play Simulation")) {
//                        playing = true;
//                        startedAnimationTime = EditorApplication.timeSinceStartup;
//                        startedIndexAnimation = frameShown;
//                    }
//                }
//                else {
//                    if (GUILayout.Button("Stop Simulation")) {
//                        playing = false;
//                        frameShownFloat = frameShown;
//                    }
//                }
//            }
//        }
//    }

//    protected virtual void OnEditorUpdate() {
//        if (playing) {
//            animationCounter = EditorApplication.timeSinceStartup - startedAnimationTime;
//            if (frameShown >= framesRead.Count - 2) {
//                playing = false;
//                frameShownFloat = frameShown;
//            }

//            if (animationCounter >= framesRead[frameShown + 1].timeTaken - framesRead[startedIndexAnimation].timeTaken && playing) {
//                frameShown++;
//            }

//        }
//    }

//    protected virtual void OnSceneGUI() {
//        if (framesRead != null) {
//            if (framesRead.Count > 0) {
//                DisplayShadows();
//                if (frameShown < framesRead.Count - 1) {
//                    if (playing) {
//                        DisplayShownCube(((float)animationCounter - framesRead[frameShown].timeTaken + framesRead[startedIndexAnimation].timeTaken) / (framesRead[frameShown + 1].timeTaken - framesRead[frameShown].timeTaken));
//                    }
//                    else {
//                        DisplayShownCube(frameShownFloat - Mathf.Floor(frameShownFloat));
//                    }
//                }
//                GUI.color = Color.yellow;
//                Handles.Label(framesRead[frameShown].GetPosition() + Vector3.up * 3f, "Current Snap Time: " + framesRead[frameShown].timeTaken, "box");
//            }
//        }
//    }

//    private void DisplayShadows() {
//        Handles.color = new Color(0, 0, 1, .2f);
//        foreach (MainCubeRecordedSnap snap in framesRead) {
//            Handles.CubeHandleCap(0, snap.GetPosition(), snap.GetRotation(), .5f, EventType.repaint);
//        }
//    }

//    private void DisplayShownCube(float lerper) {
//        Handles.color = new Color(1, 1, 0, 1f);
//        Handles.CubeHandleCap(0, Vector3.Lerp(framesRead[frameShown].GetPosition(), framesRead[frameShown + 1].GetPosition(), lerper),
//            Quaternion.Lerp(framesRead[frameShown].GetRotation(), framesRead[frameShown + 1].GetRotation(), lerper), .5f, EventType.repaint);
//        Handles.color = new Color(0, .5f, 1, .8f);
//        Handles.SphereHandleCap(0, Vector3.Lerp(framesRead[frameShown].GetMousePos(), framesRead[frameShown + 1].GetMousePos(), lerper),
//            Quaternion.identity, 1f, EventType.repaint);
//    }



//    public void ReadLists() {
//        using (Stream stream = File.Open(Application.dataPath + "/" + readingPath, FileMode.Open)) {
//            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

//            framesRead = (List<MainCubeRecordedSnap>)bformatter.Deserialize(stream);
//        }
//    }

//    protected virtual void OnEnable() {
//#if UNITY_EDITOR
//        EditorApplication.update += OnEditorUpdate;
//#endif
//    }

//    protected virtual void OnDisable() {
//#if UNITY_EDITOR
//        EditorApplication.update -= OnEditorUpdate;
//#endif
//    }

//}

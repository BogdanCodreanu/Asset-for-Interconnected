using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public const string MainMenuSceneName = "Main Menu New";

    public LoadingScreen loadingScreenPrefab;
    private static LoadingScreen loadingScreenStaticPrefab;

    private DatabaseSceneInfo databaseSceneInfo;
    private static MonoBehaviour mono;

    private SceneInfo currentSceneInfo;

    private void Awake() {
        mono = this as MonoBehaviour;
        loadingScreenStaticPrefab = loadingScreenPrefab;
    }
    public static void GoToMainMenu() {
        LoadSceneWithLoadingScreen(MainMenuSceneName, false);
    }

    public static void LoadSceneWithLoadingScreen(string sceneName, bool needToPressAnything = true) {
        mono.StartCoroutine(LoadSceneAsync(sceneName, needToPressAnything));
    }

    private static IEnumerator LoadSceneAsync(string sceneName, bool needToPressAnything) {
        Scene oldScene = SceneManager.GetActiveScene();

        LoadingScreen loadingScreen = Instantiate(loadingScreenStaticPrefab).GetComponent<LoadingScreen>();

        AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loader.allowSceneActivation = false;
        
        while (!loader.isDone) {
            loadingScreen.SetLoadingSlider(loader.progress / 0.9f);

            if (loader.progress == 0.9f) {
                if (needToPressAnything) {
                    loadingScreen.PressAnyKeyTurnOn();

                    if (Input.anyKey) {
                        loader.allowSceneActivation = true;
                    }

                } else {
                    loader.allowSceneActivation = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        SceneManager.UnloadSceneAsync(oldScene);
    }
}

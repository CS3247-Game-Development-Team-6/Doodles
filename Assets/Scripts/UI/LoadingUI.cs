using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {
    public static string loadingUIName { get; private set; }
    public Image slider;
    private List<string> scenesToLoad;
    private List<AsyncOperation> scenesLoading;

    private float totalSceneProgress;

    private void Start() {
        loadingUIName = name;
    }

    public void AddSceneToLoad(string sceneName) {
        if (scenesToLoad == null) scenesToLoad = new List<string>();
        scenesToLoad.Add(sceneName);
    }

    public void StartLoad() {
        scenesLoading = new List<AsyncOperation>();

        foreach (string load in scenesToLoad) {
            scenesLoading.Add(SceneManager.LoadSceneAsync(load));
        }

        if (scenesLoading.Count != 0) StartCoroutine(GetSceneLoadProgress());
    }

    public void GotoScene(string sceneName) {
        SaveSceneToPref(sceneName);
        this.gameObject.SetActive(true);
        AddSceneToLoad(sceneName);
        StartLoad();
    }

    public void SaveSceneToPref(string sceneName) {
        int latestSceneIndex = 1;
        int currSceneIndex = 0;
        if (PlayerPrefs.HasKey("latestSceneIndex")) {
            latestSceneIndex = PlayerPrefs.GetInt("latestSceneIndex");
        }
        switch (sceneName) {
            case "tutorial_scene_dialogue":
                currSceneIndex = 1;
                break;
            case "spider_theme_dialogue":
                currSceneIndex = 3;
                break;
            case "graveyard_theme_dialogue":
                currSceneIndex = 5;
                break;
            case "clown_theme_dialogue":
                currSceneIndex = 7;
                break;
            case "TutorialScene-beta":
                currSceneIndex = 2;
                break;
            case "SpiderScene-beta":
                currSceneIndex = 4;
                break;
            case "GhostScene-beta":
                currSceneIndex = 6;
                break;
            case "ClownScene-beta":
                currSceneIndex = 8;
                break;
            default: // loadout and menu-beta
                break;
        }
        if (currSceneIndex > latestSceneIndex) {
            latestSceneIndex = currSceneIndex;
        }
        PlayerPrefs.SetInt("latestSceneIndex", latestSceneIndex);
    }

    public IEnumerator GetSceneLoadProgress() {
        for (int i = 0; i < scenesLoading.Count; i++) {
            while (!scenesLoading[i].isDone) {
                totalSceneProgress = 0;

                foreach (AsyncOperation op in scenesLoading) {
                    totalSceneProgress += op.progress;
                }

                totalSceneProgress = totalSceneProgress / (float)scenesLoading.Count;
                slider.fillAmount = totalSceneProgress;
                yield return null;
            }
        }
        gameObject.SetActive(false);
    }
}

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
        //SaveSceneToPref(sceneName);
        this.gameObject.SetActive(true);
        AddSceneToLoad(sceneName);
        StartLoad();
    }

    public void SaveSceneToPref(string sceneName) {
        int currSceneIndex = 0;
        int latestSceneIndex = PlayerPrefs.HasKey("latestSceneIndex") ? PlayerPrefs.GetInt("latestSceneIndex") : 1;

        if (MainMenu.staticMapInfos == null) {
            Debug.LogWarning("No mapInfos found, please reload from MainMenu");
            return;
        }

        for (int i = 0; i < MainMenu.staticMapInfos.Length; i++) {
            if (sceneName == MainMenu.staticMapInfos[i].dialogueSceneName) {
                currSceneIndex = GetDialogueSceneIndex(i);
            } else if (sceneName == MainMenu.staticMapInfos[i].gameSceneName) {
                currSceneIndex = GetGameSceneIndex(i);
            } else if (sceneName == "story-end") {
                currSceneIndex = 9;
                break;
            }
        }

        int GetDialogueSceneIndex(int lvl) => lvl * 2 + 1;
        int GetGameSceneIndex(int lvl) => (lvl + 1) * 2;

        latestSceneIndex = currSceneIndex > latestSceneIndex ? currSceneIndex : latestSceneIndex;
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

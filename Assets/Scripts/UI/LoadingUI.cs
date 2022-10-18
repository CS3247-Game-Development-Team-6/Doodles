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

    public IEnumerator GetSceneLoadProgress() {
        for (int i = 0; i < scenesLoading.Count; i++) {
            while (!scenesLoading[i].isDone) {
                totalSceneProgress = 0;

                foreach(AsyncOperation op in scenesLoading) {
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

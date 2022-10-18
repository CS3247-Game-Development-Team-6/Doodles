using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loadout : MonoBehaviour {
    public TextMeshProUGUI title;
    public LoadingUI loadingScreen;
    public Shop shop;
    public EnemyInventoryUI enemyInventoryUI;
    public static MapInfo mapToLoad;
    public List<TowerInfo> towersToLoad;
    public GameObject shopSlotPrefab;

    private void Start() {
        if (!mapToLoad) {
            Debug.Log("No map to load");
            return;
        }

        if (enemyInventoryUI != null) {
            enemyInventoryUI.LoadEnemies(mapToLoad);
        }

        title.text = mapToLoad.levelName;
        towersToLoad = new List<TowerInfo>();
    }

    public void StartGame() {
        if (!mapToLoad) {
            Debug.Log("No map to load");
            return;
        }


        towersToLoad = shop.GetTowersForLoading();
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.AddSceneToLoad(mapToLoad.gameSceneName);
        loadingScreen.StartLoad();
    }
}

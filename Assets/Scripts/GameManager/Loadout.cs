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
    public GameObject containerPrefab;

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
        LoadoutContainer loadout = FindObjectOfType<LoadoutContainer>();
        GameObject loadoutContainerObj;
        if (loadout == null) {
            loadoutContainerObj = Instantiate(containerPrefab);
            loadout = loadoutContainerObj.GetComponent<LoadoutContainer>();
            DontDestroyOnLoad(loadoutContainerObj);
            Debug.Log($"created loadout container {loadoutContainerObj}");
        } else {
            loadoutContainerObj = loadout.gameObject;
            Debug.Log($"updated loadout container {loadoutContainerObj}");
        }

        loadout.towersToLoad = shop.GetTowersForLoading();
        loadout.shopSlotPrefab = shopSlotPrefab;
        loadoutContainerObj.SetActive(true);


        if (!mapToLoad) {
            Debug.Log("No map to load");
            return;
        }

        loadingScreen.GotoScene(mapToLoad.gameSceneName);
    }
}

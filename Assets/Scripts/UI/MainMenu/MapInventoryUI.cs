using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapInventoryUI : MonoBehaviour {

    public List<MapSlotUI> mapList { get; private set; }
    public Button startGameButton;
    public string loadoutSceneName;
    public LoadingUI loadingScreen;

    public MapSlotUI selectedSlot { get; private set; }

    private void Update() {
        bool canStart = selectedSlot != null;
        startGameButton.enabled = canStart;
        startGameButton.interactable = canStart;
    }

    public int Subscribe(MapSlotUI slot) {
        if (mapList == null) mapList = new List<MapSlotUI>();
        int index = mapList.Count;
        mapList.Add(slot);
        return index;
    }

    public void SelectMap(MapSlotUI map) {
        if (selectedSlot != null) {
            selectedSlot.background.sprite = selectedSlot == map ? selectedSlot.selectedBg : selectedSlot.unselectedBg;
            selectedSlot.background.color = selectedSlot == map ? selectedSlot.selectedBgColor : selectedSlot.unselectedBgColor;
        }
        selectedSlot = map;
    }

    public void StartMap() {
        //  TODO: Add loading screen and trigger the starting of the correct map
        if (!loadingScreen || !selectedSlot || !selectedSlot.mapInfo) {
            Debug.Log("Missing loading screen or slot/map not selected.");
            return;
        }
        Debug.Log($"Loading {selectedSlot.mapInfo.levelName}");
        Loadout.mapToLoad = selectedSlot.mapInfo;
        loadingScreen.GotoScene(loadoutSceneName);
        Destroy(this);
    }

}

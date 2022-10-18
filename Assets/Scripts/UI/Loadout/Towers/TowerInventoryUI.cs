using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerInventoryUI : MonoBehaviour {
    public List<TowerSlotUI> inventoryList { get; private set; }

    private HashSet<int> selectedIndexes;

    [Header("UI Components")]
    [SerializeField] private TowerDescriptionUI towerDescriptionUI;
    [SerializeField] private Shop shop;
    public Button startGameButton;
    private int maxSlots;


    void Start() {
        selectedIndexes = new HashSet<int>();
        maxSlots = shop.MaxSlots;
    }

    public int Subscribe(TowerSlotUI slot) {
        if (inventoryList == null) inventoryList = new List<TowerSlotUI>();
        int index = inventoryList.Count;
        inventoryList.Add(slot);
        return index;
    }

    void Update() {
        startGameButton.enabled = shop.slots != null && shop.slots.Count == shop.MaxSlots;
        startGameButton.interactable = shop.slots != null && shop.slots.Count == shop.MaxSlots;
    }

    public void HoverTower(TowerInfo towerInfo, Sprite sprite) {
        towerDescriptionUI.SetInfo(towerInfo, sprite);
    }


    public void UnhoverTower() {
        towerDescriptionUI.ResetInfo();
    }


    public bool SelectTower(TowerSlotUI slot) {
        if (selectedIndexes.Contains(slot.Index)) {
            shop.Delete(slot);
            selectedIndexes.Remove(slot.Index);
            return false;
        } else if (selectedIndexes.Count < maxSlots) {
            towerDescriptionUI.SelectInfo(slot.towerInfo);
            shop.Add(slot);
            selectedIndexes.Add(slot.Index);
            return true;
        } else {
            Debug.Log($"selected {selectedIndexes.Count} out of {maxSlots}");
            towerDescriptionUI.SetInfo(slot.towerInfo, slot.image.sprite);
            return false;
        }
    }


}

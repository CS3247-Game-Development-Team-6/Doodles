using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    public CardManager defaultTower { get; private set; }
    [SerializeField] public CardManager selectedTower;
    [SerializeField] public ShopTowerUI currentTower;
    [SerializeField] private ParticleSystem invalidAction;
    [SerializeField] PlayerMovement playerMovement;
    public List<ShopTowerUI> emptySlots { get; private set; }
    public List<ShopTowerUI> slots { get; private set; }
    public int MaxSlots { get; private set; }

    private void Start() {
        emptySlots = new List<ShopTowerUI>();
        slots = new List<ShopTowerUI>();
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            ShopTowerUI slot = child.GetComponent<ShopTowerUI>();
            if (slot == null) continue;
            slot.Index = i;
            if (slot.towerInfo == null) {
                emptySlots.Add(slot);
            } else {
                slots.Add(slot);
            }
        }
        MaxSlots = emptySlots.Count + slots.Count;
    }

    public void Add(TowerSlotUI slot) {
        if  (emptySlots.Count > 0) {
            emptySlots.Sort();
            ShopTowerUI newShopTower = emptySlots[0];
            newShopTower.towerInfo = slot.towerInfo;
            emptySlots.RemoveAt(0);
            slots.Add(newShopTower);
        } else {
            Debug.LogWarning("slots full");
        }
    }

    public void Delete(TowerSlotUI slot) {
        foreach (var s in slots) {
            if (slot.towerInfo == s.towerInfo) {
                s.towerInfo = null;
                slots.Remove(s);
                emptySlots.Add(s);
                return;
            } 
        }
        Debug.LogWarning($"Slot {slot} not found");
    }

    // Deprecating
    public void SetDefaultTower(CardManager defaultTower) {
        this.defaultTower = defaultTower;
    }

    private void TriggerInvalidAction() {
        Instantiate(invalidAction, transform);
    }

    // Deprecating
    public void SetTowerToBuild(CardManager item) {
        if (playerMovement && playerMovement.GetIsBuilding()) {
            TriggerInvalidAction();
            return;
        }
        item.gameObject.GetComponent<Image>().sprite = item.selected;
        if (selectedTower != null && selectedTower != item) {
            selectedTower.gameObject.GetComponent<Image>().sprite = selectedTower.unselected;
        }

        selectedTower = item;
        TowerInfo towerInfo = item.towerInfo;
        TowerManager.instance.SetTowerToBuild(towerInfo);
    }

    public void SetTowerToBuild(ShopTowerUI item) {
        if (playerMovement && playerMovement.GetIsBuilding()) {
            TriggerInvalidAction();
            return;
        }
        item.gameObject.GetComponent<Image>().sprite = item.image.sprite;
        if (currentTower != null && currentTower != item) {
            currentTower.gameObject.GetComponent<Image>().sprite = currentTower.unselectedBg;
        }

        currentTower = item;
        TowerInfo towerInfo = item.towerInfo;
        TowerManager.instance.SetTowerToBuild(towerInfo);
    }
}

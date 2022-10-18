using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    // Deprecating
    public CardManager defaultTower { get; private set; }
    // Deprecating
    [SerializeField] public CardManager selectedTower;

    [SerializeField] public ShopTowerUI currentTower;
    [SerializeField] private ParticleSystem invalidAction;
    [SerializeField] PlayerMovement playerMovement;
    public List<ShopTowerUI> emptySlots { get; private set; }
    public List<ShopTowerUI> slots { get; private set; }
    public int MaxSlots { get; private set; }
    public bool isSettingInventory;

    private void Start() {
        if (!isSettingInventory) {
            //LoadTowersIntoShop(FindObjectOfType<Loadout>());
            LoadoutContainer loadout = FindObjectOfType<LoadoutContainer>();
            LoadTowersIntoShop(loadout);
            Destroy(loadout.gameObject);
        } else {
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

    public List<TowerInfo> GetTowersForLoading() {
        List<TowerInfo> towerInfos = new List<TowerInfo>();
        foreach (var s in slots) {
            towerInfos.Add(s.towerInfo);
        }
        return towerInfos;
    }

    public void LoadTowersIntoShop(Loadout loadout) {
        if (!loadout) {
            Debug.LogWarning("No Loadout found, default inventory displayed");
            return;
        }

        List<TowerInfo> towerInfos = loadout.towersToLoad;
        GameObject slotPrefab = loadout.shopSlotPrefab;
        if (!slotPrefab.GetComponent<ShopTowerUI>()) {
            Debug.LogError($"Shop Slot prefab {name} is not of type TowerSlotUI");
            return;
        }

        // Start from blank
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        emptySlots = new List<ShopTowerUI>();
        slots = new List<ShopTowerUI>();
        foreach (var towerInfo in towerInfos) {
            ShopTowerUI shopSlot = Instantiate(slotPrefab, transform).GetComponent<ShopTowerUI>();
            shopSlot.towerInfo = towerInfo;
            slots.Add(shopSlot);
        }
    }

    public void LoadTowersIntoShop(LoadoutContainer loadout) {
        if (!loadout) {
            Debug.LogWarning("No Loadout found, default inventory displayed");
            return;
        }

        List<TowerInfo> towerInfos = loadout.towersToLoad;
        GameObject slotPrefab = loadout.shopSlotPrefab;
        if (!slotPrefab.GetComponent<ShopTowerUI>()) {
            Debug.LogError($"Shop Slot prefab {name} is not of type TowerSlotUI");
            return;
        }

        // Start from blank
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        emptySlots = new List<ShopTowerUI>();
        slots = new List<ShopTowerUI>();
        foreach (var towerInfo in towerInfos) {
            ShopTowerUI shopSlot = Instantiate(slotPrefab, transform).GetComponent<ShopTowerUI>();
            shopSlot.towerInfo = towerInfo;
            slots.Add(shopSlot);
        }
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

        // Redirects to the current default building method
        if (item == null) { 
            SetDefaultTowerToBuild();
            return;
        }

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

    public void SetDefaultTowerToBuild() {
        if (slots == null || slots.Count == 0) return;
        if (currentTower == null) currentTower = slots[0];
        SetTowerToBuild(currentTower);
    }

    public void SetTowerToBuild(ShopTowerUI item) {
        if (TowerManager.instance == null) {
            Debug.LogError("No TowerManager found");
            return;
        }

        if (playerMovement && playerMovement.GetIsBuilding()) {
            TriggerInvalidAction();
            return;
        }
        item.background.sprite = item.selectedBg;
        if (currentTower != null && currentTower != item) {
            currentTower.background.sprite = currentTower.unselectedBg;
        }

        currentTower = item;
        TowerInfo towerInfo = item.towerInfo;
        TowerManager.instance.SetTowerToBuild(towerInfo);
    }
}

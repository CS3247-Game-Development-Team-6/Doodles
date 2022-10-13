using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    /**
     * loadout tower selection
     */
    public int Index { get; private set; }
    private TowerInventoryUI inventoryUI;
    private Shop shop;

    /**
     * in-game tower building
     */
    public Image image;
    public TextMeshProUGUI costText;
    public TowerInfo towerInfo;

    void Start() {
        shop = GetComponentInParent<Shop>();
        inventoryUI = GetComponentInParent<TowerInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");

        if (towerInfo == null) return;

        if (costText != null) costText.text = towerInfo.cost.ToString();
        if (towerInfo.sprite != null) {
            image.sprite = towerInfo.sprite;
            image.enabled = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (TowerInventoryUI.isInGame && shop != null) {
            SetTowerInShop();
        } else {
            inventoryUI.SelectTower(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryUI.HoverTower(towerInfo, image.sprite);
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventoryUI.UnhoverTower();
    }

    public void SetTowerInShop() {
        if (!shop) {
            Debug.LogError("Parent of this button is not a shop.");
            return;
        } else if (!towerInfo) {
            Debug.LogError("Tower not set for this button.");
            return;
        }

        shop.SetTowerToBuild(this);
    }

}

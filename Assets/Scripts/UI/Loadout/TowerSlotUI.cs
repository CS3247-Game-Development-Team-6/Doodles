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

    /**
     * in-game tower building
     */
    public Image image;
    public Image background;
    public TextMeshProUGUI costText;
    public TowerInfo towerInfo;
    public Sprite selectedBg;
    public Sprite unselectedBg;

    void Start() {
        inventoryUI = GetComponentInParent<TowerInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");

        if (towerInfo == null) return;

        if (costText != null) costText.text = towerInfo.cost.ToString();
        if (towerInfo.sprite != null) {
            background.sprite = unselectedBg;
            image.sprite = towerInfo.sprite;
            image.enabled = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!TowerInventoryUI.isInGame) {
            bool isInLoadout = inventoryUI.SelectTower(this);
            background.sprite = isInLoadout ? selectedBg : unselectedBg;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryUI.HoverTower(towerInfo, image.sprite);
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventoryUI.UnhoverTower();
    }

}

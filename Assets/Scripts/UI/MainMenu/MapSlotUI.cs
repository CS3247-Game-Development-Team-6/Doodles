using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public int Index { get; private set; }
    public Image image;
    public Image background;
    public TextMeshProUGUI labelText;
    public Color selectedBgColor;
    public Sprite selectedBg;
    public Color unselectedBgColor;
    public Sprite unselectedBg;
    public MapInfo mapInfo;
    private MapInventoryUI inventoryUI;

    void Start() {
        inventoryUI = GetComponentInParent<MapInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");
        if (mapInfo != null) {
            if (image != null) image.sprite = mapInfo.levelPreview;
            if (labelText != null) labelText.text = mapInfo.levelName;
        }
        background.sprite = inventoryUI.selectedSlot == this ? selectedBg : unselectedBg;
        background.color = inventoryUI.selectedSlot == this ? selectedBgColor : unselectedBgColor;
    }

    public void OnPointerDown(PointerEventData eventData) {
        inventoryUI.SelectMap(this);
        background.sprite = selectedBg;
        background.color = selectedBgColor;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        background.sprite = selectedBg;
        background.color = selectedBgColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        background.sprite = inventoryUI.selectedSlot == this ? selectedBg : unselectedBg;
        background.color = inventoryUI.selectedSlot == this ? selectedBgColor : unselectedBgColor;
    }
}


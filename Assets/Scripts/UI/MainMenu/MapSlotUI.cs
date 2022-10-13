using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public int Index { get; private set; }
    public Image image;
    public Image background;
    public TextMeshProUGUI labelText;
    public Sprite selectedBg;
    public Sprite unselectedBg;
    public MapInfo mapInfo;
    private MapInventoryUI inventoryUI;

    void Start() {
        inventoryUI = GetComponentInParent<MapInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");

    }

    private void Update() {
        if (mapInfo == null) return;

        labelText.text = mapInfo.levelName;
    }

    public void OnPointerDown(PointerEventData eventData) {
        inventoryUI.SelectMap(this);
        background.sprite = selectedBg;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        background.sprite = selectedBg;
    }

    public void OnPointerExit(PointerEventData eventData) {
        background.sprite = inventoryUI.selectedSlot == this ? selectedBg : unselectedBg;
    }
}


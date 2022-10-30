using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public int index;
    public Image image;
    public Image background;
    public TextMeshProUGUI labelText;
    public Color selectedBgColor;
    public Sprite selectedBg;
    public Color unselectedBgColor;
    public Sprite unselectedBg;
    public MapInfo mapInfo;
    private MapInventoryUI inventoryUI;
    public Color lockedBgColor;

    void Start() {
        inventoryUI = GetComponentInParent<MapInventoryUI>();
        if (inventoryUI != null) inventoryUI.Subscribe(this); //has removed index since it is not fixed, depends on who started mapSlotUI first
        else Debug.LogError($"inventoryUI not found in parent of {name}");
        if (mapInfo != null) {
            if (image != null) image.sprite = mapInfo.levelPreview;
            if (labelText != null) labelText.text = mapInfo.levelName;
        }
        background.sprite = inventoryUI.selectedSlot == this ? selectedBg : unselectedBg;
        if (PlayerPrefs.HasKey("latestSceneIndex")) {
            if (index <= PlayerPrefs.GetInt("latestSceneIndex") / 2 - 1) {
                background.color = inventoryUI.selectedSlot == this ? selectedBgColor : unselectedBgColor;
            } else {
                background.color = lockedBgColor;
            }
        } else {
            background.color = lockedBgColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (index > PlayerPrefs.GetInt("latestSceneIndex") / 2 - 1) {
            return;
        }

        inventoryUI.SelectMap(this);
        background.sprite = selectedBg;
        background.color = selectedBgColor;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (index > PlayerPrefs.GetInt("latestSceneIndex") / 2 - 1) {
            return;
        }
        background.sprite = selectedBg;
        background.color = selectedBgColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (index > PlayerPrefs.GetInt("latestSceneIndex") / 2 - 1) {
            return;
        }
        background.sprite = inventoryUI.selectedSlot == this ? selectedBg : unselectedBg;
        background.color = inventoryUI.selectedSlot == this ? selectedBgColor : unselectedBgColor;
    }
}


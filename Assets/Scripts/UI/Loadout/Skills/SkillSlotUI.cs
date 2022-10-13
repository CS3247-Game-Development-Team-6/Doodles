using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlotUI : MonoBehaviour {
    public int Index { get; private set; }
    public Image image;
    public Image background;
    public TextMeshProUGUI costText;
    public Sprite selectedBg;
    public Sprite unselectedBg;
    private SkillInventoryUI inventoryUI;

    void Start() {
        inventoryUI = GetComponentInParent<SkillInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");

    }

    public void OnPointerDown(PointerEventData eventData) {
        inventoryUI.Select();
        Debug.LogWarning("Skill info not implemented yet");
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryUI.Hover();
        Debug.LogWarning("Skill info not implemented yet");
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventoryUI.Unhover();
        Debug.LogWarning("Skill info not implemented yet");
    }
}

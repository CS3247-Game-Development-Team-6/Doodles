using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class SpellSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler  {
    public int Index { get; private set; }
    public Image image;
    public Image background;
    public TextMeshProUGUI costText;
    public Sprite selectedBg;
    public Sprite unselectedBg;
    private SpellInventoryUI inventoryUI;

    public SpellUI spellUi;
    public SpellInfo spellInfo;

    void Start() {
        inventoryUI = GetComponentInParent<SpellInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");

    }

    private void Update() {
        if (spellInfo) {
            image.sprite = spellInfo.sprite;
            costText.text = spellInfo.cost.ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        inventoryUI.Select(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryUI.Hover(spellInfo);
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventoryUI.Unhover();
    }
}

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
    public Color addedToInventoryColor = Color.white;
    private SpellInventoryUI inventoryUI;

    public SpellUI spellUi;
    public SpellInfo spellInfo;
    private bool selectable;

    void Start() {
        selectable = true;
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

    public void DisableSelect() {
        background.color = addedToInventoryColor;
        background.sprite = unselectedBg;
        selectable = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (selectable) inventoryUI.Select(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryUI.Hover(spellInfo);
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventoryUI.Unhover();
    }
}

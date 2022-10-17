using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class EnemySlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public int Index { get; private set; }
    public Image image;
    public Image background;
    public TextMeshProUGUI earnText;
    public Sprite selectedBg;
    public Sprite unselectedBg;
    public EnemyInfo enemyInfo;
    private EnemyInventoryUI inventoryUI;

    void Start() {
        inventoryUI = GetComponentInParent<EnemyInventoryUI>();
        if (inventoryUI != null) Index = inventoryUI.Subscribe(this);
        else Debug.LogError($"inventoryUI not found in parent of {name}");

        if (enemyInfo == null) return;
        if (earnText != null) earnText.text = $"+${enemyInfo.inkGained}";
        if (image != null) image.sprite = enemyInfo.sprite;
    }

    public void OnPointerDown(PointerEventData eventData) {
        inventoryUI.Select(this);
        background.sprite = inventoryUI.currentEnemySelected == this ? selectedBg : unselectedBg;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryUI.Hover(enemyInfo);
        background.sprite = selectedBg;
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventoryUI.Unhover();
        background.sprite = inventoryUI.currentEnemySelected == this ? selectedBg : unselectedBg;
    }
}


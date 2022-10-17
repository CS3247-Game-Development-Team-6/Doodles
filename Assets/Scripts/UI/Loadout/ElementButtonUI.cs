using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementButtonUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    private TowerDescriptionUI descUI;
    [SerializeField] private bool isUpgrade;
    [SerializeField] private ElementType type;
    private TowerInfo previousTowerInfo;
    private void Start() {
        descUI = GetComponentInParent<TowerDescriptionUI>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // TODO: Remember to change the Sprite once updated
        if (!descUI) return;
        previousTowerInfo = descUI.towerInfo;
        if (!previousTowerInfo) return;
        if (isUpgrade) {
            descUI.SetInfo(descUI.towerInfo.nextUpgrade, false);
        } else {
            foreach (var elementTower in descUI.towerInfo.nextElements) {
                if (elementTower.element != type) continue;
                descUI.SetInfo(elementTower.tower, false);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData) {
        if (!descUI) return;
        descUI.SetInfo(previousTowerInfo);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!descUI) return;
        // For shop
    }
}

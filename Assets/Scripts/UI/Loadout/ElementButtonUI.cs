using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementButtonUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    private TowerDescriptionUI desc;
    [SerializeField] private bool isUpgrade;
    [SerializeField] private ElementType type;
    private TowerInfo previousTowerInfo;
    private void Start() {
        desc = GetComponentInParent<TowerDescriptionUI>();

        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // TODO: Remember to change the Sprite once updated
        if (!desc) return;
        previousTowerInfo = desc.towerInfo;
        if (isUpgrade) {
            desc.SetInfo(desc.towerInfo.nextUpgrade, desc.sprite);
        } else {
            foreach (var elementTower in desc.towerInfo.nextElements) {
                if (elementTower.element != type) continue;
                desc.SetInfo(elementTower.tower, desc.sprite);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData) {
        if (!desc) return;
        desc.SetInfo(previousTowerInfo, desc.sprite);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!desc) return;
        // For shop
    }
}

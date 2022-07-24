using UnityEngine;
using UnityEngine.EventSystems;

public class NodeUITooltipTrigger : TooltipTrigger {
    public TowerInfo currentTower;
    public TowerInfo newTower;

    public void SetTowerInfo(TowerInfo currentTower, TowerInfo newTower) {
        this.currentTower = currentTower;
        this.newTower = newTower;
    }

    private string FormatContent() {
        return content;
    }

    private string FormatHeader() {
        return header;
    }
    
    public override void OnPointerEnter(PointerEventData eventData) {
        TooltipSystem.Show(FormatContent(), FormatHeader());
    }
}

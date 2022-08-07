using UnityEngine.EventSystems;

public class NodeUITooltipTrigger : TooltipTrigger {
    private TowerInfo currentTower;
    private TowerInfo newTower;
    private bool isUpgrade;
    public bool isNotAvailable { get; set; }

    public void SetTowerInfo(TowerInfo currentTower, TowerInfo newTower) {
        this.currentTower = currentTower;
        this.newTower = newTower;
        this.isUpgrade = currentTower.element == newTower.element 
            && currentTower.upgradeNum < newTower.upgradeNum; 
    }

    private string FormatContent() {
        if (!newTower || !currentTower) return content;
        int damageDiff = newTower.damage - currentTower.damage;
        float rangeDiff = newTower.range - currentTower.range;

        string effect = isUpgrade ? $"Upgrade to level {newTower.upgradeNum}" : $"Effect: {newTower.element.effect.Name}";
        string pattern = $"{effect}\nDMG: {newTower.damage} [{damageDiff}]\nRange: {newTower.range} [{rangeDiff}]";
        return pattern;
    }

    private string FormatHeader() {
        if (!newTower || !currentTower) return header;
        return header + $@" [${newTower.cost}]";
    }
    
    public override void OnPointerEnter(PointerEventData eventData) {
        if (!isNotAvailable) {
            TooltipSystem.Show(FormatContent(), FormatHeader());
        }
    }
}

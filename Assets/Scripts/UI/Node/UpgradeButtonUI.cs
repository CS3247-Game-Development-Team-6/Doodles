using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButtonUI : NodeButtonUI {
    private NodePanelUI nodePanel;
    private TowerInfo myTowerInfo;
    private void Start() {
        nodePanel = GetComponentInParent<NodePanelUI>();
        action = ActionType.UPGRADE;
        Init();
    }

    public override void SetTower(Tower tower) {
        base.SetTower(tower);
        if (tower.nextUpgrade == null) {
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
            myTowerInfo = null;
        } else {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            myTowerInfo = tower.nextUpgrade;
            costUi.text = tower.nextUpgrade.cost.ToString();
        }
    }

    public override void OnPointerEnter(PointerEventData e) {
        nodePanel.CompareTower(myTowerInfo);
    }

    public override void OnPointerExit(PointerEventData e) {
        nodePanel.ResetTowerInfo();
    }

    public override void OnPointerDown(PointerEventData e) {
        Debug.Log("charging and upgrading");
        TowerManager.instance.UpgradeTower();
    }
}

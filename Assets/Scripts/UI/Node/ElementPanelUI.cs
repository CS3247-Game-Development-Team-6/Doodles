using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementPanelUI : NodeButtonUI {
    private NodePanelUI nodePanel;
    private TowerInfo myTowerInfo;
    public ElementInfo element;

    private void Start() {
        action = ActionType.ELEMENT;
        nodePanel = GetComponentInParent<NodePanelUI>();
        Init();
    }

    public override void SetTower(Tower tower) {
        base.SetTower(tower);
        if (tower.element == element) {
            costUi.text = "";
            canvasGroup.blocksRaycasts = false;
            return;
        } else if (tower.element != null) {
            costUi.text = "";
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
            return;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        foreach (ElementKeyValue info in tower.towerInfo.nextElements) {
            if (info.element == element.type) { 
                myTowerInfo = info.tower;
                Debug.Log($"{myTowerInfo}, {costUi}, {myTowerInfo.cost}");
                costUi.text = myTowerInfo.cost.ToString();
                break;
            }
        }
    }

    public override void OnPointerEnter(PointerEventData e) {
        nodePanel.CompareTower(myTowerInfo);
    }

    public override void OnPointerExit(PointerEventData e) {
        nodePanel.ResetTowerInfo();
    }

    public override void OnPointerDown(PointerEventData e) {
        base.OnPointerDown(e);
        Debug.Log($"Element {element}");
        TowerManager.instance.ReplaceElementTower(element);
    }
}

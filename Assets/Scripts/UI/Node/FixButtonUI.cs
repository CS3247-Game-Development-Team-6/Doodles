using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixButtonUI : NodeButtonUI {
    private void Start() {
        action = ActionType.FIX;
        Init();
    }

    private void Update() {
        if (tower) costUi.text = ((int)tower.damageFixCost).ToString();
        else return;
        if (tower.damageFixCost == 0) {
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
        } else {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public override void OnPointerDown(PointerEventData e) {
        Debug.Log("charging and fixing");
        TowerManager.instance.FixTower();
    }

}

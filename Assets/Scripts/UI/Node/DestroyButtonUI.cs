using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyButtonUI : NodeButtonUI {
    private void Start() {
        action = ActionType.UPGRADE;
        Init();
    }

    public override void OnPointerDown(PointerEventData e) {
        Debug.Log("destroying");
        TowerManager.instance.DestroyTower();
    }
}

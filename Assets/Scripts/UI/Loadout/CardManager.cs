using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public GameObject selfGO;
    public TowerSelectionManager towerSelectionManager;
    public bool isSelected;

    public CardManager parentCard;

    void Start() {

    }

    void Update() {

    }

    public void OnPointerDown(PointerEventData eventData) {
        towerSelectionManager.SelectTower(selfGO, this);
    }

    public void OnPointerUp(PointerEventData eventData) {

    }
}
